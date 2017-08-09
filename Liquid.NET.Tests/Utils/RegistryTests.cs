using System;
using Liquid.NET.Utils;
using Xunit;

namespace Liquid.NET.Tests.Utils
{
    
    public class RegistryTests
    {
        private readonly Registry<String> _registry;
            
        public RegistryTests()
        {
            _registry = new Registry<String>();
        }

        [Fact]
        public void It_Should_Save_A_Type()
        {
            // Arrange
            var key = "mykey";
            _registry.Register<String>(key);

            // Act
            Type type = _registry.Find(key);

            // Assert
            Assert.Equal("String", type.Name);

        }

        [Fact]
        public void It_Should_Deregister_A_Type()
        {
            // Arrange
            var key = "mykey";
            _registry.Register<String>(key);            

            // Act
            _registry.Deregister(key);
            Type type = _registry.Find(key);

            // Assert
            Assert.Null(type);

        }


        [Fact]
        public void It_Should_Override_A_Type()
        {
            // Arrange
            var key = "mykey";
            var registry = new Registry<Object>();
            registry.Register<String>(key);

            // Act
            registry.Register<int>(key);

            // Assert
            Assert.Equal("Int32", registry.Find(key).Name);

        }


    }
}
