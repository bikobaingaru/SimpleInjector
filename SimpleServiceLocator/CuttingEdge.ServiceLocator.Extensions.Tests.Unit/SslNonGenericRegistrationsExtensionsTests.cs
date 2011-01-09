﻿using System;
using System.Data.SqlClient;
using System.Linq;

using CuttingEdge.ServiceLocation;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CuttingEdge.ServiceLocator.Extensions.Tests.Unit
{
    [TestClass]
    public class SslNonGenericRegistrationsExtensionsTests
    {
        private interface IService
        {
        }

        [TestMethod]
        public void RegisterSingleByInstance_ValidRegistration_GetInstanceReturnsExpectedInstance()
        {
            // Arrange
            var container = new SimpleServiceLocator();

            object impl = new ServiceImpl(null);

            // Act
            container.RegisterSingle(typeof(IService), impl);

            // Assert
            Assert.AreEqual(impl, container.GetInstance<IService>(),
                "GetInstance should return the instance registered using RegisterSingle.");
        }

        [TestMethod]
        public void RegisterSingleByInstance_ValidRegistration_GetInstanceAlwaysReturnsSameInstance()
        {
            // Arrange
            var container = new SimpleServiceLocator();

            object impl = new ServiceImpl(null);

            // Act
            container.RegisterSingle(typeof(IService), impl);

            var instance1 = container.GetInstance<IService>();
            var instance2 = container.GetInstance<IService>();

            // Assert
            Assert.AreEqual(instance1, instance2, "RegisterSingle should register singleton.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterSingleByInstance_ImplementationNoDescendantOfServiceType_ThrowsException()
        {
            // Arrange
            var container = new SimpleServiceLocator();

            object impl = new SqlConnection();

            // Act
            container.RegisterSingle(typeof(IService), impl);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterSingleByInstance_NullContainer_ThrowsException()
        {
            // Arrange
            SimpleServiceLocator invalidContainer = null;

            Type validServiceType = typeof(IService);
            object validInstance = new ServiceImpl(null);

            // Act
            invalidContainer.RegisterSingle(validServiceType, validInstance);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterSingleByInstance_NullServiceType_ThrowsException()
        {
            // Arrange
            var container = new SimpleServiceLocator();

            Type invalidServiceType = null;
            object validInstance = new ServiceImpl(null);

            // Act
            container.RegisterSingle(invalidServiceType, validInstance);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterSingleByInstance_NullInstance_ThrowsException()
        {
            // Arrange
            var container = new SimpleServiceLocator();

            Type validServiceType = typeof(IService);
            object invalidInstance = null;

            // Act
            container.RegisterSingle(validServiceType, invalidInstance);
        }

        [TestMethod]
        public void RegisterSingleByType_ValidRegistration_GetInstanceReturnsExpectedType()
        {
            // Arrange
            var container = new SimpleServiceLocator();

            // Act
            container.RegisterSingle(typeof(IService), typeof(ServiceImpl));

            // Assert
            Assert.IsInstanceOfType(container.GetInstance<IService>(), typeof(ServiceImpl));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterSingleByType_NullServiceType_ThrowsException()
        {
            // Arrange
            var container = new SimpleServiceLocator();

            Type invalidServiceType = null;

            // Act
            container.RegisterSingle(invalidServiceType, typeof(ServiceImpl));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterSingleByType_NullImplementationType_ThrowsException()
        {
            // Arrange
            var container = new SimpleServiceLocator();

            Type invalidImplementationType = null;

            // Act
            container.RegisterSingle(typeof(IService), invalidImplementationType);
        }

        [TestMethod]
        public void RegisterSingleByType_ValidRegistration_GetInstanceAlwaysReturnsSameInstance()
        {
            // Arrange
            var container = new SimpleServiceLocator();

            object impl = new ServiceImpl(null);

            // Act
            container.RegisterSingle(typeof(IService), typeof(ServiceImpl));
            
            var instance1 = container.GetInstance<IService>();
            var instance2 = container.GetInstance<IService>();

            // Assert
            Assert.AreEqual(instance1, instance2, "RegisterSingle should register singleton.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterSingleByType_InstanceThatDoesNotImplementServiceType_Fails()
        {
            // Arrange
            var container = new SimpleServiceLocator();

            // Act
            container.RegisterSingle(typeof(IService), typeof(object));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterSingleByType_ImplementationIsServiceType_Fails()
        {
            // Arrange
            var container = new SimpleServiceLocator();

            // Act
            container.RegisterSingle(typeof(ServiceImpl), typeof(ServiceImpl));
        }

        [TestMethod]
        public void RegisterSingleByFunc_ValidArguments_GetInstanceAlwaysReturnsSameInstance()
        {
            // Arrange
            var container = new SimpleServiceLocator();

            Func<object> instanceCreator = () => new ServiceImpl(null);

            // Act
            container.RegisterSingle(typeof(IService), instanceCreator);

            var instance1 = container.GetInstance<IService>();
            var instance2 = container.GetInstance<IService>();

            // Assert
            Assert.AreEqual(instance1, instance2, "RegisterSingle should register singleton.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterSingleByFunc_NullContainer_ThrowsException()
        {
            // Arrange
            SimpleServiceLocator invalidContainer = null;

            Type validServiceType = typeof(IService);
            Func<object> validInstanceCreator = () => new ServiceImpl(null);

            // Act
            invalidContainer.RegisterSingle(validServiceType, validInstanceCreator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterSingleByFunc_NullServiceType_ThrowsException()
        {
            // Arrange
            var container = new SimpleServiceLocator();

            Type invalidServiceType = null;
            Func<object> validInstanceCreator = () => new ServiceImpl(null);

            // Act
            container.RegisterSingle(invalidServiceType, validInstanceCreator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterSingleByFunc_NullInstanceCreator_ThrowsException()
        {
            // Arrange
            var container = new SimpleServiceLocator();

            Type validServiceType = typeof(IService);
            Func<object> invalidInstanceCreator = null;

            // Act
            container.RegisterSingle(validServiceType, invalidInstanceCreator);
        }

        [TestMethod]
        public void RegisterAll_WithValidCollectionOfServices_Succeeds()
        {
            // Arrange
            var instance = new ServiceImpl(null);

            var container = new SimpleServiceLocator();

            // Act
            container.RegisterAll(typeof(IService), new IService[] { instance });

            // Assert
            var instances = container.GetAllInstances<IService>();

            Assert.AreEqual(1, instances.Count());
            Assert.AreEqual(instance, instances.First());
        }

        [TestMethod]
        public void RegisterAll_WithValidObjectCollectionOfServices_Succeeds()
        {
            // Arrange
            var instance = new ServiceImpl(null);

            var container = new SimpleServiceLocator();

            // Act
            container.RegisterAll(typeof(IService), new object[] { instance });

            // Assert
            var instances = container.GetAllInstances<IService>();

            Assert.AreEqual(1, instances.Count());
            Assert.AreEqual(instance, instances.First());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterAll_NullContainer_ThrowsException()
        {
            // Arrange
            var instance = new ServiceImpl(null);

            SimpleServiceLocator invalidContainer = null;

            // Act
            invalidContainer.RegisterAll(typeof(IService), new IService[] { instance });
        }

        [TestMethod]
        public void RegisterAll_WithValidCollectionOfImplementations_Succeeds()
        {
            // Arrange
            var instance = new ServiceImpl(null);

            var container = new SimpleServiceLocator();

            // Act
            container.RegisterAll(typeof(IService), new ServiceImpl[] { instance });

            // Assert
            var instances = container.GetAllInstances<IService>();

            Assert.AreEqual(1, instances.Count());
            Assert.AreEqual(instance, instances.First());
        }

        [TestMethod]
        public void RegisterByType_ValidArguments_Succeeds()
        {
            // Arrange
            var container = new SimpleServiceLocator();

            Type validServiceType = typeof(IService);
            Type validImplementation = typeof(ServiceImpl);

            // Act
            container.Register(validServiceType, validImplementation);

            // Assert
            var instance = container.GetInstance(validServiceType);

            Assert.IsInstanceOfType(instance, validImplementation);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterByType_NullContainer_ThrowsException()
        {
            // Arrange
            SimpleServiceLocator invalidContainer = null;

            Type validServiceType = typeof(IService);
            Type validImplementation = typeof(ServiceImpl);

            // Act
            invalidContainer.Register(validServiceType, validImplementation);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterByType_NullServiceType_ThrowsException()
        {
            // Arrange
            var container = new SimpleServiceLocator();

            Type invalidServiceType = null;
            Type validImplementation = typeof(ServiceImpl);

            // Act
            container.Register(invalidServiceType, validImplementation);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterByType_NullImplementation_ThrowsException()
        {
            // Arrange
            var container = new SimpleServiceLocator();

            Type validServiceType = typeof(IService);
            Type invalidImplementation = null;

            // Act
            container.Register(validServiceType, invalidImplementation);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterByType_ServiceTypeAndImplementationSameType_ThrowsException()
        {
            // Arrange
            var container = new SimpleServiceLocator();

            Type implementation = typeof(ServiceImpl);

            // Act
            container.Register(implementation, implementation);
        }

        [TestMethod]
        public void RegisterByFunc_ValidArguments_Succeeds()
        {
            // Arrange
            var container = new SimpleServiceLocator();

            Type validServiceType = typeof(IService);
            Func<object> instanceCreator = () => new ServiceImpl(null);

            // Act
            container.Register(validServiceType, instanceCreator);

            // Assert
            var instance = container.GetInstance(validServiceType);

            Assert.IsInstanceOfType(instance, typeof(ServiceImpl));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterByFunc_NullInstanceCreator_ThrowsException()
        {
            // Arrange
            var container = new SimpleServiceLocator();

            Type validServiceType = typeof(IService);
            Func<object> invalidInstanceCreator = null;

            // Act
            container.Register(validServiceType, invalidInstanceCreator);
        }

        private sealed class ServiceImpl : IService
        {
            public ServiceImpl(Dependency dependency)
            {
            }
        }

        private sealed class Dependency
        {
        }
    }
}