using System;
using Xunit;

namespace Liquid.NET.Tests.Filters.Strings
{
    
    public class SplitFilterTests
    {

        [Fact]
        public void It_Should_Split_A_String()
        {
            // Arrange
            const String tmpl = @"{{ ""Uses cheat codes, calls the game boring."" | split: ' ' }}";

            // Act
            var result = RenderingHelper.RenderTemplate(tmpl);
            Logger.Log(result);

            // Assert            
            //Assert.Equal(@"[ ""Uses"", ""cheat"", ""codes,"", ""calls"", ""the"", ""game"", ""boring."" ]", result);
            Assert.Equal(@"Usescheatcodes,callsthegameboring.", result);
        }

        [Fact]
        public void It_Should_Split_A_String_Inside_A_Tag() 
        {        
            // Arrange
            const String tmpl = @"{% assign words = ""Uses cheat codes, calls the game boring."" | split: ' ' %}{{words}}";

            // Act
            var result = RenderingHelper.RenderTemplate(tmpl);
            Logger.Log(result);

            // Assert            
            //Assert.Equal(@"[ ""Uses"", ""cheat"", ""codes,"", ""calls"", ""the"", ""game"", ""boring."" ]", result);
            Assert.Equal(@"Usescheatcodes,callsthegameboring.", result);
        
        }

        [Fact]
        // SEE: https://docs.shopify.com/themes/liquid-documentation/filters/string-filters#split
        public void It_Should_Replicate_The_Shopify_Documentation()
        {
            // Arrange
            const String tmpl = @"{% assign words = ""Uses cheat codes, calls the game boring."" | split: ' ' %}First word: {{ words.first }}
First word: {{ words[0] }}
Second word: {{ words[1] }}
Last word: {{ words.last }}
All words: {{ words | join: ', ' }}

{% for word in words %}{{ word }} {% endfor %}";

            const string expected = @"First word: Uses
First word: Uses
Second word: cheat
Last word: boring.
All words: Uses, cheat, codes,, calls, the, game, boring.

Uses cheat codes, calls the game boring. ";

            // Act
            var result = RenderingHelper.RenderTemplate(tmpl);

            // Assert            
            Assert.Equal(expected, result);

        }
    }
}
