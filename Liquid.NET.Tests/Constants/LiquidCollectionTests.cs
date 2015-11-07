using System.Collections.Generic;
using Liquid.NET.Constants;
using NUnit.Framework;

namespace Liquid.NET.Tests.Constants
{
    [TestFixture]
    public class LiquidCollectionTests
    {
        [Test]
        public void It_Should_Dereference_An_Array_Element()
        {

            // Arrange
            LiquidCollection liquidCollection = new LiquidCollection {
                LiquidString.Create("a string"), 
                LiquidNumeric.Create(123), 
                LiquidNumeric.Create(456m),
                new LiquidBoolean(false)
            };

            // Assert
            var valueAt = liquidCollection.ValueAt(0);
            Assert.That(valueAt.Value, Is.EqualTo("a string"));
        }

        [Test]
        public void It_Should_Access_Size_Property_Of_An_Array()
        {
            // Arrange
            LiquidCollection liquidCollection = new LiquidCollection
            {
                LiquidString.Create("a string"),
                LiquidNumeric.Create(123),
                LiquidNumeric.Create(456m),
                new LiquidBoolean(false)
            };
            var ctx = new TemplateContext().WithAllFilters();
            ctx.DefineLocalVariable("myarray", liquidCollection);
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ myarray.size }}", ctx);

            // Assert
          
            Assert.That(result, Is.EqualTo("Result : 4" ));
        }

        [Test]
        public void It_Should_Save_Meta_Data()
        {
            // Arrange

            var expected = "Hello";
            var liquidCollection = new LiquidCollection();

            // Act
            liquidCollection.MetaData["test"] = expected;

            // Assert
            Assert.That(liquidCollection.MetaData.ContainsKey("test"));
            Assert.That(liquidCollection.MetaData["test"], Is.EqualTo(expected));
            
        }

        [Test]
        public void It_Should_Clear_An_Array()
        {
            var liquidCollection = new LiquidCollection{LiquidString.Create("test")};
            liquidCollection.Clear();
            Assert.That(liquidCollection.Count, Is.EqualTo(0));
        }


        [Test]
        public void It_Should_Remove_An_Element_From_An_Array()
        {
            var liquidCollection = new LiquidCollection { LiquidString.Create("test") };
            liquidCollection.Remove(LiquidString.Create("test"));
            Assert.That(liquidCollection.Count, Is.EqualTo(0));
        }

        [Test]
        public void It_Should_Remove_An_Element_At_An_Index_In_An_Array()
        {
            var liquidCollection = new LiquidCollection { LiquidString.Create("test") };
            liquidCollection.RemoveAt(0);
            Assert.That(liquidCollection.Count, Is.EqualTo(0));
        }
        [Test]
        public void It_Should_Find_An_Element_By_Value()
        {
            var liquidCollection = new LiquidCollection { LiquidString.Create("test") };
            var index = liquidCollection.IndexOf(LiquidString.Create("test"));
            Assert.That(index, Is.EqualTo(0));
        }
        [Test]
        public void It_Should_Insert_An_Element()
        {
            var liquidCollection = new LiquidCollection { LiquidString.Create("test") };
            liquidCollection.Insert(0,LiquidString.Create("test 1"));
            Assert.That(liquidCollection.IndexOf(LiquidString.Create("test 1")), Is.EqualTo(0));
        }
        [Test]
        public void It_Should_Set_An_Element()
        {
            var liquidCollection = new LiquidCollection { LiquidString.Create("test") };
            liquidCollection[0] = LiquidString.Create("test 1");
            Assert.That(liquidCollection.IndexOf(LiquidString.Create("test 1")), Is.EqualTo(0));
        }
    }
}
