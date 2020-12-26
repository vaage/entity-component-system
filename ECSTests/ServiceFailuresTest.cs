using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECSTests
{
    [TestClass]
    public class ServiceFailuresTest
    {
        // We will need multiple services for some tests. However, if we only
        // need one, we should use "service".
        private ECS.Service service;
        private ECS.Service serviceA;
        private ECS.Service serviceB;

        [TestInitialize]
        public void SetUp()
        {
            service = new ECS.Service();
            serviceA = new ECS.Service();
            serviceB = new ECS.Service();
        }

        [TestMethod]
        public void EntityCannotContainDuplicateComponents()
        {
            ECS.Entity entity = service.CreateEntity();
            service.CreateComponent<object>(entity);

            Assert.ThrowsException<ArgumentException>(() => {
                service.CreateComponent<object>(entity);
            });
        }

        [TestMethod]
        public void ServiceWontAcceptEnternalComponents()
        {
            ECS.Entity entity = serviceA.CreateEntity();

            // Since "serviceA" owns "entity", "serviceB" should not create a
            // component for it.
            Assert.ThrowsException<ArgumentException>(() => {
                serviceB.CreateComponent<object>(entity);
            });
        }
    }
}
