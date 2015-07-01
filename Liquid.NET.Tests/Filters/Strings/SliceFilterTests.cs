using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Filters.Array;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class SliceFilterTests
    {
        [Test]
        [TestCase("Hello", "0", "H")]
        [TestCase("Hello", "1,3", "ell")]
        [TestCase("Hello", "1", "e")]
        [TestCase("Hello", "-3,2", "ll")] // Shopify example is incorrect
        [TestCase("Hello", "2,-3", "")]
        [TestCase("Hello", "3,1", "l")]
        [TestCase("Hello", "10,1", "")]
        public void It_Should_Slice_A_String(String orig, String slice, String expected)
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"" + orig + "\" | slice : "+slice+" }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : " + expected));
        }

        [Test]
        //[TestCase("0", "[ \"a string\" ]")]
        //[TestCase("1,3", "[ 123, 456, false ]")]
        //[TestCase("-3,2", "[ 123, 456 ]")]
        [TestCase("0", "a string")]
        [TestCase("1,3", "123456false")]
        [TestCase("-3,2", "123456")]
        
        public void It_Should_Slice_An_Array(String slice, string expected)
        {
            // Arrange
            var ctx = new TemplateContext();
            ctx.Define("array", CreateArray());
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ array | slice : " + slice + " }}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : "+expected));


        }


        public ArrayValue CreateArray()
        {
            // Arrange
            IList<IExpressionConstant> objlist = new List<IExpressionConstant>
            {
                new StringValue("a string"), 
                new NumericValue(123), 
                new NumericValue(456m),
                new BooleanValue(false)
            };
            return new ArrayValue(objlist);

        }


        
    }
}
