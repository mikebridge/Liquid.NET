using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Filters;
using Liquid.NET.Filters.Math;
using Liquid.NET.Filters.Strings;
using Liquid.NET.Symbols;
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
            var filter = FilterFactory.InstantiateFilter<UpCaseFilter>("upcase", new List<Option<IExpressionConstant>>());

            // Assert
            Assert.That(filter, Is.TypeOf(typeof (UpCaseFilter)));

        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void It_Should_Return_An_Error_If_Type_Isnt_A_Filter()
        {
            // Act
            var filter = FilterFactory.InstantiateFilter("string", typeof(String), new List<Option<IExpressionConstant>>());

        }

        [Test]
        public void It_Should_Convert_A_Symbol_With_Args_To_A_Filter()
        {
            // Arrange
            var filter = FilterFactory.InstantiateFilter<RemoveFilter>("remove", new List<Option<IExpressionConstant>>() { new StringValue("test") });

            // Act

            // Assert
            Assert.That(filter, Is.TypeOf(typeof(RemoveFilter)));
        }

        [Test]
        [Ignore("this is no longer the case.  TODO: make the args into option types")]
        public void It_Should_Pass_ArgDefault_If_Missing_Args()
        {
            // Arrange

            var filter = FilterFactory.InstantiateFilter<MockStringToStringFilter>("mockfilter", new List<Option<IExpressionConstant>>() { new StringValue("test") });

            // Assert
            Assert.That(filter.StringArg1, Is.Not.Null);
            Assert.That(filter.StringArg2, Is.Not.Null);
            Assert.Fail("Fix former undefined behaviour");
            //Assert.That(filter.StringArg2.IsUndefined, Is.True);
        }

        [Test]
        public void It_Should_Cast_Numeric_Args_To_A_String()
        {
            // Arrange
            var filter = FilterFactory.InstantiateFilter<MockStringToStringFilter>("mockfilter", new List<Option<IExpressionConstant>>() { new NumericValue(123) });

            // Act

            Assert.That(filter.StringArg1.Value, Is.EqualTo("123"));
        }


        [Test]
        [Ignore("Not Implemented yet")]
        public void It_Should_Fail_If_Missing_Arg_Is_Not_Null()
        {
            // Arrange
            var filterRegistry = new FilterRegistry();
            filterRegistry.Register<PlusFilter>("add");

            // Act
            var symbol = new FilterSymbol("add");

            //ar filter = (PlusFilter)CreateFilterFactory(filterRegistry).CreateFilter(symbol);

            // Assert
            Assert.Fail("Not sure");

        }

        [Test]
        public void It_Should_Pass_A_Wrapped_Null_When_Arg_Missing()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 'x123y' | replace_first : '123'}}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : xy"));
        }


        [Test]
        public void It_Should_Instantiate_The_Right_Generic_Class()
        {
            // Arrange

            // Act
            var result = FilterFactory.CreateCastExpression(typeof(NumericValue), typeof(StringValue));


            // Assert
            Assert.That(result, Is.TypeOf(typeof(CastFilter<NumericValue, StringValue>)));

        }

        public class MockStringToStringFilter : FilterExpression<StringValue, StringValue>
        {
            public StringValue StringArg1 { get; set; }
            public StringValue StringArg2 { get; set; }

            public String MESSAGE = "The object was {0}, parm1 was {1} and parm2 was {2}.";

            public MockStringToStringFilter(StringValue stringLiteral, StringValue stringLiteral2)
            {
                StringArg1 = stringLiteral;
                StringArg2 = stringLiteral2;
            }

            public override LiquidExpressionResult Apply(ITemplateContext ctx, StringValue liquidStringExpression)
            {
                throw new NotImplementedException();
            }
        }

        private static FilterFactory CreateFilterFactory(FilterRegistry registry =null)
        {
            if (registry == null)
            {
                registry = new FilterRegistry();
                registry.Register<UpCaseFilter>("upcase");
                registry.Register<RemoveFilter>("remove");
            }
            return new FilterFactory(registry);
        }


    }
}
