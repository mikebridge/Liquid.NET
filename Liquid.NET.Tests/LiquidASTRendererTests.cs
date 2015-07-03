using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Tags;
using Liquid.NET.Tests.Filters.Array;
using Liquid.NET.Utils;
using NUnit.Framework;

namespace Liquid.NET.Tests
{
    [TestFixture]
    public class LiquidASTRendererTests
    {
        private LiquidASTGenerator _generator;

        [SetUp]
        public void SetUp()
        {
            _generator = new LiquidASTGenerator();
        }

        [Test]
        [TestCase("\"hello\"", "hello")]
        [TestCase("'hello'", "hello")]
        [TestCase("123", "123")]
        [TestCase("true", "true")]
        public void It_Should_Render_Simple_Literals(String literal, String expected)
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();
          
            // Arrange
            var ast = _generator.Generate("Result : {{ "+literal+" }}");

            // Act
            String result = new LiquidASTRenderer().Render(templateContext, ast);
            // Act

            // Assert
            Assert.That(result, Is.EqualTo("Result : "+expected));

        }

        
        [Test]
        public void It_Should_Parse_An_AST()
        {
            // Arrange
            const string helloWorld = "HELLO WORLD";
            LiquidAST ast = new LiquidAST();
            TemplateContext ctx = new TemplateContext();
            ast.RootNode.AddChild(new TreeNode<IASTNode>(new RawBlockTag(helloWorld)));
            var evaluator = new LiquidASTRenderer();

            // Act
            String result = evaluator.Render(ctx, ast);

            // Assert
            Assert.That(result, Is.EqualTo(helloWorld));

        }


        [Test]
        [TestCase("hello", "hello", "equal")]
        [TestCase("hello", "hello2", "not equal")]
        [TestCase("hello", null, "not equal")]
        [TestCase(null, "hello", "not equal")]
        [TestCase(null, null, "equal")]
        public void It_Should_Compare_Two_Equal_Strings(String str1, String str2, String expected)
        {
            // Arrange
            var ast =
                _generator.Generate("Result : {% if \"" + str1 + "\" == \"" + str2 +
                                    "\" %}equal{% else %}not equal{% endif %}");

            // Act
            var evaluator = new LiquidASTRenderer();

            // Act
            String result = evaluator.Render(new TemplateContext(), ast);

            // Assert
            Assert.That(result, Is.EqualTo("Result : " + expected));

        }

        [Test]
        [TestCase("true", "and", "true", "TRUE")]
        [TestCase("false", "and", "true", "FALSE")]
        [TestCase("true", "and", "false", "FALSE")]
        [TestCase("false", "and", "false", "FALSE")]
        [TestCase("\"test\"==\"test\"", "and", "true", "TRUE")]
        [TestCase("\"test\"==\"zzz\"", "and", "true", "FALSE")]
        [TestCase("true", "or", "true", "TRUE")]
        [TestCase("false", "or", "true", "TRUE")]
        [TestCase("true", "or", "false", "TRUE")]
        [TestCase("false", "or", "false", "FALSE")]
        public void It_Should_AndOr_Two_Expressions(String str1, String op, String str2, String expected)
        {
            // Arrange
            var ast = _generator.Generate("Result : {% if " + str1 + " " + op + " " + str2 +" %}TRUE{% else %}FALSE{% endif %}");
            var evaluator = new LiquidASTRenderer();

            // Act
            String result = evaluator.Render(new TemplateContext(), ast);

            // Assert
            Assert.That(result, Is.EqualTo("Result : " + expected));

        }

        [Test]
        public void It_Should_Resolve_A_Variable_In_An_Expression()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();
            templateContext.Define("myVar", new StringValue("My Variable"));
            var ast = _generator.Generate("Result : {% if myVar %}OK{% else %}NOT OK{% endif %}");
            var evaluator = new LiquidASTRenderer();

            // Act
            String result = evaluator.Render(templateContext, ast);

            // Assert
            Assert.That(result, Is.EqualTo("Result : OK"));

        }

        [Test]
        public void It_Should_Eval_An_Undefine_Variable_In_An_IfExpression_As_False()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();
            var ast = _generator.Generate("Result : {% if myUndefinedVar %}OK{% else %}NOT OK{% endif %}");
            var evaluator = new LiquidASTRenderer();

            // Act
            String result = evaluator.Render(templateContext, ast);

            // Assert
            Assert.That(result, Is.EqualTo("Result : NOT OK"));

        }

        [Test]
        public void It_Should_Render_A_Defined_Variable_In_A_LiquidExpression()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();
            const string val = "My Variable";
            templateContext.Define("myVar", new StringValue(val));
            var ast = _generator.Generate("Result : {{ myVar }}");
            var evaluator = new LiquidASTRenderer();

            // Act
            String result = evaluator.Render(templateContext, ast);

            // Assert
            // TODO: What is this supposed to do?
            Assert.That(result, Is.EqualTo("Result : " + val));

        }

   
        [Test]
        public void It_Should_Not_Render_An_Undefined_Variable_In_A_LiquidExpression()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();
            var ast = _generator.Generate("Result : {{ myUndefinedVar }}");
            var evaluator = new LiquidASTRenderer();

            // Act
            String result = evaluator.Render(templateContext, ast);

            // Assert
            // TODO: What is this supposed to do?
            Assert.That(result, Is.EqualTo("Result : "));

        }

        [Test]
        [TestCase("true and false", "FALSE")]
        [TestCase("true and (false or true)", "TRUE")]
        [TestCase("(true and false) or false", "FALSE")]
        public void It_Should_Group_Expressions(String expr, String expected)
        {
            // Arrange
            var ast = _generator.Generate("Result : {% if " + expr + " %}TRUE{% else %}FALSE{% endif %}");

            // Act
            String result = new LiquidASTRenderer().Render(new TemplateContext(), ast);

            Console.WriteLine("result is " + result);
            // Assert
            Assert.That(result, Is.EqualTo("Result : " + expected));

        }

        [Test]
        public void It_Should_Evaluate_A_Filter_Inside_A_Tag()
        {
            // Arrange
            var templateContext = new TemplateContext().WithAllFilters();
            templateContext.Define("myvar", new StringValue("hello"));
            var ast = _generator.Generate("Result : {% if myvar | upcase == \"HELLO\" %}TRUE{% else %}FALSE{% endif %}");

            // Act
            String result = new LiquidASTRenderer().Render(templateContext, ast);
            Console.WriteLine("result is " + result);

            // Assert
            Assert.That(result, Is.EqualTo("Result : TRUE"));

        }

        [Test]
        public void It_Should_Evaluate_An_Index_Inside_A_Tag()
        {
            // Arrange
            var templateContext = new TemplateContext().WithAllFilters();
            templateContext.Define("myvar", new ArrayValue(new List<IExpressionConstant>{new StringValue("hello")}));
            var ast = _generator.Generate("Result : {% if myvar[0] | upcase == \"HELLO\" %}TRUE{% else %}FALSE{% endif %}");

            // Act
            String result = new LiquidASTRenderer().Render(templateContext, ast);
            Console.WriteLine("result is " + result);

            // Assert
            Assert.That(result, Is.EqualTo("Result : TRUE"));

        }

//        [Test]
//        public void It_Should_Render_A_Dictionary()
//        {
//            // Arrange
//            TemplateContext templateContext = new TemplateContext();
//            var dict = new Dictionary<String, String> { { "test1", "test element" } };
//
//            templateContext.Define("mydict", new ObjectValue(dict));
//
//            // Arrange
//            var ast = _generator.Generate("Result : {{ mydict }}");
//
//            // Act
//            String result = new LiquidASTRenderer().Render(templateContext, ast);
//            // Act
//
//            // Assert
//            Assert.That(result, Is.EqualTo("Result : { \"test1\" : \"test element\" }"));
//
//
//        }

        [Test]
        public void It_Should_Render_A_Dictionary_Element()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();
            
            //var dict = new Dictionary<String, String> {{"test", "test element"}};
            var dict = new Dictionary<String, IExpressionConstant> { { "test1", new StringValue("test element") } };

            templateContext.Define("mydict", new DictionaryValue(dict) );

            // Arrange
            var ast = _generator.Generate("Result : {{ mydict.test1 }}");

            // Act
            String result = new LiquidASTRenderer().Render(templateContext, ast);

            // Assert
            Assert.That(result, Is.EqualTo("Result : test element"));

        }

        [Test]
        public void It_Should_Render_A_Nested_Dictionary_Element()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();

            //var dict = new Dictionary<String, String> {{"test", "test element"}};
            var dict3 = new Dictionary<String, IExpressionConstant> { { "test2", new StringValue("test element") } };
            var dict2 = new Dictionary<String, IExpressionConstant> { { "test1", new DictionaryValue(dict3) } };
            var dict1 = new Dictionary<String, IExpressionConstant> { { "test", new DictionaryValue(dict2) } };

            templateContext.Define("mydict", new DictionaryValue(dict1));

            // Arrange
            var ast = _generator.Generate("Result : {{ mydict.test.test1.test2 }}");

            // Act
            String result = new LiquidASTRenderer().Render(templateContext, ast);

            // Assert
            Assert.That(result, Is.EqualTo("Result : test element"));

        }

        [Test]
        public void It_Should_Return_Nothing_When_Nested_Dictionaries_Pipe_Into_Each_Other_But_Have_Missing_Key()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();

            //var dict = new Dictionary<String, String> {{"test", "test element"}};
            var dict3 = new Dictionary<String, IExpressionConstant> { { "test2", new StringValue("test element") } };
            var dict2 = new Dictionary<String, IExpressionConstant> { { "test1", new DictionaryValue(dict3) } };
            var dict1 = new Dictionary<String, IExpressionConstant> { { "test", new DictionaryValue(dict2) } };

            templateContext.Define("mydict", new DictionaryValue(dict1));

            // Arrange
            var ast = _generator.Generate("Result : {{ mydict.zzz.test1.test2 }}");

            // Act
            String result = new LiquidASTRenderer().Render(templateContext, ast);

            // Assert
            Assert.That(result, Is.EqualTo("Result : "));

        }

        [Test]
        public void It_Should_Render_An_Array_In_A_Dictionary()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();
            var list = new ArrayValue(new List<IExpressionConstant>{ new StringValue("aaa"), new NumericValue(123m) } );
            var dict = new Dictionary<String, IExpressionConstant> { { "test", list } };

            templateContext.Define("mydict", new DictionaryValue(dict));

            // Arrange
            var ast = _generator.Generate("Result : {{ mydict.test[0] }}");

            // Act
            String result = new LiquidASTRenderer().Render(templateContext, ast);
            // Act

            // Assert
            Assert.That(result, Is.EqualTo("Result : aaa"));

        }

        [Test]
        public void It_Should_Render_An_Array()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();
            var list = new ArrayValue(new List<IExpressionConstant> { new StringValue("aaa"), new NumericValue(123m) });
          
            templateContext.Define("myarray", list);

            // Arrange
            var ast = _generator.Generate("Result : {{ myarray }}");

            // Act
            String result = new LiquidASTRenderer().Render(templateContext, ast);
            // Act

            // Assert
            //Assert.That(result, Is.EqualTo("Result : [ \"aaa\", 123 ]"));
            Assert.That(result, Is.EqualTo("Result : aaa123.0"));

        }

        [Test]
        public void It_Should_Render_An_Array_Element()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();
            var list = new ArrayValue(new List<IExpressionConstant> { new StringValue("aaa"), new NumericValue(123m) });

            templateContext.Define("myarray", list);

            // Arrange
            var ast = _generator.Generate("Result : {{ myarray[1] }}");

            // Act
            String result = new LiquidASTRenderer().Render(templateContext, ast);
            // Act

            // Assert
            Assert.That(result, Is.EqualTo("Result : 123.0"));

        }

        [Test]
        public void It_Should_Render_An_Array_Element_From_A_Nested_Index()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();
            var indexlist = new ArrayValue(new List<IExpressionConstant> { new NumericValue(0), new NumericValue(1) });
            var list = new ArrayValue(new List<IExpressionConstant> { new StringValue("aaa"), new StringValue("bbb") });

            templateContext.Define("indexes", indexlist);
            templateContext.Define("myarray", list);

            // Arrange
            var ast = _generator.Generate("Result : {{ myarray[indexes[1]] }}");

            // Act
            String result = new LiquidASTRenderer().Render(templateContext, ast);
            // Act

            // Assert
            Assert.That(result, Is.EqualTo("Result : bbb"));

        }

        [Test]
        public void It_Should_Render_An_Array_Element_From_A_Variable_in_A_Nested_Element_In_a_Tag()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();
            var arrayofbool = new ArrayValue(new List<IExpressionConstant> { new BooleanValue(true), new BooleanValue(false) });
            var array1 = new ArrayValue(new List<IExpressionConstant> { arrayofbool });
            var array2 = new ArrayValue(new List<IExpressionConstant> { array1 });
            

            templateContext.Define("idx1", new NumericValue(0));
            templateContext.Define("idx2", new NumericValue(1));
            templateContext.Define("array1", array1);
            templateContext.Define("array2", array2);
            templateContext.Define("arrayofbool", arrayofbool);
            
            // Act
            var ast = _generator.Generate("Result : {% if array2[0][0][idx1] %}first is TRUE{% endif %}{%if array2[0][0][idx2] %}second is TRUE{% endif %}");
            String result = new LiquidASTRenderer().Render(templateContext, ast);
            Console.WriteLine(result);
            // Assert
            Assert.That(result, Is.EqualTo("Result : first is TRUE"));

        }

        [Test]
        public void It_Should_Render_An_Array_Element_From_A_Crazy_Chain_of_Nested_Indexes()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();
            var arrayofnums = new ArrayValue(new List<IExpressionConstant> { new NumericValue(0), new NumericValue(1) });
            var array1 = new ArrayValue(new List<IExpressionConstant> { arrayofnums });
            var array2 = new ArrayValue(new List<IExpressionConstant> { array1 });
            var arrayofstr = new ArrayValue(new List<IExpressionConstant> { new StringValue("aaa"), new StringValue("bbb") });

            //templateContext.Define("arrayofnums", new NumericValue(1));
            templateContext.Define("arrayofnums", arrayofnums);
            templateContext.Define("array1", array1);
            templateContext.Define("array2", array2);
            templateContext.Define("arrayofstr", arrayofstr);

            // Arrange
            var ast = _generator.Generate("Result : {{ arrayofstr[array2[0][0][arrayofnums[arrayofnums[1]]]] }}");

            // Act
            String result = new LiquidASTRenderer().Render(templateContext, ast);
            // Act

            // Assert
            Assert.That(result, Is.EqualTo("Result : bbb"));

        }

        [Test]
        public void It_Should_Return_Empty_When_Invalid_Index()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();
            var arrayofnums = new ArrayValue(new List<IExpressionConstant> { new NumericValue(0), new NumericValue(1) });

            templateContext.Define("arrayofnums", arrayofnums);

            // Arrange
            var ast = _generator.Generate("Result : {{ arrayofnums[4] }}");

            // Act
            String result = new LiquidASTRenderer().Render(templateContext, ast);
            // Act

            // Assert
            Assert.That(result, Is.EqualTo("Result : "));

        }

        [Test]
        public void It_Should_Return_Empty_When_Index_Is_Empty()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();
            var arrayofnums = new ArrayValue(new List<IExpressionConstant> { new NumericValue(0), new NumericValue(1) });

            templateContext.Define("arrayofnums", arrayofnums);

            // Arrange
            var ast = _generator.Generate("Result : {{ arrayofnums[4][arrayofnums[4]] }}");

            // Act
            String result = new LiquidASTRenderer().Render(templateContext, ast);
            // Act

            // Assert
            Assert.That(result, Is.EqualTo("Result : "));

        }

        [Test]
        public void It_Should_Return_An_Error_If_A_Number_Is_Treated_Like_An_Array()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();
            var arrayofnums = new ArrayValue(new List<IExpressionConstant> { new NumericValue(0), new NumericValue(1) });

            templateContext.Define("arrayofnums", arrayofnums);
            templateContext.Define("numeric", new NumericValue(1));

            // Arrange
            var ast = _generator.Generate("Result : {{ arrayofnums[4][numeric[4]] }}");

            // Act
            String result = new LiquidASTRenderer().Render(templateContext, ast);
            // Act

            // Assert
            Assert.That(result, Is.StringContaining("Unable to dereference"));

        }

        [Test]
        public void It_Should_Not_Parse_Matching_Text_Off_Island()
        {
            // Arrange
            var templateContext = new TemplateContext();
            var ast = _generator.Generate("123 : {{ \"HELLO\" }}" );

            // Act
            String result = new LiquidASTRenderer().Render(templateContext, ast);
            Console.WriteLine("result is " + result);

            // Assert
            Assert.That(result, Is.EqualTo("123 : HELLO"));

        }

        [Test]
        public void It_Should_Allow_Tags_To_Span_Lines()
        {
            // Arrange
            var templateContext = new TemplateContext();
            IList<IExpressionConstant> objlist = new List<IExpressionConstant>
            {
                DataFixtures.CreateDictionary(1, "Title 1", "Value 1 B"), 
                DataFixtures.CreateDictionary(2, "Title 2", "Value 2 B"), 
                DataFixtures.CreateDictionary(3, "Title 3", "Value 3 B"), 
                DataFixtures.CreateDictionary(4, "Title 4", "Value 4 B"),
            };
            ArrayValue arrayValue = new ArrayValue(objlist);
            templateContext.Define("posts", arrayValue);
            const string tmpl = @"{%
               for post in posts %}{{
                     post.field1 }}{%
                endfor
              %}";

//            const string tmpl = @"{%
//  for post in posts %}{%
//    if post.title %}{{
//      post.title }}{%
//    endif %}{%
//  endfor
//%}";
            var ast = _generator.Generate(tmpl);

            // Act
            String result = new LiquidASTRenderer().Render(templateContext, ast);
            Console.WriteLine("result is " + result);

            // Assert
            Assert.That(result, Is.EqualTo("Title 1Title 2Title 3Title 4"));

        }

    }

}
