using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using NUnit.Framework;

namespace Liquid.NET.Tests.Constants
{
    [TestFixture]
    public class ConstantFactoryTests
    {
//        [Test]
//        public void It_Should_Determine_The_Return_Type_Of_A_Function()
//        {
//            // Arrange
//            var type = ConstantFactory.GetReturnType(_testToString);
//
//            // Act
//            Console.WriteLine("Type is "+type);
//
//            // Assert
//            Assert.That(type, Is.EqualTo(typeof(StringValue)));
//
//        }

//        [Test]
//        public void It_Should_Create_An_Undefined_Variable_Corresponding_To_A_Return_Type()
//        {
//            // Arrange
//            
//            //ConstantFactory.CreateUndefinedX(_testToString);
//            // Act
//            var result = ConstantFactory.CreateNilValueOfType(_testToString, "testing...");
//
//            // Assert
//            Assert.That(result, Is.TypeOf<StringValue>());
//            Assert.That(result.IsUndefined);
//        }

//        [Test]
//        public void It_Should_Create_A_String_If_Type_Return_Type_Is_Not_Instantiable()
//        {
//            // Arrange
//
//            //ConstantFactory.CreateUndefinedX(_testToString);
//            // Act
//            var result = ConstantFactory.CreateNilValueOfType(_testToInterface, "testing...");
//
//            // Assert
//            Assert.That(result, Is.TypeOf<StringValue>());
//            Assert.That(result.IsUndefined);
//        }

        private readonly Func<NumericValue, StringValue> _testToString = num => new StringValue(num.Value.ToString());

        private readonly Func<NumericValue, IExpressionConstant> _testToInterface = num => new StringValue(num.Value.ToString());

    }
}
