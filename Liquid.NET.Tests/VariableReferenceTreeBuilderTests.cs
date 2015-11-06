using System;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using NUnit.Framework;

namespace Liquid.NET.Tests
{
    [TestFixture]
    public class VariableReferenceTreeBuilderTests
    {

        [Test]
        public void It_Should_Create_A_Variable_Reference()
        {
            // Arrange

            // Act
            var builder = new LiquidASTGenerator.VariableReferenceTreeBuilder();
            builder.StartVariable();
            builder.AddVarName("test");
            builder.EndVariable();
            Logger.Log("RESULT: " + VariableReferenceTreePrinter.Print(builder.Result));

            // Assert
            Assert.That(builder.Result.Value, Is.TypeOf<VariableReference>());
            var varReference = (VariableReference) builder.Result.Value;
            Assert.That(varReference.Name, Is.EqualTo("test"));

        }

        [Test]
        public void It_Should_Create_A_Variable_With_An_Index()
        {
            // Arrange
            // test[idx]
            var builder = new LiquidASTGenerator.VariableReferenceTreeBuilder();
            
            // Act
            builder.StartVariable();
            builder.AddVarName("test");

            builder.StartIndex();
            builder.StartVariable();
            builder.AddVarName("idx");
            builder.EndVariable();
            builder.EndIndex();
                        
            builder.EndVariable();
            String result = VariableReferenceTreePrinter.Print(builder.Result);

            // Assert
            Assert.That(result, Is.EqualTo("test[idx]"));

        }

        [Test]
        public void It_Should_Create_A_Variable_With_Two_Indexes()
        {
            // Arrange
            // test[idx1][idx2]
            
            var builder = new LiquidASTGenerator.VariableReferenceTreeBuilder();
            
            // Act
            builder.StartVariable();
            builder.AddVarName("test");

            builder.StartIndex();
            builder.StartVariable();
            builder.AddVarName("idx1");
            builder.EndVariable();
            builder.EndIndex();
            
            builder.StartIndex();
            builder.StartVariable();
            builder.AddVarName("idx2");
            builder.EndVariable();
            builder.EndIndex();
            

            builder.EndVariable();
            //Logger.Log("RESULT: " + VariableReferenceTreePrinter.Print(builder.Result));

            String result = VariableReferenceTreePrinter.Print(builder.Result);

            // Assert
            Assert.That(result, Is.EqualTo("test[idx1][idx2]"));

        }

        [Test]
        public void It_Should_Create_A_Variable_With_Nested_Indexes()
        {
            // Arrange
            // test[idx1[idx2]]

            var builder = new LiquidASTGenerator.VariableReferenceTreeBuilder();

            // Act
            builder.StartVariable();
            builder.AddVarName("test");

            builder.StartIndex();
            builder.StartVariable();
            builder.AddVarName("idx1");
            builder.EndVariable();

            builder.StartIndex();
            builder.StartVariable();
            builder.AddVarName("idx2");
            builder.EndVariable();
            builder.EndIndex();
            
            builder.EndIndex();

            builder.EndVariable();
            
            String result = VariableReferenceTreePrinter.Print(builder.Result);

            // Assert
            Assert.That(result, Is.EqualTo("test[idx1[idx2]]"));

        }

        [Test]
        public void It_Should_Create_A_Variable_With_Numeric_Indices()
        {
            // Arrange
            // test[3[5]]

            var builder = new LiquidASTGenerator.VariableReferenceTreeBuilder();

            // Act
            builder.StartVariable();
            builder.AddVarName("test");

            builder.StartIndex();
            builder.StartVariable();
            builder.AddIntIndex(3);
            builder.EndVariable();

            builder.StartIndex();
            builder.StartVariable();
            builder.AddIntIndex(5);
            builder.EndVariable();
            builder.EndIndex();

            builder.EndIndex();

            builder.EndVariable();

            String result = VariableReferenceTreePrinter.Print(builder.Result);

            // Assert
            Assert.That(result, Is.EqualTo("test[3[5]]"));

        }

        [Test]
        public void It_Should_Create_A_Variable_With_String_Indices()
        {
            // Arrange
            // test[3[5]]

            var builder = new LiquidASTGenerator.VariableReferenceTreeBuilder();

            // Act
            builder.StartVariable();
            builder.AddVarName("test");

            builder.StartIndex();
            builder.StartVariable();
            builder.AddStringIndex("test1");
            builder.EndVariable();

            builder.EndIndex();

            builder.EndVariable();

            String result = VariableReferenceTreePrinter.Print(builder.Result);

            // Assert
            Assert.That(result, Is.EqualTo("test[\"test1\"]"));

        }

        [Test]
        public void It_Should_Pare()
        {
            // Arrange
            // test[3[5]]

            var builder = new LiquidASTGenerator.VariableReferenceTreeBuilder();

            // Act
            builder.StartVariable();
            builder.AddVarName("test");

            builder.StartIndex();
            builder.StartVariable();
            builder.AddStringIndex("test1");
            builder.EndVariable();

            builder.EndIndex();

            builder.EndVariable();

            String result = VariableReferenceTreePrinter.Print(builder.Result);

            // Assert
            Assert.That(result, Is.EqualTo("test[\"test1\"]"));

        }

        [Test]
        public void It_Should_Create_A_Variable_With_Very_Nested_Indexes()
        {
            // Arrange
            // a[b[c][d]][e]

            var builder = new LiquidASTGenerator.VariableReferenceTreeBuilder();

            // Act
            builder.StartVariable();
            builder.AddVarName("a"); // a

            builder.StartIndex(); // [

            builder.StartVariable();
            builder.AddVarName("b"); // b
            builder.EndVariable();

            builder.StartIndex(); // [

            builder.StartVariable();
            builder.AddVarName("c"); // c
            builder.EndVariable();

            builder.EndIndex(); // ]
            Logger.Log("1) " + VariableReferenceTreePrinter.Print(builder.Result));
            builder.StartIndex(); // [
            Logger.Log("2) " + VariableReferenceTreePrinter.Print(builder.Result));
            builder.StartVariable();
            Logger.Log("3) " + VariableReferenceTreePrinter.Print(builder.Result));
            builder.AddVarName("d"); // d
            builder.EndVariable();
            Logger.Log("4) " + VariableReferenceTreePrinter.Print(builder.Result));

            builder.EndIndex(); // ]
            Logger.Log("5) " + VariableReferenceTreePrinter.Print(builder.Result));
            builder.EndIndex(); // ]
            Logger.Log("6) " + VariableReferenceTreePrinter.Print(builder.Result));
            builder.StartIndex(); // [
            Logger.Log("7) " + VariableReferenceTreePrinter.Print(builder.Result));
            builder.StartVariable();
            Logger.Log("8) " + VariableReferenceTreePrinter.Print(builder.Result));
            builder.AddVarName("e");
            builder.EndVariable();
            builder.EndIndex(); // ]

            builder.EndVariable();

            String result = VariableReferenceTreePrinter.Print(builder.Result);

            // Assert
            Assert.That(result, Is.EqualTo("a[b[c][d]][e]"));
            //            Assert.That(builder.Result.Value, Is.TypeOf<VariableReference>());
            //            var node = (VariableReferenceTree)builder.Result.Value;
            //            var varReferenceValue = (VariableReference)node.Value;
            //            var varReferenceIndex = (VariableReference)node.IndexExpression;
            //
            //            Assert.That(varReferenceValue.Name, Is.EqualTo("test"));
            //            Assert.That(varReferenceIndex.Name, Is.EqualTo("idx1"));

        }

        public static class VariableReferenceTreePrinter
        {
            public static String Print(IExpressionDescription ex)
            {
                if (ex == null)
                {
                    return "";
                }
                return AsString((dynamic) ex);
            }

            private static String AsString(VariableReferenceTree tree)
            {
                var result = Print(tree.Value);
                if (tree.IndexExpression != null)
                {
                    result += "[" + Print(tree.IndexExpression) + "]";
                }
                return result;
            }

            private static String AsString(LiquidNumeric ex)
            {
                return ex.ToString();
            }

            private static String AsString(LiquidString ex)
            {
                return "\"" + ex + "\"";
            }


            private static String AsString(IExpressionDescription ex)
            {
                return "Unknown " + ex;
            }

            private static String AsString(VariableReference varref)
            {
                return varref.Name;
            }

        }
    }
}
