using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Filters;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class RemoveFilterTests
    {
        [Test]
        public void It_Should_Remove_an_Integer()
        {
            // Arrange
            var removeFilter = new RemoveFilter(new StringValue("123"));

            // Act
            var result = removeFilter.Apply(new StringValue("Remove the 123 in this string."));

            // Assert
            Assert.That(result.Value, Is.EqualTo("Remove the  in this string."));

        }

       

    }
}
