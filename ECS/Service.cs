using System;
using System.Collections.Generic;

namespace ECS
{
    using ComponentGroup = SortedList<int, object>;

    public sealed class Service
    {
        // Since we need these every time we create a new component, keep static
        // readonly copies to avoid memory allocations.
        private static readonly Type[] defaultConstructorTypes = { };
        private static readonly object[] defaultConstructorValues = { };

        private static int nextServicedId = 0;

        // Increment nextServiceId each time we create a service so that each
        // service will have a different id.
        private readonly int serviceId = nextServicedId++;

        private readonly Dictionary<Type, ComponentGroup> components = new Dictionary<Type, ComponentGroup>();
        private readonly ComponentGroup nullComponents = new ComponentGroup();

        private int nextEntityId = 0;
        
        public Entity CreateEntity()
        {
            return new Entity(serviceId, nextEntityId++);
        }

        public Component CreateComponent<Component>(Entity entity) where Component : class
        {
            // Make sure that this entity actually belongs to this service.
            if (entity.Service != serviceId)
            {
                throw new ArgumentException("Entity does not belong to this service.");
            }

            var componentType = typeof(Component);

            // The group will be missing the first time we create a component of
            // any type.
            if (!components.TryGetValue(componentType, out ComponentGroup group))
            {
                group = new ComponentGroup();
                components.Add(componentType, group);
            }

            if (group.ContainsKey(entity.Id))
            {
                throw new ArgumentException("Entity already contains component.");
            }

            object component = CreateComponent(componentType);
            group.Add(entity.Id, component);
            return component as Component;
        }

        public Cursor Query(params Type[] componentTypes)
        {
            var groups = new ComponentGroup[componentTypes.Length];

            for (int i = 0; i < componentTypes.Length; i++)
            {
                if (components.TryGetValue(componentTypes[i], out ComponentGroup group))
                {
                    groups[i] = group;
                }
                else
                {
                    // Assign the null components group to this slot so that we
                    // don't need to support null in our cursor object.
                    groups[i] = nullComponents;
                }
            }

            return new Cursor(groups);
        }

        private static object CreateComponent(Type type)
        {
            // User reflection to create the component. Since components are
            // just data, we assume they have a default constructor.
            return type.GetConstructor(defaultConstructorTypes).Invoke(defaultConstructorValues);
        }
    }
}