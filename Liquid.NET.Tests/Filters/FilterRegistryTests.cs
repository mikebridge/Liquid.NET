using Liquid.NET.Filters;
using Liquid.NET.Filters.Strings;
using Xunit;

namespace Liquid.NET.Tests.Filters
{
    
    public class FilterRegistryTests
    {
        [Fact]
        public void It_Should_Register_A_Filter()
        {
            // Arrange
            const string key = "upcase";
            FilterRegistry filterRegistry = new FilterRegistry();

            // Act
            filterRegistry.Register<UpCaseFilter>(key);
            var filterType = filterRegistry.Find(key);

            // Assert
            Assert.Equal(typeof(UpCaseFilter), filterType);

        }
    }
}
