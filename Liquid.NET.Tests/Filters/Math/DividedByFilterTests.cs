using System.Globalization;
using System.Threading;
using Xunit;

namespace Liquid.NET.Tests.Filters.Math
{
    
    public class DividedByFilterTests
    {
        [Fact]
        public void It_Should_Divide_A_Number_By_Another_Number()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 9 | divided_by: 3 }}");

            // Assert
            Assert.Equal("Result : 3", result);
        }

        [Fact]
        public void It_Should_Return_An_Int_When_Dividing_IntLike_Strings()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 10 | divided_by: 4 }}");

            // Assert
            //Assert.Equal("Result : 2.5", result);
            Assert.Equal("Result : 2", result);
        }

        [Fact]
        public void It_Should_Return_A_Decimal_When_Dividing_IntLike_Strings()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 10.0 | divided_by: 4 }}");

            // Assert
            Assert.Equal("Result : 2.5", result);
        }

        [Fact]
        public void It_Should_Cast_Strings()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"10\" | divided_by: \"4\" }}");

            // Assert
            //Assert.Equal("Result : 2.5", result);
            Assert.Equal("Result : 2", result);
        }

        [Fact]
        public void It_Should_Not_Divide_By_Zero_In_French()
        {
            // Arrange
            //var origCulture = Thread.CurrentThread.CurrentCulture;
            var origCulture = CultureInfo.DefaultThreadCurrentCulture;
            try
            {
                //Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("fr-CA");
                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("fr-CA");
                var result = RenderingHelper.RenderTemplate("Result : {{ 10.0 | divided_by: 4 }}");

                // Assert
                Assert.Equal("Result : 2,5", result);
            }
            finally
            {
                CultureInfo.DefaultThreadCurrentCulture = origCulture;
            }
        }

    }
}
