using Liquid.NET.Filters.Array;
using Xunit;

namespace Liquid.NET.Tests.Symbols
{
    
    public class SymbolTableStackFactoryTests
    {
        [Fact]
        public void It_Should_Initialize_The_Filter_Registry()
        {
            // Arrange
            var ctx = new TemplateContext().WithFilter<SortFilter>("abcde");
            
            // Act
            var result = ctx.SymbolTableStack;

            // Assert
            Assert.True(result.HasFilter("abcde"));

        }

    }
}
