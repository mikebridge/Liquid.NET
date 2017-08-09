using System;
using Liquid.NET.Utils;
using Xunit;

namespace Liquid.NET.Tests.Utils
{
    
    public class OptionTests
    {
        [Fact]
        public void It_Should_Create_A_None_Option()
        {
            Assert.False(Option<String>.None().HasValue);
        }

        [Fact]
        //[ExpectedException(typeof(InvalidOperationException))]
        public void It_Should_Disallow_Dereferenced_None_Value()
        {
            // ReSharper disable once UnusedVariable
            //var result = Option<String>.None().Value;
            Assert.Throws<InvalidOperationException>(() => Option<String>.None().Value);

        }

        [Fact]
        public void It_Should_Create_A_Some_Option()
        {
            Assert.True(Option<String>.Create("Test").HasValue);
        }

        [Fact]
        public void It_Should_Store_The_Value()
        {
            Assert.Equal("Test", Option<String>.Create("Test").Value);
        }

        [Fact]
        public void Underlying_Values_Should_Be_Considered_Equal_With_Operator()
        {
            var val1 = Option<String>.Create("Test");
            var val2 = Option<String>.Create("Test");
            Assert.True(val1 == val2);
        }

        [Fact]
        public void Underlying_Values_Should_Be_Considered_Equal()
        {
            var val1 = Option<String>.Create("Test");
            var val2 = Option<String>.Create("Test");
            Assert.True(val1.Equals(val2));
        }

        [Fact]
        public void Same_Ref_Should_Be_Considered_Equal()
        {
            var val1 = Option<String>.Create("Test");
            // ReSharper disable once EqualExpressionComparison
            Assert.True(val1.Equals(val1));
        }


        [Fact]
        public void Underlying_Differing_Values_Should_Not_Be_Considered_Equal()
        {
            var val1 = Option<String>.Create("Test");
            var val2 = Option<String>.Create("Test2");
            Assert.False(val1 == val2);
        }

        [Fact]
        public void Underlying_Differing_Values_Should_Not_Be_Considered_Equal_To_Null()
        {
            var val1 = Option<String>.Create("Test");
            Assert.False(val1.Equals(null));
        }

        [Fact]
        public void Underlying_Differing_Values_Should_Be_Unequal()
        {
            var val1 = Option<String>.Create("Test");
            var val2 = Option<String>.Create("Test2");
            Assert.True(val1 != val2);
        }


        [Fact]
        public void Underlying_Differing_Values_Should_Be_Unequal_With_Other_Nulls()
        {
            var val1 = Option<String>.Create("Test");
            var val2 = (String) null;
            Assert.False(val1.Equals(val2));
        }

        [Fact]
        //[ExpectedException(typeof(ArgumentNullException))]
        public void Options_Cant_Be_Compared_To_Null()
        {
            var val1 = Option<String>.Create("Test");
            // ReSharper disable once UnusedVariable
            Assert.False(val1 == null);
        }

        [Fact]
        //[ExpectedException(typeof(ArgumentNullException))]
        public void Null_Compared_To_Option_Is_False()
        {
            var val1 = Option<String>.Create("Test");
            // ReSharper disable once UnusedVariable
            Assert.False(null == val1);
        }

        [Fact]
        public void It_Should_Use_WhenSome_When_Has_Value()
        {
            var isSome = false;
            Option<String>.Create("Test").WhenSome(x => isSome = true);
            Assert.True(isSome);
        }

        [Fact]
        public void It_Should_Not_Use_WhenNone_When_Has_Value()
        {
            var isNone = false;
            Option<String>.Create("Test").WhenNone(() => isNone = true);
            Assert.False(isNone);
        }

        [Fact]
        public void It_Should_Use_WhenNone_When_Has_No_Value()
        {
            var isNone = false;
            Option<String>.None().WhenNone(() => isNone = true);
            Assert.True(isNone);
        }

        [Fact]
        public void It_Should_Not_Use_WhenSome_When_Has_No_Value()
        {
            var isSome = false;
            Option<String>.None().WhenSome(x => isSome = true);
            Assert.False(isSome);
        }

        [Fact]
        public void It_Should_Chain_Some_Options()
        {
            var result = Option<String>.Create("Test").Bind(x => Option<String>.Create(x + "1"));
            Assert.Equal("Test1", result.Value);
        }

        [Fact]
        public void It_Should_Bind_None_To_Some()
        {
            var result = Option<String>.Create("Test").Bind(x => Option<String>.None());
            Assert.False(result.HasValue);
        }

        [Fact]
        public void It_Should_Bind_Some_To_None()
        {
            var result = Option<String>.None().Bind(x => Option<String>.Create("Test"));
            Assert.False(result.HasValue);
        }

        [Fact]
        //[ExpectedException(typeof(ArgumentNullException))]
        public void It_Should_Not_Bind_To_Null_func()
        {
            Assert.Throws<ArgumentNullException>(() => Option<String>.None().Bind(null));
        }

        [Fact]
        public void It_Should_Use_The_Underling_Hash_Value()
        {
            var result = Option<String>.Create("Test").GetHashCode();
            Assert.Equal("Test".GetHashCode(), result);
        }

        [Fact]
        public void GetOrElse_Should_Use_The_Default_When_None()
        {
            // Arrange
            var none = Option<String>.None(); 

            // Act
            var result = none.GetOrElse("nothing");

            // Assert
            Assert.Equal("nothing", result);

        }


        [Fact] 
        public void GetOrElse_Should_Use_The_Value_When_Some()
        {
            // Arrange
            var none = Option<String>.Create("Something");

            // Act
            var result = none.GetOrElse("nothing");

            // Assert
            Assert.Equal("Something", result);

        }


    }
}
