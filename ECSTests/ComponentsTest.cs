using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ECSTests
{
    [TestClass]
    public class ComponentsTest
    {
        private ECS.Service service;

        [TestInitialize]
        public void SetUp()
        {
            service = new ECS.Service();
        }

        [TestMethod]
        public void CreateObjectComponent()
        {
            ECS.Entity entity = service.CreateEntity();
            object component = service.CreateComponent<object>(entity);

            Assert.IsNotNull(component);
        }

        [TestMethod]
        public void CreateNonObjectComponent()
        {
            // The type does not really matter as long as it is not "object".
            ECS.Entity entity = service.CreateEntity();
            ArgumentException component = service.CreateComponent<ArgumentException>(entity);

            Assert.IsNotNull(component);
        }
    }
}
