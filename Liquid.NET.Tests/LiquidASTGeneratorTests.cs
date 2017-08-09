using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Symbols;
using Liquid.NET.Tags;
using Liquid.NET.Utils;
using Xunit;

namespace Liquid.NET.Tests
{
    
    public class LiquidASTGeneratorTests
    {
        [Fact]
        public void It_Should_Parse_An_Object_Expression()
        {
            // Arrange
            var ast = CreateAST("Result : {{ 123 }}");

            // Assert

            var liquidExpressions = FindNodesWithType(ast, typeof (LiquidExpressionTree));
            Logger.Log("There are " + ast.RootNode.Children.Count+" Nodes");
            Logger.Log("It is " + ast.RootNode.Children[0].Data);
            Assert.Equal(1, liquidExpressions.Count());
        }

        private static LiquidAST CreateAST(string template)
        {
            LiquidASTGenerator generator = new LiquidASTGenerator();

            // Act
           return generator.Generate(template).LiquidAST;

        }

        [Fact]
        public void It_Should_Parse_An_Object_Expression_With_A_Variable()
        {
            // Arrange
            var ast = CreateAST("Result : {{ a }}");

            // Assert

            var liquidExpressions = FindNodesWithType(ast, typeof(LiquidExpressionTree));
            Logger.Log("There are " + ast.RootNode.Children.Count + " Nodes");
            Assert.Equal(1, liquidExpressions.Count());

        }


        [Fact]
        public void It_Should_Parse_An_Object_Expression_With_An_Propertied_Variable()
        {
            // Arrange
            var ast = CreateAST("Result : {{ a.b }}");

            // Assert

            var liquidExpressions = FindNodesWithType(ast, typeof(LiquidExpressionTree));
            Logger.Log("There are " + ast.RootNode.Children.Count + " Nodes");
            Assert.Equal(1, liquidExpressions.Count());

        }



        [Fact]
        public void It_Should_Find_A_Filter()
        {
            // Arrange
            var ast = CreateAST("Result : {{ 123 | plus: 3}}");

            // Assert
            var liquidExpressions = FindNodesWithType(ast, typeof(LiquidExpressionTree)).FirstOrDefault();
            Assert.NotNull(liquidExpressions);
            // ReSharper disable once PossibleNullReferenceException
            var liquidExpression = ((LiquidExpressionTree)liquidExpressions.Data);

            Assert.Equal(1, liquidExpression.ExpressionTree.Data.FilterSymbols.Count);

        }

        [Fact]
        public void It_Should_Find_A_Filter_Argument()
        {
            // Arrange
            var ast = CreateAST("Result : {{ 123 | add: 3}}");

            // Assert
            var liquidExpressions = FindNodesWithType(ast, typeof (LiquidExpressionTree)).FirstOrDefault();
            Assert.NotNull(liquidExpressions);
            // ReSharper disable once PossibleNullReferenceException
            var liquidExpression = (LiquidExpressionTree) liquidExpressions.Data;
            // ReSharper disable once PossibleNullReferenceException
            Assert.Equal(1, liquidExpression.ExpressionTree.Data.FilterSymbols.FirstOrDefault().Args.Count);

        }

        [Fact]
        public void It_Can_Find_Two_Object_Expressions()
        {
            // Arrange
            var ast = CreateAST("Result : {{ 123 }} {{ 456 }}");

            // Assert
            var liquidExpressions = FindNodesWithType(ast, typeof(LiquidExpressionTree));

            Assert.Equal(2, liquidExpressions.Count());
        }

        [Fact]
        public void It_Should_Capture_The_Raw_Text()
        {
            // Arrange
            var ast = CreateAST("Result : {{ 123 | add: 3}} More Text.");

            // Assert
            var liquidExpressions = FindNodesWithType(ast, typeof(RawBlockTag));
            Assert.Equal(2, liquidExpressions.Count());

        }

        [Fact]
        public void It_Should_Find_An_If_Tag()
        {
            // Arrange
            var ast = CreateAST("Result : {% if true %} abcd {% endif %}");

            // Assert
            var tagExpressions = FindNodesWithType(ast, typeof(IfThenElseBlockTag));
            Assert.Equal(1, tagExpressions.Count());
            //Assert.NotNull(liquidExpressions);
            //Assert.Equal(1, ((LiquidExpression)liquidExpressions.Data).FilterSymbols.Count());

        }

        [Fact]
        public void It_Should_Find_An_If_Tag_With_ElsIfs_And_Else()
        {
            // Arrange
            var ast = CreateAST("Result : {% if true %} aaaa {% elsif false %} test 2 {% elsif true %} test 3 {% else %} ELSE {% endif %}");

            // Assert
            var tagExpressions = FindNodesWithType(ast, typeof(IfThenElseBlockTag));
            Assert.Equal(1, tagExpressions.Count());
            //Assert.NotNull(liquidExpressions);
            //Assert.Equal(1, ((LiquidExpression)liquidExpressions.Data).FilterSymbols.Count());

        }

        [Fact]
        public void It_Should_Find_An_Object_Expression_Inside_A_Block_ElsIfs_And_Else()
        {
            // Arrange
            var ast = CreateAST("Result : {% if true %} 33 + 4 = {{ 33 | add: 4}} {% else %} hello {% endif %}");
            var tagExpressions = FindNodesWithType(ast, typeof(IfThenElseBlockTag)).FirstOrDefault();
            // ReSharper disable once PossibleNullReferenceException
            var ifThenElseTag = (IfThenElseBlockTag) tagExpressions.Data;
            var liquidExpressions = FindWhere(ifThenElseTag.IfElseClauses[0].LiquidBlock.Children, typeof(LiquidExpressionTree));
            
            // Assert
            Assert.Equal(1, liquidExpressions.Count());
        }

        [Fact]
        public void It_Should_Nest_Expressions_Inside_Else()
        {
            // Arrange
            var ast = CreateAST("Result : {% if true %} 33 + 4 = {% if true %} {{ 33 | add: 4}} {% endif %}{% else %} hello {% endif %}");

            var tagExpressions = FindNodesWithType(ast, typeof(IfThenElseBlockTag)).FirstOrDefault();
            // ReSharper disable once PossibleNullReferenceException
            var ifThenElseTag = (IfThenElseBlockTag)tagExpressions.Data;
            var blockTags = ifThenElseTag.IfElseClauses[0].LiquidBlock.Children;
            var liquidExpressions = FindWhere(blockTags, typeof(IfThenElseBlockTag));

            // Assert
            Assert.Equal(1, liquidExpressions.Count());

        }

        private static void StartVisiting(IASTVisitor visitor, TreeNode<IASTNode> rootNode)
        {
            rootNode.Data.Accept(visitor);
            rootNode.Children.ForEach(child => StartVisiting(visitor, child));
        }

        [Fact]
        public void It_Should_Group_Expressions_In_Parens()
        {
            // Arrange
            var ast = CreateAST("Result : {% if true and (false or false) %}FALSE{% endif %}");

            var visitor= new DebuggingVisitor();

            StartVisiting(visitor,ast.RootNode);
             
            // Assert
            var tagExpressions = FindNodesWithType(ast, typeof(IfThenElseBlockTag)).ToList();
            var ifThenElseTag = (IfThenElseBlockTag) tagExpressions[0].Data;

            Assert.Equal(1, tagExpressions.Count);
            //Assert.IsType<AndExpression>(ifThenElseTag.IfElseClauses[0].RootNode.Data);            
            //Assert.IsType<AndExpression>(ifThenElseTag.IfElseClauses[0].RootNode[0].Data);
            var ifTagSymbol = ifThenElseTag.IfElseClauses[0];
            //Assert.IsType<AndExpression>(ifTagSymbol.RootNode.Data);
            var expressionSymbolTree = ifTagSymbol.LiquidExpressionTree;
            Assert.IsType<AndExpression>(expressionSymbolTree.Data.Expression);
            Assert.Equal(2, expressionSymbolTree.Children.Count);
            Assert.IsType<LiquidBoolean>(expressionSymbolTree[0].Data.Expression);
            Assert.IsType<GroupedExpression>(expressionSymbolTree[1].Data.Expression);
            Assert.Equal(1, expressionSymbolTree[1].Children.Count);
            Assert.IsType<OrExpression>(expressionSymbolTree[1][0].Data.Expression);
            Assert.IsType<LiquidBoolean>(expressionSymbolTree[1][0][0].Data.Expression);
            Assert.IsType<LiquidBoolean>(expressionSymbolTree[1][0][1].Data.Expression);
            //Assert.IsType<GroupedExpression>(ifThenElseTag.IfElseClauses[0].LiquidExpression[2].Data);
            
            //Assert.NotNull(liquidExpressions);
            //Assert.Equal(1, ((LiquidExpression)liquidExpressions.Data).FilterSymbols.Count());

        }

        [Theory]
        [InlineData("{{ a[b[c[d][e]][f][g[h]]] }}")]
        [InlineData("{{ a[b][c] }}")]
        [InlineData("{{ a[b] }}")]
        public void It_Should_Parse_An_Indexed_Object_Reference(String tmpl)
        {
            // Arrange
            var ast = CreateAST("Result : " + tmpl);

            // Assert

            var liquidExpressions = FindNodesWithType(ast, typeof(LiquidExpressionTree));
            
            Assert.NotNull(liquidExpressions);

            //Logger.Log("There are " + ast.RootNode.Children.Count + " Nodes");
            //Logger.Log("It is " + ast.RootNode.Children[0].Data);
            //Assert.Equal(1, liquidExpressions.Count());
            Assert.Equal(1, liquidExpressions.Count());
            //String result = VariableReferenceTreeBuilderTests.VariableReferenceTreePrinter.Print(liquidExpressions);
        }

        [Fact]
        public void It_Should_Parse_A_Regular_Variable()
        {
            String input = @"{{ v | plus: v }}";
            var template = LiquidTemplate.Create(input);
            
            // Act
            String result = template.LiquidTemplate.Render(new TemplateContext()
                .DefineLocalVariable("v", LiquidNumeric.Create(3))
                .WithAllFilters()).Result;
        
            // Assert
            Assert.Equal("6", result.Trim());
        }

        [Fact]
        public void It_Should_Generate_One_Error() // bug
        {
            // Arrange
            var templateResult = LiquidTemplate.Create("This tag delimiter is not terminated: {% .");

            // Assert
            Assert.Equal(1, templateResult.ParsingErrors.Count);
        }

        [Fact]
        public void It_Should_Generate_An_AST_Even_When_Parsing_Errors_Exist()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext();
            var templateResult = LiquidTemplate.Create("This filter is not terminated: {{ test" +
                                                       "Some more text");
            String error = String.Join(",", templateResult.ParsingErrors.Select(x => x.Message));

            Assert.Contains("Missing '}}'", error);
            Assert.NotNull(templateResult.LiquidTemplate);

            // Act
            var result = templateResult.LiquidTemplate.Render(ctx);

            // Assert
            //Console.WriteLine(result.Result);
            Assert.Contains("This filter is not terminated", result.Result);
            //Assert.Contains("Some more text", result.Result); // this seems to terminate here...


        }

        public static IEnumerable<TreeNode<IASTNode>> FindNodesWithType(LiquidAST ast, Type type)
        {
            return FindWhere(ast.RootNode.Children, type);
        }

        // ReSharper disable UnusedParameter.Local
        private static IEnumerable<TreeNode<IASTNode>> FindWhere(IEnumerable<TreeNode<IASTNode>> nodes, Type type)
        // ReSharper restore UnusedParameter.Local
        {
            return TreeNode<IASTNode>.FindWhere(nodes, x => x.GetType() == type);
        }
    }
}
