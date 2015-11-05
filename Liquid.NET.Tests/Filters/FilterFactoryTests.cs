using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Filters;
using Liquid.NET.Filters.Strings;
using Liquid.NET.Utils;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters
{
    [TestFixture]
    public class FilterFactoryTests
    {


        [Test]
        public void It_Should_Instantiate_A_Filter()
        {
            // Act
            var filterResult = FilterFactory.InstantiateFilter("upcase", typeof(UpCaseFilter), new List<Option<IExpressionConstant>>());
        
            // Assert
            Assert.That(filterResult.Value, Is.TypeOf(typeof (UpCaseFilter)));
       
        }

        [Test]
        public void It_Should_Return_An_Error_If_Type_Isnt_A_Filter()
        {
            // Act
            var result = FilterFactory.InstantiateFilter("string", typeof(String), new List<Option<IExpressionConstant>>());
            Assert.That(result.IsFailure, Is.True);
        }

        [Test]
        public void It_Should_Convert_A_Symbol_With_Args_To_A_Filter()
        {
            // Arrange
            var filter = FilterFactory.InstantiateFilter("remove", typeof(RemoveFilter), new List<Option<IExpressionConstant>>() { new StringValue("test") });

            // Act

            // Assert
            Assert.That(filter.Value, Is.TypeOf(typeof(RemoveFilter)));
        }      

        [Test]
        public void It_Should_Cast_Numeric_Args_To_A_String()
        {
            // Arrange
            var filter = FilterFactory.InstantiateFilter("mockfilter", typeof(MockStringToStringFilter), new List<Option<IExpressionConstant>>() { NumericValue.Create(123) });

            // Act

            Assert.That((((MockStringToStringFilter) filter.Value).StringArg1.Value), Is.EqualTo("123"));
        }

        [Test]
        public void It_Should_Return_Error_When_Not_Filter_Type()
        {
            // Arrange
            var filter = FilterFactory.InstantiateFilter("mockfilter", typeof(String), new List<Option<IExpressionConstant>> { NumericValue.Create(123) });

            // Assert
            Assert.That(filter.IsFailure, Is.True);
        }


        [Test]
        public void It_Should_Return_Error_When_Null_Type()
        {
            // Arrange
            var filter = FilterFactory.InstantiateFilter("mockfilter", null, new List<Option<IExpressionConstant>> { NumericValue.Create(123) });

            // Assert
            Assert.That(filter.IsFailure, Is.True);
        }

        [Test]
        public void It_Should_Return_Error_When_Multiple_Constructors()
        {
            // Arrange
            var filter = FilterFactory.InstantiateFilter("mockfilter", typeof(TwoConstructorFilter), new List<Option<IExpressionConstant>> { NumericValue.Create(123) });

            // Assert
            Assert.That(filter.IsFailure, Is.True);
        }


        [Test]
        public void It_Should_Pass_A_Wrapped_Null_When_Arg_Missing()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 'x123y' | replace_first : '123'}}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : xy"));
        }

        public class TwoConstructorFilter : FilterExpression<StringValue, StringValue>
        {
            public StringValue StringArg1 { get; set; }
            public StringValue StringArg2 { get; set; }

            public String MESSAGE = "The object was {0}, parm1 was {1} and parm2 was {2}.";

            public TwoConstructorFilter(StringValue stringLiteral1, StringValue stringLiteral2)
            {
                StringArg1 = stringLiteral1;
                StringArg2 = stringLiteral2;
            }

            public TwoConstructorFilter(StringValue stringLiteral1)
            {
                StringArg1 = stringLiteral1;
            }

        }


        // ReSharper disable once ClassNeverInstantiated.Global
        public class MockStringToStringFilter : FilterExpression<StringValue, StringValue>
        {
            public StringValue StringArg1 { get; set; }
            public StringValue StringArg2 { get; set; }

            public String MESSAGE = "The object was {0}, parm1 was {1} and parm2 was {2}.";

            public MockStringToStringFilter(StringValue stringLiteral1, StringValue stringLiteral2)
            {
                StringArg1 = stringLiteral1;
                StringArg2 = stringLiteral2;
            }

            public override LiquidExpressionResult Apply(ITemplateContext ctx, StringValue liquidStringExpression)
            {
                return LiquidExpressionResult.Success(new StringValue(
                    (liquidStringExpression == null ? "NULL" : liquidStringExpression.StringVal) +" " + 
                    (StringArg1 == null ? "NULL" : StringArg1.StringVal) + " " +
                    (StringArg2 == null ? "NULL" : StringArg2.StringVal)));
            }
        }


    }
}
