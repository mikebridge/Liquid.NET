using System;
using Xunit;

namespace Liquid.NET.Tests.Filters.Strings
{
    
    public class TruncateWordsFilterTests
    {
        [Fact]
        public void It_Should_Truncate_By_Word()
        {
            // Arrange
            const String tmpl = @"{{ ""The cat came back the very next day"" | truncatewords: 4 }}";

            // Act
            var result = RenderingHelper.RenderTemplate(tmpl);

            // Assert
            Assert.Equal("The cat came back...", result);
        }

        [Fact]
        public void It_Should_Add_Something_Other_Than_Ellipses()
        {
            // Arrange
            const String tmpl = @"{{ ""The cat came back the very next day"" | truncatewords: 4, ""!!!"" }}";

            // Act
            var result = RenderingHelper.RenderTemplate(tmpl);

            // Assert
            Assert.Equal("The cat came back!!!", result);
        }

        [Fact]
        public void It_Should_Not_Add_Ellipses_With_Fewer_Words()
        {
            // Arrange
            const String tmpl = @"{{ ""The"" | truncatewords: 4 }}";

            // Act
            var result = RenderingHelper.RenderTemplate(tmpl);

            // Assert
            Assert.Equal("The", result);
        }

        [Fact]
        public void It_Should_Have_A_Default()
        {
            // Arrange
            const String tmpl = @"{{ ""The"" | truncatewords }}";

            // Act
            var result = RenderingHelper.RenderTemplate(tmpl);

            // Assert
            Assert.Equal("The", result);
        }

        [Fact]
        public void It_Should_Not_Count_Blank_Words()
        {
            // Arrange
            const String tmpl = @"{{ ""The cat     came    back the very next day"" | truncatewords: 4 }}";

            // Act
            var result = RenderingHelper.RenderTemplate(tmpl);

            // Assert
            Assert.Equal("The cat came back...", result);
        }

    }
}
