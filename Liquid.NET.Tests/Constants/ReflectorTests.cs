using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using NUnit.Framework;

namespace Liquid.NET.Tests.Constants
{
    [TestFixture]
    public class ReflectorTests
    {
        [Test]
        public void It_Should_Create_A_Dictionary_From_An_Object()
        {
            // Arrange
            var testObj = new TestObj();
            var reflector = new Reflector();

            // Act

            var exprObject = (DictionaryValue) reflector.GenerateExpressionConstant(testObj);

            // Assert
            Assert.That(exprObject, Is.Not.Null);
            Assert.That(exprObject.ValueAt("MyProperty").Value, Is.EqualTo(testObj.MyProperty));
            Assert.That(exprObject.ValueAt("MyPropertyWithPrivateSetter").Value, Is.EqualTo(testObj.MyPropertyWithPrivateSetter));
            Assert.That(exprObject.ValueAt("MyPublicField").Value, Is.EqualTo(testObj.MyPublicField));
            Assert.That(exprObject.ValueAt("MyPrivateProperty").HasValue, Is.False);
            Assert.That(exprObject.ValueAt("MyPrivateField").HasValue, Is.False);
        }

        public class TestObj
        {
            public TestObj()
            {
                MyProperty = "A Property";
                MyPropertyWithPrivateSetter = "Property With Private Setter";
                MyPrivateProperty = "A Private Property";
            }

            public String MyPublicField = "a public field";

#pragma warning disable 169
#pragma warning disable 414
            // ReSharper disable once InconsistentNaming
            private String MyPrivateField = "a private field";
#pragma warning restore 414
#pragma warning restore 169

            public String MyProperty { get; set; }

            public String MyPropertyWithPrivateSetter { get; private set; }

            private String MyPrivateProperty { get; set; }

        }


    }
}
