//namespace Liquid.NET.Tests.Expressions
//{
//    
//    [Ignore("Removed simplified math to allow minus in variables")]
//    class MathExpressionTests
//    {
//        [Theory]
//        [InlineData("{% assign a = 2 + 1 %}{{a}}", "3")]
//        [InlineData("{% assign a = 2 - 1 %}{{a}}", "1")]
//        [InlineData("{% assign a = 2 * 2 %}{{a}}", "4")]
//        [InlineData("{% assign a = 2 / 2 %}{{a}}", "1")]
//        [InlineData("{% assign a = 4 % 3 %}{{a}}", "1")]
//        public void It_Should_Evaluate_A_Math_Expression(String expr, String expected)
//        {
//            // Act
//            var result = RenderingHelper.RenderTemplate(expr);
//
//            // Assert
//            Assert.Equal(expected, result);
//        }
//
//    }
//}
