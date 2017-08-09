using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Tests.Helpers;
using Xunit;

namespace Liquid.NET.Tests
{
    
    public class CapacityTests
    {
        [Fact]
        public void It_Should_Parse_Includes()
        {
            DateTime start = DateTime.Now;
            // Arrange
            int includes = 100;
            //String templateFragment = "{% for i in (1..10) %}{{ array[i] }}";
            String template = "<html><body>\r\n";
            for (int i = 0; i < includes; i++)
            {
                template += "{% include \"test" + i + "\" %}";
            }

            var virtualFs = Enumerable.Range(1, includes).ToDictionary(k => "test" + k.ToString(), CreateInclude);
            template += "</body>\r\n</html>";
            ITemplateContext ctx = new TemplateContext()
                .WithAllFilters()
                .WithFileSystem(new TestFileSystem(virtualFs))
                .DefineLocalVariable("array", CreateArrayValues());


            // Act

            // ReSharper disable once UnusedVariable
            var result = RenderingHelper.RenderTemplate(template, ctx);

            //Logger.Log(result);

            // Assert
            var end = DateTime.Now;
            TimeSpan timeDiff = end - start;
            Logger.Log("ELAPSED: " + timeDiff.TotalMilliseconds);
            Assert.True(timeDiff.Milliseconds < 500);
            //Assert.Fail("Not Implemented Yet");

        }

        [Fact]
        // https://github.com/antlr/antlr4/issues/192#issuecomment-15238595
        public void It_Should_Not_Bog_On_Raw_Text_With_Adaptive_Prediction()
        {
            // Arrange

            DateTime start = DateTime.Now;
            Logger.Log("STARTING...");
            int iterations = 100;

            String template = "<html><body>\r\n";
            for (int i = 0; i < iterations; i++)
            {
                template += "{% for x in (1..10) %} Test {{ x }}{% endfor %}";
                for (int j = 0; j < 100; j++)
                {
                    template += " Raw text "+j;
                }
            }
            template += "</body>\r\n</html>";
            ITemplateContext ctx = new TemplateContext()
                .WithAllFilters();

            // Act

            var result = RenderingHelper.RenderTemplate(template, ctx);

            Logger.Log(result);

            // Assert
            var end = DateTime.Now;
            TimeSpan timeDiff = end - start;
            Logger.Log("ELAPSED: " + timeDiff.TotalMilliseconds);
            Assert.True(timeDiff.Milliseconds < 1000);
            //Assert.Fail("Not Implemented Yet");

        }


        private string CreateInclude(int i)
        {
            return "{% for i in (1..10) %}{{ array[i] }}{% endfor %}";
        }

        private LiquidCollection CreateArrayValues()
        {
            return new LiquidCollection
            {
                LiquidString.Create("a string"),
                LiquidNumeric.Create(123),
                LiquidNumeric.Create(456m),
                new LiquidBoolean(false),
                LiquidString.Create("a string 2"),
                LiquidNumeric.Create(999999.0m ),
                LiquidNumeric.Create(456m),
                new LiquidBoolean(true),
                new LiquidCollection{LiquidNumeric.Create(1), LiquidNumeric.Create(2), LiquidNumeric.Create(3)},
                new LiquidHash{{"1", LiquidNumeric.Create(1)}, {"2", LiquidNumeric.Create(2)}}
            };
        }

    }
}