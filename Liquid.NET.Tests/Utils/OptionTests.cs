using System;
using Liquid.NET.Utils;
using NUnit.Framework;

namespace Liquid.NET.Tests.Utils
{
    [TestFixture]
    public class OptionTests
    {
        [Test]
        public void It_Should_Create_A_None_Option()
        {
            Assert.That(Option<String>.None().HasValue, Is.False);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void It_Should_Disallow_Dereferenced_None_Value()
        {
            // ReSharper disable once UnusedVariable
            var result = Option<String>.None().Value;
        }

        [Test]
        public void It_Should_Create_A_Some_Option()
        {
            Assert.That(Option<String>.Create("Test").HasValue, Is.True);
        }

        [Test]
        public void It_Should_Store_The_Value()
        {
            Assert.That(Option<String>.Create("Test").Value, Is.EqualTo("Test"));
        }

        [Test]
        public void Underlying_Values_Should_Be_Considered_Equal_With_Operator()
        {
            var val1 = Option<String>.Create("Test");
            var val2 = Option<String>.Create("Test");
            Assert.That(val1 == val2, Is.True);
        }

        [Test]
        public void Underlying_Values_Should_Be_Considered_Equal()
        {
            var val1 = Option<String>.Create("Test");
            var val2 = Option<String>.Create("Test");
            Assert.That(val1.Equals(val2), Is.True);
        }

        [Test]
        public void Same_Ref_Should_Be_Considered_Equal()
        {
            var val1 = Option<String>.Create("Test");
            // ReSharper disable once EqualExpressionComparison
            Assert.That(val1.Equals(val1), Is.True);
        }


        [Test]
        public void Underlying_Differing_Values_Should_Not_Be_Considered_Equal()
        {
            var val1 = Option<String>.Create("Test");
            var val2 = Option<String>.Create("Test2");
            Assert.That(val1 == val2, Is.False);
        }

        [Test]
        public void Underlying_Differing_Values_Should_Not_Be_Considered_Equal_To_Null()
        {
            var val1 = Option<String>.Create("Test");
            Assert.That(val1.Equals(null), Is.False);
        }

        [Test]
        public void Underlying_Differing_Values_Should_Be_Unequal()
        {
            var val1 = Option<String>.Create("Test");
            var val2 = Option<String>.Create("Test2");
            Assert.That(val1 != val2, Is.True);
        }


        [Test]
        public void Underlying_Differing_Values_Should_Be_Unequal_With_Other_Nulls()
        {
            var val1 = Option<String>.Create("Test");
            var val2 = (String) null;
            Assert.That(val1.Equals(val2), Is.False);
        }

        [Test]
        //[ExpectedException(typeof(ArgumentNullException))]
        public void Options_Cant_Be_Compared_To_Null()
        {
            var val1 = Option<String>.Create("Test");
            // ReSharper disable once UnusedVariable
            Assert.That(val1 == null, Is.False);
        }

        [Test]
        //[ExpectedException(typeof(ArgumentNullException))]
        public void Null_Compared_To_Option_Is_False()
        {
            var val1 = Option<String>.Create("Test");
            // ReSharper disable once UnusedVariable
            Assert.That(null == val1, Is.False);
        }

        [Test]
        public void It_Should_Use_WhenSome_When_Has_Value()
        {
            var isSome = false;
            Option<String>.Create("Test").WhenSome(x => isSome = true);
            Assert.That(isSome, Is.True);
        }

        [Test]
        public void It_Should_Not_Use_WhenNone_When_Has_Value()
        {
            var isNone = false;
            Option<String>.Create("Test").WhenNone(() => isNone = true);
            Assert.That(isNone, Is.False);
        }

        [Test]
        public void It_Should_Use_WhenNone_When_Has_No_Value()
        {
            var isNone = false;
            Option<String>.None().WhenNone(() => isNone = true);
            Assert.That(isNone, Is.True);
        }

        [Test]
        public void It_Should_Not_Use_WhenSome_When_Has_No_Value()
        {
            var isSome = false;
            Option<String>.None().WhenSome(x => isSome = true);
            Assert.That(isSome, Is.False);
        }

        [Test]
        public void It_Should_Chain_Some_Options()
        {
            var result = Option<String>.Create("Test").Bind(x => Option<String>.Create(x + "1"));
            Assert.That(result.Value, Is.EqualTo("Test1"));
        }

        [Test]
        public void It_Should_Bind_None_To_Some()
        {
            var result = Option<String>.Create("Test").Bind(x => Option<String>.None());
            Assert.That(result.HasValue, Is.False);
        }

        [Test]
        public void It_Should_Bind_Some_To_None()
        {
            var result = Option<String>.None().Bind(x => Option<String>.Create("Test"));
            Assert.That(result.HasValue, Is.False);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void It_Should_Not_Bind_To_Null_func()
        {
            // ReSharper disable once UnusedVariable
            var result = Option<String>.None().Bind(null);           
        }

        [Test]
        public void It_Should_Use_The_Underling_Hash_Value()
        {
            var result = Option<String>.Create("Test").GetHashCode();
            Assert.That(result, Is.EqualTo("Test".GetHashCode()));
        }

        [Test]
        public void GetOrElse_Should_Use_The_Default_When_None()
        {
            // Arrange
            var none = Option<String>.None(); 

            // Act
            var result = none.GetOrElse("nothing");

            // Assert
            Assert.That(result, Is.EqualTo("nothing"));

        }


        [Test] 
        public void GetOrElse_Should_Use_The_Value_When_Some()
        {
            // Arrange
            var none = Option<String>.Create("Something");

            // Act
            var result = none.GetOrElse("nothing");

            // Assert
            Assert.That(result, Is.EqualTo("Something"));

        }


    }
}
