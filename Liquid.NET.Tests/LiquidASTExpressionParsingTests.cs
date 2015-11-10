using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Symbols;
using Liquid.NET.Tags;
using Liquid.NET.Utils;
using NUnit.Framework;

namespace Liquid.NET.Tests
{
    [TestFixture]
    public class LiquidASTExpressionParsingTests
    {
        LiquidASTGenerator _generator;

        [SetUp]
        public void SetUp()
        {
            _generator = new LiquidASTGenerator();
        }

        [Test]
        public void It_Should_Parse_And_And_Or()
        {
            
            // Act
            var ast = _generator.Generate("Result : {% if true and false or true %} OK {% endif %}");


            // Assert
            var ifThenSymbolNode =
                LiquidASTGeneratorTests.FindNodesWithType(ast.LiquidAST, typeof (IfThenElseBlockTag)).FirstOrDefault();
            Assert.That(ifThenSymbolNode, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            var predicateTree =
                ((IfThenElseBlockTag)ifThenSymbolNode.Data).IfElseClauses[0].LiquidExpressionTree;
            foreach (var expr in ((IfThenElseBlockTag)ifThenSymbolNode.Data).IfElseClauses)
            {
                DebugIfExpressions(expr.LiquidExpressionTree);
            }
            Assert.That(predicateTree.Data.Expression, Is.TypeOf<OrExpression>());
            Assert.That(predicateTree[0].Data.Expression, Is.TypeOf<AndExpression>());
            Assert.That(predicateTree[1].Data.Expression, Is.TypeOf<LiquidBoolean>());
            Assert.That(predicateTree[0][0].Data.Expression, Is.TypeOf<LiquidBoolean>());
            Assert.That(predicateTree[0][1].Data.Expression, Is.TypeOf<LiquidBoolean>());
        }

        [Test]
        public void It_Should_Parse_If_Var_Equals_String()
        {

            // Act
            var ast = _generator.Generate("Result : {% if myvar == \"hello\" %} OK {% endif %}");

            // Assert
            var ifThenSymbolNode =
                LiquidASTGeneratorTests.FindNodesWithType(ast.LiquidAST, typeof(IfThenElseBlockTag)).FirstOrDefault();
            Assert.That(ifThenSymbolNode, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            var predicateTree = ((IfThenElseBlockTag)ifThenSymbolNode.Data).IfElseClauses[0].LiquidExpressionTree;

            Assert.That(predicateTree.Data.Expression, Is.TypeOf<EqualsExpression>());
            Assert.That(predicateTree[0].Data.Expression, Is.TypeOf<VariableReferenceTree>());
            Assert.That(predicateTree[1].Data.Expression, Is.TypeOf<LiquidString>());
            //Assert.That(predicateTree[0].Data, Is.TypeOf<VariableReference>());
            //Assert.That(predicateTree[1].Data, Is.TypeOf<String>());
        }

        [Test]
        public void It_Should_Parse_ElsIf()
        {
           
            // Act
            var ast = _generator.Generate("Result : {% if true %} OK {% elsif false %} NO {% elsif true %} YES {% endif %}");

            // Assert
            var ifThenSymbolNode =
                LiquidASTGeneratorTests.FindNodesWithType(ast.LiquidAST, typeof(IfThenElseBlockTag)).FirstOrDefault();

            Assert.That(ifThenSymbolNode, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            var elsIfPredicateTrees = ((IfThenElseBlockTag) ifThenSymbolNode.Data).IfElseClauses.Select(x => x.LiquidExpressionTree).ToList();

            foreach (var expr in ((IfThenElseBlockTag) ifThenSymbolNode.Data).IfElseClauses)
            {
                DebugIfExpressions(expr.LiquidExpressionTree);
            }
            Assert.That(elsIfPredicateTrees.Count, Is.EqualTo(3));

            Assert.That(elsIfPredicateTrees[0].Data.Expression, Is.TypeOf<LiquidBoolean>());
            Assert.That(elsIfPredicateTrees[1].Data.Expression, Is.TypeOf<LiquidBoolean>());
        }

        private void DebugIfExpressions(TreeNode<LiquidExpression> ifExpression, int level = 0)
        {
  
            Logger.Log("-> " + new string(' ', level * 2) +  ifExpression.Data);
            foreach (var child in ifExpression.Children)
            {
                DebugIfExpressions(child, level + 1);
            }
            
        }

        [Test]
        public void It_Should_Parse_Else()
        {
            // Act
            var ast = _generator.Generate("Result : {% if true %} OK {% elsif false %} NO {% elsif true %} YES {% else %} ELSE {% endif %}");

            // Assert
            var ifThenSymbolNode =
                LiquidASTGeneratorTests.FindNodesWithType(ast.LiquidAST, typeof(IfThenElseBlockTag)).FirstOrDefault();
            Assert.That(ifThenSymbolNode, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            var elseSymbols = ((IfThenElseBlockTag) ifThenSymbolNode.Data).IfElseClauses;

            Logger.Log("-- AST --");
            Logger.Log(new ASTWalker().Walk(ast.LiquidAST));

            Assert.That(elseSymbols.Count, Is.EqualTo(4)); // the else symbol is an elsif set to "true".

        }

        [Test]
        public void It_Should_Parse_Nested_If()
        {

            // Act
            var ast = _generator.Generate("Result : {% if true %} {% if false %} True and false {% endif %} {% endif %}");

            // Assert
            var parentIfThenElseSymbol = LiquidASTGeneratorTests.FindNodesWithType(ast.LiquidAST, typeof(IfThenElseBlockTag)).FirstOrDefault();

            // ReSharper disable once PossibleNullReferenceException
            var childIfThenElse = ((IfThenElseBlockTag) parentIfThenElseSymbol.Data).IfElseClauses[0].LiquidBlock;
            //Logger.Log(childIfThenElse);
            Assert.That(childIfThenElse, Is.Not.Null);

        }

        [Test]
        public void It_Should_Add_A_Variable_Reference()
        {
            // Act
            var ast = _generator.Generate("Result : {% if myvar  %} OK {% endif %}");

            // Assert
            var ifThenElseNode = LiquidASTGeneratorTests.FindNodesWithType(ast.LiquidAST, typeof(IfThenElseBlockTag)).FirstOrDefault();
            Assert.That(ifThenElseNode, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            var ifThenElseSymbol = ((IfThenElseBlockTag)ifThenElseNode.Data);

            // Assert
            Assert.That(ifThenElseSymbol.IfElseClauses[0].LiquidExpressionTree.Data.Expression, Is.TypeOf<VariableReferenceTree>());

        }

        [Test]
        public void It_Should_Add_An_Int_Indexed_Array_Reference_In_An_Object()
        {
            // Act
            var ast = _generator.Generate("Result : {{ myvar[3] }}");

            // Assert
            var liquidExpressionNode = LiquidASTGeneratorTests.FindNodesWithType(ast.LiquidAST, typeof(LiquidExpressionTree)).FirstOrDefault();
            Assert.That(liquidExpressionNode, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(liquidExpressionNode.Data, Is.Not.Null);
            var liquidExpression = ((LiquidExpressionTree)liquidExpressionNode.Data);

            // Assert
            Assert.That(liquidExpression.ExpressionTree.Data.Expression, Is.Not.Null);
            Assert.That(liquidExpression.ExpressionTree.Data.Expression, Is.TypeOf<VariableReferenceTree>());
            VariableReferenceTree varRefTree = (VariableReferenceTree)liquidExpression.ExpressionTree.Data.Expression;
            Assert.That(varRefTree.IndexExpression, Is.TypeOf<VariableReferenceTree>());
            VariableReferenceTree indexVarRefTree = (VariableReferenceTree)varRefTree.IndexExpression;
            Assert.That(indexVarRefTree.Value, Is.AssignableTo<LiquidNumeric>());
            Assert.That(((LiquidNumeric) indexVarRefTree.Value).IntValue, Is.EqualTo(3));

        }

        [Test]
        public void It_Should_Add_A_String_Indexed_Array_Reference_In_An_Object()
        {
            // Act
            var ast = _generator.Generate("Result : {{ myvar[\"test\"] }}");

            // Assert
            var liquidExpressionNode = LiquidASTGeneratorTests.FindNodesWithType(ast.LiquidAST, typeof(LiquidExpressionTree)).FirstOrDefault();
            Assert.That(liquidExpressionNode, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(liquidExpressionNode.Data, Is.Not.Null);
            var liquidExpression = ((LiquidExpressionTree)liquidExpressionNode.Data);

            // Assert
            Assert.That(liquidExpression.ExpressionTree.Data.Expression, Is.Not.Null);
            Assert.That(liquidExpression.ExpressionTree.Data.Expression, Is.TypeOf<VariableReferenceTree>());
            VariableReferenceTree varRefTree = (VariableReferenceTree) liquidExpression.ExpressionTree.Data.Expression;
            Assert.That(varRefTree.IndexExpression, Is.TypeOf<VariableReferenceTree>());
            VariableReferenceTree indexVarRefTree = (VariableReferenceTree)varRefTree.IndexExpression;
            Assert.That(indexVarRefTree.Value, Is.TypeOf<LiquidString>());

        }

        [Test]
        public void It_Should_Add_A_Boolean_Reference()
        {
            // Act
            var ast = _generator.Generate("Result : {% if true %} OK {% endif %}");
            var ifThenElseNode = LiquidASTGeneratorTests.FindNodesWithType(ast.LiquidAST, typeof(IfThenElseBlockTag)).FirstOrDefault();
            Assert.That(ifThenElseNode, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            var ifThenElseSymbol = ((IfThenElseBlockTag)ifThenElseNode.Data);

            // Assert
            Assert.That(ifThenElseSymbol.IfElseClauses[0].LiquidExpressionTree.Data.Expression, Is.TypeOf<LiquidBoolean>());

        }

        [Test]
        public void It_Should_Add_A_String_Reference()
        {
            // Act
            var ast = _generator.Generate("Result : {% if \"hello\" %} OK {% endif %}");
            var ifThenElseNode = LiquidASTGeneratorTests.FindNodesWithType(ast.LiquidAST, typeof(IfThenElseBlockTag)).FirstOrDefault();
            Assert.That(ifThenElseNode, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            var ifThenElseSymbol = ((IfThenElseBlockTag)ifThenElseNode.Data);

            // Assert
            Assert.That(ifThenElseSymbol.IfElseClauses[0].LiquidExpressionTree.Data.Expression, Is.TypeOf<LiquidString>());

        }

 


    }
}
