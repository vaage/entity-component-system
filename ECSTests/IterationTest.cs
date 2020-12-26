using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECSTests
{
    [TestClass]
    public class IterationTest
    {
        private sealed class ComponentA
        {
            // We don't use this variable, but we still want to have a variable
            // in this component.
            public int _;   
        }

        private sealed class ComponentB
        {
            // We don't use this variable, but we still want to have a variable
            // in this component.
            public int _;
        }

        private ECS.Service service;

        [TestInitialize]
        public void SetUp()
        {
            service = new ECS.Service();
        }

        [TestMethod]
        public void IterateThroughSingleComponents()
        {
            var entity0 = service.CreateEntity();
            var entity1 = service.CreateEntity();
            var entity2 = service.CreateEntity();

            var component0 = service.CreateComponent<ComponentA>(entity0);
            var component1 = service.CreateComponent<ComponentA>(entity1);
            var component2 = service.CreateComponent<ComponentA>(entity2);

            var cursor = service.Query(typeof(ComponentA));

            object[] values = new object[1];

            Assert.IsTrue(cursor.Next(values));
            Assert.AreSame(component0, values[0]);

            Assert.IsTrue(cursor.Next(values));
            Assert.AreSame(component1, values[0]);

            Assert.IsTrue(cursor.Next(values));
            Assert.AreSame(component2, values[0]);

            Assert.IsFalse(cursor.Next(values));
        }

        [TestMethod]
        public void IterateThroughMultipleComponents()
        {
            var entity0 = service.CreateEntity();
            var entity1 = service.CreateEntity();
            var entity2 = service.CreateEntity();

            var componentA0 = service.CreateComponent<ComponentA>(entity0);
            var componentB0 = service.CreateComponent<ComponentB>(entity0);

            var componentA1 = service.CreateComponent<ComponentA>(entity1);
            var componentB1 = service.CreateComponent<ComponentB>(entity1);

            var componentA2 = service.CreateComponent<ComponentA>(entity2);
            var componentB2 = service.CreateComponent<ComponentB>(entity2);

            var cursor = service.Query(typeof(ComponentA), typeof(ComponentB));

            object[] values = new object[2];

            Assert.IsTrue(cursor.Next(values));
            Assert.AreSame(componentA0, values[0]);
            Assert.AreSame(componentB0, values[1]);

            Assert.IsTrue(cursor.Next(values));
            Assert.AreSame(componentA1, values[0]);
            Assert.AreSame(componentB1, values[1]);

            Assert.IsTrue(cursor.Next(values));
            Assert.AreSame(componentA2, values[0]);
            Assert.AreSame(componentB2, values[1]);

            Assert.IsFalse(cursor.Next(values));
        }

        [TestMethod]
        public void IterateThroughOnlyFullMatches()
        {
            var entity0 = service.CreateEntity();
            var entity1 = service.CreateEntity();
            var entity2 = service.CreateEntity();

            var componentA0 = service.CreateComponent<ComponentA>(entity0);
            var componentB0 = service.CreateComponent<ComponentB>(entity0);

            // Only create one of the two components for this entity so that we
            // we will skip it during iteration.
            service.CreateComponent<ComponentA>(entity1);

            var componentA2 = service.CreateComponent<ComponentA>(entity2);
            var componentB2 = service.CreateComponent<ComponentB>(entity2);

            var cursor = service.Query(typeof(ComponentA), typeof(ComponentB));

            object[] values = new object[2];

            Assert.IsTrue(cursor.Next(values));
            Assert.AreSame(componentA0, values[0]);
            Assert.AreSame(componentB0, values[1]);

            Assert.IsTrue(cursor.Next(values));
            Assert.AreSame(componentA2, values[0]);
            Assert.AreSame(componentB2, values[1]);

            Assert.IsFalse(cursor.Next(values));
        }

        [TestMethod]
        public void IterateOverMissingComponents()
        {
            service.CreateEntity();
            service.CreateEntity();
            service.CreateEntity();

            var cursor = service.Query(typeof(ComponentA), typeof(ComponentB));

            object[] values = new object[2];

            Assert.IsFalse(cursor.Next(values));
        }

        [TestMethod]
        public void IterateOverPartialMissingComponents()
        {
            var entity0 = service.CreateEntity();
            var entity1 = service.CreateEntity();
            var entity2 = service.CreateEntity();

            // Only create one type of element so that we will be missing one
            // whole group.
            service.CreateComponent<ComponentA>(entity0);
            service.CreateComponent<ComponentA>(entity1);
            service.CreateComponent<ComponentA>(entity2);

            var cursor = service.Query(typeof(ComponentA), typeof(ComponentB));

            object[] values = new object[2];

            Assert.IsFalse(cursor.Next(values));
        }

        [TestMethod]
        public void IterateOverMixedComponents()
        {
            var entity0 = service.CreateEntity();
            var entity1 = service.CreateEntity();
            var entity2 = service.CreateEntity();

            // Have some entities have some components but not others so that
            // there are no entities that have both.
            service.CreateComponent<ComponentA>(entity0);
            service.CreateComponent<ComponentB>(entity1);
            service.CreateComponent<ComponentA>(entity2);

            var cursor = service.Query(typeof(ComponentA), typeof(ComponentB));

            object[] values = new object[2];

            Assert.IsFalse(cursor.Next(values));
        }
    }
}
