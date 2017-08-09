using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Utils;
using Xunit;

namespace Liquid.NET.Tests.Ruby
{
    
    public class DictionaryFactoryTests
    {
        [Fact]
        public void It_Should_Convert_An_Array()
        {
            // Arrange
            String json = "[1,2,3]";
 
            // Act
            //var result = DictionaryFactory.Transform(json);
            var result = DictionaryFactory.CreateArrayFromJson(json);

            // Assert
            //Logger.Log(result.ToString());
            Assert.True(result.HasValue);
            Assert.IsType<LiquidCollection>(result.Value);

            //Assert.That(((LiquidCollection) result.Value).Select(x => x.Value.Value), Is.EquivalentTo(new List<int> {1,2,3}));            
            Assert.Equal(((LiquidCollection)result.Value).Select(x => Convert.ToInt32(x.Value.Value)), 
                                                                 new List<int> { 1, 2, 3 });


        }

        [Fact]
        public void It_Should_Convert_A_Dictionary_Containing_An_Array()
        {
            // Arrange
            String json = "{\"array\": [1,2,3]}";

            // Act
            IList<Tuple<String, Option<ILiquidValue>>> result = DictionaryFactory.CreateStringMapFromJson(json);

            // Assert
            //Logger.Log(result);
            

            Assert.Equal("array", result[0].Item1);
            Assert.IsType<LiquidCollection>(result[0].Item2.Value);
            var array = (LiquidCollection)result[0].Item2.Value;
            //Assert.That(array.Select(x => x.Value.Value), Is.EquivalentTo(new List<int> {1,2,3}));
            Assert.Equal(array.Select(x => Convert.ToInt32(x.Value.Value)), new List<int> { 1, 2, 3 });


        }

    }
}

