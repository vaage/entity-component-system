using System;

namespace ECS
{
    public abstract class AbstractSystem
    {
        private readonly Service service;

        public AbstractSystem(Service service)
        {
            this.service = service;
        }

        public abstract void Tick();

        protected Cursor Query(params Type[] componentTypes)
        {
            return service.Query(componentTypes);
        }
    }
}
