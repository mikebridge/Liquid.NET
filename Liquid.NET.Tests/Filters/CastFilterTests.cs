using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Filters;
using Liquid.NET.Symbols;
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
            var result = castFilter.Apply(inputObj);
            //result.Eval(new SymbolTableStack(new TemplateContext()), new List<IExpressionConstant>());

            // Assert
            Assert.That(result, Is.TypeOf<NumericValue>());
            Assert.That((decimal) result.Value, Is.EqualTo(123m));

        }
    }
}
