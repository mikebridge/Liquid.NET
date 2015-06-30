using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using NUnit.Framework;

namespace Liquid.NET.Tests.Expressions
{
    [TestFixture]
    public class ContainsExpressionTests
    {
        [Test]
        [TestCase("\"hello\"", "'el'", "TRUE")]
        [TestCase("\"hello\"", "'e'", "TRUE")]
        [TestCase("\"hello\"", "'X'", "FALSE")]
        public void It_Should_Determine_If_A_String_Contains_A_Substring(String val, String contains, String expected)
        {
            // Act
            var result = RenderingHelper.RenderTemplate("{%if "+val+" contains "+contains+" %}TRUE{% else %}FALSE{% endif %}");

            // Assert
            Assert.That(result, Is.EqualTo(expected));

        }

        [Test]
        [TestCase("1", "FALSE")]
        [TestCase("\'1\'", "TRUE")]
        [TestCase("\'X\'", "FALSE")]
        [TestCase("2", "TRUE")]
        [TestCase("\'2\'", "FALSE")]
        [TestCase("true", "TRUE")]
        [TestCase("ZZZ", "FALSE")]
        public void It_Should_Determine_If_An_Array_Contains_An_Element(String contains, String expected)
        {
            // Arrange
            var ctx = new TemplateContext();
            ctx.Define("array", CreateArray());

            // Act
            var result = RenderingHelper.RenderTemplate("{%if array contains " + contains + " %}TRUE{% else %}FALSE{% endif %}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo(expected));

        }

        [Test]
        [TestCase("1", "FALSE")]
        [TestCase("\"one\"", "TRUE")]
        [TestCase("2", "FALSE")]
        [TestCase("\'two\'", "TRUE")]
        public void It_Should_Determine_If_An_Dictionary_Contains_A_KEy(String contains, String expected)
        {
            // Arrange
            var ctx = new TemplateContext();
            ctx.Define("dict", CreateDictionary());

            // Act
            var result = RenderingHelper.RenderTemplate("{%if dict contains " + contains + " %}TRUE{% else %}FALSE{% endif %}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo(expected));

        }


        [Test]
        public void It_Should_Not_Work_For_A_Numeric_Value()
        {
            // Arrange
            var ctx = new TemplateContext();
            ctx.Define("array", CreateArray());

            // Act
            var result = RenderingHelper.RenderTemplate("{%if 3 contains 3 %}TRUE{% else %}FALSE{% endif %}");

            // Assert
            Assert.That(result, Is.StringContaining("FALSE")); // TODO: SHould this be an error?
        }

        public ArrayValue CreateArray()
        {
            // Arrange
            IList<IExpressionConstant> objlist = new List<IExpressionConstant>
            {
                new StringValue("1"), 
                new NumericValue(2), 
                new StringValue("Three"),
                new BooleanValue(true)
            };
            return new ArrayValue(objlist);

        }

        public DictionaryValue CreateDictionary()
        {
            return new DictionaryValue(
                new Dictionary<string, IExpressionConstant>
                {
                    {"one",  new StringValue("1")},
                    {"two", new NumericValue(2)},
                    {"three", new StringValue("Three")},
                    {"four", new BooleanValue(true)}

                });

        }


    }
}
