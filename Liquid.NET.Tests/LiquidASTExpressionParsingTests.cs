using System;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Symbols;
using Liquid.NET.Tags;
using Liquid.NET.Utils;
using Xunit;

namespace Liquid.NET.Tests
{
    
    public class LiquidASTExpressionParsingTests
    {
        readonly LiquidASTGenerator _generator;

        public LiquidASTExpressionParsingTests()
        {
            _generator = new LiquidASTGenerator();
        }

        [Fact]
        public void It_Should_Parse_And_And_Or()
        {
            
            // Act
            var ast = _generator.Generate("Result : {% if true and false or true %} OK {% endif %}");


            // Assert
            var ifThenSymbolNode =
                LiquidASTGeneratorTests.FindNodesWithType(ast.LiquidAST, typeof (IfThenElseBlockTag)).FirstOrDefault();
            Assert.NotNull(ifThenSymbolNode);
            // ReSharper disable once PossibleNullReferenceException
            var predicateTree =
                ((IfThenElseBlockTag)ifThenSymbolNode.Data).IfElseClauses[0].LiquidExpressionTree;
            foreach (var expr in ((IfThenElseBlockTag)ifThenSymbolNode.Data).IfElseClauses)
            {
                DebugIfExpressions(expr.LiquidExpressionTree);
            }
            Assert.IsType<OrExpression>(predicateTree.Data.Expression);
            Assert.IsType<AndExpression>(predicateTree[0].Data.Expression);
            Assert.IsType<LiquidBoolean>(predicateTree[1].Data.Expression);
            Assert.IsType<LiquidBoolean>(predicateTree[0][0].Data.Expression);
            Assert.IsType<LiquidBoolean>(predicateTree[0][1].Data.Expression);
        }

        [Fact]
        public void It_Should_Parse_If_Var_Equals_String()
        {

            // Act
            var ast = _generator.Generate("Result : {% if myvar == \"hello\" %} OK {% endif %}");

            // Assert
            var ifThenSymbolNode =
                LiquidASTGeneratorTests.FindNodesWithType(ast.LiquidAST, typeof(IfThenElseBlockTag)).FirstOrDefault();
            Assert.NotNull(ifThenSymbolNode);
            // ReSharper disable once PossibleNullReferenceException
            var predicateTree = ((IfThenElseBlockTag)ifThenSymbolNode.Data).IfElseClauses[0].LiquidExpressionTree;

            Assert.IsType<EqualsExpression>(predicateTree.Data.Expression);
            Assert.IsType<VariableReferenceTree>(predicateTree[0].Data.Expression);
            Assert.IsType<LiquidString>(predicateTree[1].Data.Expression);
            //Assert.That(predicateTree[0].Data, Is.TypeOf<VariableReference>());
            //Assert.That(predicateTree[1].Data, Is.TypeOf<String>());
        }

        [Fact]
        public void It_Should_Parse_ElsIf()
        {
           
            // Act
            var ast = _generator.Generate("Result : {% if true %} OK {% elsif false %} NO {% elsif true %} YES {% endif %}");

            // Assert
            var ifThenSymbolNode =
                LiquidASTGeneratorTests.FindNodesWithType(ast.LiquidAST, typeof(IfThenElseBlockTag)).FirstOrDefault();

            Assert.NotNull(ifThenSymbolNode);
            // ReSharper disable once PossibleNullReferenceException
            var elsIfPredicateTrees = ((IfThenElseBlockTag) ifThenSymbolNode.Data).IfElseClauses.Select(x => x.LiquidExpressionTree).ToList();

            foreach (var expr in ((IfThenElseBlockTag) ifThenSymbolNode.Data).IfElseClauses)
            {
                DebugIfExpressions(expr.LiquidExpressionTree);
            }
            Assert.Equal(3, elsIfPredicateTrees.Count);

            Assert.IsType<LiquidBoolean>(elsIfPredicateTrees[0].Data.Expression);
            Assert.IsType<LiquidBoolean>(elsIfPredicateTrees[1].Data.Expression);
        }

        private void DebugIfExpressions(TreeNode<LiquidExpression> ifExpression, int level = 0)
        {
  
            Logger.Log("-> " + new string(' ', level * 2) +  ifExpression.Data);
            foreach (var child in ifExpression.Children)
            {
                DebugIfExpressions(child, level + 1);
            }
            
        }

        [Fact]
        public void It_Should_Parse_Else()
        {
            // Act
            var ast = _generator.Generate("Result : {% if true %} OK {% elsif false %} NO {% elsif true %} YES {% else %} ELSE {% endif %}");

            // Assert
            var ifThenSymbolNode =
                LiquidASTGeneratorTests.FindNodesWithType(ast.LiquidAST, typeof(IfThenElseBlockTag)).FirstOrDefault();
            Assert.NotNull(ifThenSymbolNode);
            // ReSharper disable once PossibleNullReferenceException
            var elseSymbols = ((IfThenElseBlockTag) ifThenSymbolNode.Data).IfElseClauses;

            Logger.Log("-- AST --");
            Logger.Log(new ASTWalker().Walk(ast.LiquidAST));

            Assert.Equal(4, elseSymbols.Count); // the else symbol is an elsif set to "true".

        }

        [Fact]
        public void It_Should_Parse_Nested_If()
        {

            // Act
            var ast = _generator.Generate("Result : {% if true %} {% if false %} True and false {% endif %} {% endif %}");

            // Assert
            var parentIfThenElseSymbol = LiquidASTGeneratorTests.FindNodesWithType(ast.LiquidAST, typeof(IfThenElseBlockTag)).FirstOrDefault();

            // ReSharper disable once PossibleNullReferenceException
            var childIfThenElse = ((IfThenElseBlockTag) parentIfThenElseSymbol.Data).IfElseClauses[0].LiquidBlock;
            //Logger.Log(childIfThenElse);
            Assert.NotNull(childIfThenElse);

        }

        [Fact]
        public void It_Should_Add_A_Variable_Reference()
        {
            // Act
            var ast = _generator.Generate("Result : {% if myvar  %} OK {% endif %}");

            // Assert
            var ifThenElseNode = LiquidASTGeneratorTests.FindNodesWithType(ast.LiquidAST, typeof(IfThenElseBlockTag)).FirstOrDefault();
            Assert.NotNull(ifThenElseNode);
            // ReSharper disable once PossibleNullReferenceException
            var ifThenElseSymbol = ((IfThenElseBlockTag)ifThenElseNode.Data);

            // Assert
            Assert.IsType<VariableReferenceTree>(ifThenElseSymbol.IfElseClauses[0].LiquidExpressionTree.Data.Expression);

        }

        [Fact]
        public void It_Should_Add_An_Int_Indexed_Array_Reference_In_An_Object()
        {
            // Act
            var ast = _generator.Generate("Result : {{ myvar[3] }}");

            // Assert
            var liquidExpressionNode = LiquidASTGeneratorTests.FindNodesWithType(ast.LiquidAST, typeof(LiquidExpressionTree)).FirstOrDefault();
            Assert.NotNull(liquidExpressionNode);
            // ReSharper disable once PossibleNullReferenceException
            Assert.NotNull(liquidExpressionNode.Data);
            var liquidExpression = ((LiquidExpressionTree)liquidExpressionNode.Data);

            // Assert
            Assert.NotNull(liquidExpression.ExpressionTree.Data.Expression);
            Assert.IsType<VariableReferenceTree>(liquidExpression.ExpressionTree.Data.Expression);
            VariableReferenceTree varRefTree = (VariableReferenceTree)liquidExpression.ExpressionTree.Data.Expression;
            Assert.IsType<VariableReferenceTree>(varRefTree.IndexExpression);
            VariableReferenceTree indexVarRefTree = (VariableReferenceTree)varRefTree.IndexExpression;
            Assert.IsAssignableFrom<LiquidNumeric>(indexVarRefTree.Value);
            Assert.Equal(3, ((LiquidNumeric) indexVarRefTree.Value).IntValue);

        }

        [Fact]
        public void It_Should_Add_A_String_Indexed_Array_Reference_In_An_Object()
        {
            // Act
            var ast = _generator.Generate("Result : {{ myvar[\"test\"] }}");

            // Assert
            var liquidExpressionNode = LiquidASTGeneratorTests.FindNodesWithType(ast.LiquidAST, typeof(LiquidExpressionTree)).FirstOrDefault();
            Assert.NotNull(liquidExpressionNode);
            // ReSharper disable once PossibleNullReferenceException
            Assert.NotNull(liquidExpressionNode.Data);
            var liquidExpression = ((LiquidExpressionTree)liquidExpressionNode.Data);

            // Assert
            Assert.NotNull(liquidExpression.ExpressionTree.Data.Expression);
            Assert.IsType<VariableReferenceTree>(liquidExpression.ExpressionTree.Data.Expression);
            VariableReferenceTree varRefTree = (VariableReferenceTree) liquidExpression.ExpressionTree.Data.Expression;
            Assert.IsType<VariableReferenceTree>(varRefTree.IndexExpression);
            VariableReferenceTree indexVarRefTree = (VariableReferenceTree)varRefTree.IndexExpression;
            Assert.IsType<LiquidString>(indexVarRefTree.Value);

        }

        [Fact]
        public void It_Should_Add_A_Boolean_Reference()
        {
            // Act
            var ast = _generator.Generate("Result : {% if true %} OK {% endif %}");
            var ifThenElseNode = LiquidASTGeneratorTests.FindNodesWithType(ast.LiquidAST, typeof(IfThenElseBlockTag)).FirstOrDefault();
            Assert.NotNull(ifThenElseNode);
            // ReSharper disable once PossibleNullReferenceException
            var ifThenElseSymbol = ((IfThenElseBlockTag)ifThenElseNode.Data);

            // Assert
            Assert.IsType<LiquidBoolean>(ifThenElseSymbol.IfElseClauses[0].LiquidExpressionTree.Data.Expression);

        }

        [Fact]
        public void It_Should_Add_A_String_Reference()
        {
            // Act
            var ast = _generator.Generate("Result : {% if \"hello\" %} OK {% endif %}");
            var ifThenElseNode = LiquidASTGeneratorTests.FindNodesWithType(ast.LiquidAST, typeof(IfThenElseBlockTag)).FirstOrDefault();
            Assert.NotNull(ifThenElseNode);
            // ReSharper disable once PossibleNullReferenceException
            var ifThenElseSymbol = ((IfThenElseBlockTag)ifThenElseNode.Data);

            // Assert
            Assert.IsType<LiquidString>(ifThenElseSymbol.IfElseClauses[0].LiquidExpressionTree.Data.Expression);

        }

 


    }
}
