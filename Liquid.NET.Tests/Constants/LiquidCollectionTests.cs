using System.Collections.Generic;
using Liquid.NET.Constants;
using Xunit;

namespace Liquid.NET.Tests.Constants
{
    
    public class LiquidCollectionTests
    {
        [Fact]
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
            Assert.Equal("a string", valueAt.Value.ToString());
        }

        [Fact]
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
          
            Assert.Equal("Result : 4" , result);
        }

        [Fact]
        public void It_Should_Save_Meta_Data()
        {
            // Arrange

            var expected = "Hello";
            var liquidCollection = new LiquidCollection();

            // Act
            liquidCollection.MetaData["test"] = expected;

            // Assert
            Assert.True(liquidCollection.MetaData.ContainsKey("test"));
            Assert.Equal(expected, liquidCollection.MetaData["test"]);
            
        }

        [Fact]
        public void It_Should_Clear_An_Array()
        {
            var liquidCollection = new LiquidCollection{LiquidString.Create("test")};
            liquidCollection.Clear();
            Assert.Empty(liquidCollection);
        }


        [Fact]
        public void It_Should_Remove_An_Element_From_An_Array()
        {
            var liquidCollection = new LiquidCollection { LiquidString.Create("test") };
            liquidCollection.Remove(LiquidString.Create("test"));
            Assert.Empty(liquidCollection);
        }

        [Fact]
        public void It_Should_Remove_An_Element_At_An_Index_In_An_Array()
        {
            var liquidCollection = new LiquidCollection { LiquidString.Create("test") };
            liquidCollection.RemoveAt(0);
            Assert.Empty(liquidCollection);
        }
        [Fact]
        public void It_Should_Find_An_Element_By_Value()
        {
            var liquidCollection = new LiquidCollection { LiquidString.Create("test") };
            var index = liquidCollection.IndexOf(LiquidString.Create("test"));
            Assert.Equal(0, index);
        }
        [Fact]
        public void It_Should_Insert_An_Element()
        {
            var liquidCollection = new LiquidCollection { LiquidString.Create("test") };
            liquidCollection.Insert(0,LiquidString.Create("test 1"));
            Assert.Equal(0, liquidCollection.IndexOf(LiquidString.Create("test 1")));
        }

        [Fact]
        public void It_Should_Cast_A_Null_Element_To_None()
        {
            var liquidCollection = new LiquidCollection { null };
            Assert.Single(liquidCollection);
        }

        [Fact]
        public void It_Should_Set_An_Element()
        {
            var liquidCollection = new LiquidCollection { LiquidString.Create("test") };
            liquidCollection[0] = LiquidString.Create("test 1");
            Assert.Equal(0, liquidCollection.IndexOf(LiquidString.Create("test 1")));
        }
    }
}
