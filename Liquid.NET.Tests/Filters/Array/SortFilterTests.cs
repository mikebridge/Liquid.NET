using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Filters.Array;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Array
{
    [TestFixture]
    public class SortFilterTests
    {
        [Test]
        public void It_Should_Sort_An_Array_By_StringValues()
        {
            // Arrange
            IList<IExpressionConstant> objlist = new List<IExpressionConstant>
            {
                new StringValue("a string"), 
                new NumericValue(123), 
                new NumericValue(456m),
                new BooleanValue(false)
            };
            ArrayValue arrayValue = new ArrayValue(objlist);
            var filter = new SortFilter(new StringValue(""));

            // Act            
            var result = filter.Apply(arrayValue);
            var resultStrings = result.Select(x => x.Value.ToString());
            
            // Assert
            Assert.That(resultStrings, Is.EqualTo(new List<String>{"123", "456", "a string", "false"}));

        }


        [Test]
        public void It_Should_Srt_Keys_The_Size_Of_A_Dictionary()
        {
            // Arrange
            var dict = new Dictionary<String, IExpressionConstant>
            {
                {"string1", new StringValue("a string")},
                {"string2", new NumericValue(123)},
                {"string3", new NumericValue(456m)}
            };
            DictionaryValue dictValue = new DictionaryValue(dict);
            SizeFilter sizeFilter = new SizeFilter();

            // Act
            var result = sizeFilter.Apply(dictValue);

            // Assert
            Assert.That(result.Value, Is.EqualTo(dict.Keys.Count()));

        }
    }
}
