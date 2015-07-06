using System;
using Liquid.NET.Constants;
using Liquid.NET.Filters;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters
{
    [TestFixture]
    public class CastFilterTests
    {
        [Test]
        public void It_Should_Cast_The_Value_From_One_Type_To_Another()
        {
            // Arrange
            var castFilter = new CastFilter<StringValue, NumericValue>();
            var inputObj = new StringValue("123");

            // Act
            var result = castFilter.Apply(new TemplateContext(),inputObj).SuccessValue<NumericValue>();
            //result.Eval(new SymbolTableStack(new TemplateContext()), new List<IExpressionConstant>());

            // Assert
            Assert.That(result, Is.TypeOf<NumericValue>());
            Assert.That((decimal) result.Value, Is.EqualTo(123m));

        }

        [Test]
        [TestCase(123.0, "123.0")]
        [TestCase(123, "123.0")]
        [TestCase(123.1, "123.1")]
        public void It_Should_Cast_A_Decimal_To_A_String_Like_Ruby_Liquid(decimal input, String expected)
        {
            // Arrange
            var castFilter = new CastFilter<NumericValue, StringValue>();

            // Act
            var result = castFilter.Apply(new TemplateContext(), new NumericValue(input)).SuccessValue<StringValue>();

            // Assert
            Assert.That(result.StringVal, Is.EqualTo(expected));


        }

        [Test]
        [TestCase(123, "123")]
        public void It_Should_Cast_An_Int_To_A_String_Like_Ruby_Liquid(int input, String expected)
        {
            // Arrange
            var castFilter = new CastFilter<NumericValue, StringValue>();

            // Act
            var result = castFilter.Apply(new TemplateContext(), new NumericValue(input)).SuccessValue<StringValue>();

            // Assert
            Assert.That(result.StringVal, Is.EqualTo(expected));


        }

    }
}
