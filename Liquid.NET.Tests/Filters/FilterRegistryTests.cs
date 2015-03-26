using Liquid.NET.Filters;
using Liquid.NET.Filters.Strings;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters
{
    [TestFixture]
    //[Ignore("Not Implemented yet")]
    public class FilterRegistryTests
    {
        [Test]
        public void It_Should_Register_A_Filter()
        {
            // Arrange
            const string key = "upcase";
            FilterRegistry filterRegistry = new FilterRegistry();

            // Act
            filterRegistry.Register<UpCaseFilter>(key);
            var filterType = filterRegistry.Find(key);

            // Assert
            Assert.That(filterType, Is.EqualTo(typeof(UpCaseFilter)));

        }
    }
}
