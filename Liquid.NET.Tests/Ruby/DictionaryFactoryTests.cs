using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using NUnit.Framework;

namespace Liquid.NET.Tests.Ruby
{
    [TestFixture]
    public class DictionaryFactoryTests
    {
        [Test]
        public void It_Should_Convert_An_Array()
        {
            // Arrange
            String json = "[1,2,3]";
 
            // Act
            //var result = DictionaryFactory.Transform(json);
            var result = DictionaryFactory.CreateArrayFromJson(json);

            // Assert
            //Logger.Log(result.ToString());
            Assert.That(result, Is.TypeOf<LiquidCollection>());
            
            Assert.That(((LiquidCollection) result).Select(x => x.Value.Value), Is.EquivalentTo(new List<int> {1,2,3}));            


        }

        [Test]
        public void It_Should_Convert_A_Dictionary_Containing_An_Array()
        {
            // Arrange
            String json = "{\"array\": [1,2,3]}";

            // Act
            IList<Tuple<String, IExpressionConstant>> result = DictionaryFactory.CreateStringMapFromJson(json);

            // Assert
            //Logger.Log(result);
            

            Assert.That(result[0].Item1, Is.EqualTo("array"));
            Assert.That(result[0].Item2, Is.TypeOf<LiquidCollection>());
            var array = (LiquidCollection)result[0].Item2;
            Assert.That(array.Select(x => x.Value.Value), Is.EquivalentTo(new List<int> {1,2,3}));


        }

    }
}

