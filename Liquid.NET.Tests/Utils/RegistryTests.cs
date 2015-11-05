using System;
using Liquid.NET.Utils;
using NUnit.Framework;

namespace Liquid.NET.Tests.Utils
{
    [TestFixture]
    public class RegistryTests
    {
        private Registry<String> _registry;
            
        [SetUp]
        public void Setup()
        {
            _registry = new Registry<String>();
        }

        [Test]
        public void It_Should_Save_A_Type()
        {
            // Arrange
            var key = "mykey";
            _registry.Register<String>(key);

            // Act
            Type type = _registry.Find(key);

            // Assert
            Assert.That(type.Name, Is.EqualTo("String"));

        }

        [Test]
        public void It_Should_Deregister_A_Type()
        {
            // Arrange
            var key = "mykey";
            _registry.Register<String>(key);            

            // Act
            _registry.Deregister(key);
            Type type = _registry.Find(key);

            // Assert
            Assert.That(type, Is.Null);

        }


        [Test]
        public void It_Should_Override_A_Type()
        {
            // Arrange
            var key = "mykey";
            var registry = new Registry<Object>();
            registry.Register<String>(key);

            // Act
            registry.Register<int>(key);

            // Assert
            Assert.That(registry.Find(key).Name, Is.EqualTo("Int32"));

        }


    }
}
