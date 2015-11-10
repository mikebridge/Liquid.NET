using System;
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
            var result = GenerateAndRender("Result : {{ "+literal+" }}");

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

            // Act
            String result = Render(ctx, ast);

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
            String result = GenerateAndRender("Result : {% if \"" + str1 + "\" == \"" + str2 +
                                    "\" %}equal{% else %}not equal{% endif %}");
           

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
            String result = GenerateAndRender("Result : {% if " + str1 + " " + op + " " + str2 + " %}TRUE{% else %}FALSE{% endif %}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : " + expected));

        }

        [Test]
        public void It_Should_Resolve_A_Variable_In_An_Expression()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();
            templateContext.DefineLocalVariable("myVar", LiquidString.Create("My Variable"));
            //var ast = _generator.Generate("Result : {% if myVar %}OK{% else %}NOT OK{% endif %}");
            String result = GenerateAndRender("Result : {% if myVar %}OK{% else %}NOT OK{% endif %}", templateContext);
            // Act
            //String result = Render(templateContext, ast);

            // Assert
            Assert.That(result, Is.EqualTo("Result : OK"));

        }

        [Test]
        public void It_Should_Eval_An_Undefine_Variable_In_An_IfExpression_As_False()
        {
            // Arrange
            String result = GenerateAndRender("Result : {% if myUndefinedVar %}OK{% else %}NOT OK{% endif %}");
            
            // Assert
            Assert.That(result, Is.EqualTo("Result : NOT OK"));

        }

        [Test]
        public void It_Should_Render_A_Defined_Variable_In_A_LiquidExpression()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();
            const string val = "My Variable";
            templateContext.DefineLocalVariable("myVar", LiquidString.Create(val));
            String result = GenerateAndRender("Result : {{ myVar }}", templateContext);
 
            // Assert
            Assert.That(result, Is.EqualTo("Result : " + val));

        }

   
        [Test]
        public void It_Should_Not_Render_An_Undefined_Variable_In_A_LiquidExpression()
        {
            // Arrange
            String result = GenerateAndRender("Result : {{ myUndefinedVar }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : "));

        }

        [Test]
        [TestCase("true and false", "FALSE")]
        [TestCase("true and (false or true)", "TRUE")]
        [TestCase("(true and false) or false", "FALSE")]
        public void It_Should_Group_Expressions(String expr, String expected)
        {
            // Arrange
            String result = GenerateAndRender("Result : {% if " + expr + " %}TRUE{% else %}FALSE{% endif %}");


            Logger.Log("result is " + result);
            // Assert
            Assert.That(result, Is.EqualTo("Result : " + expected));

        }

        [Test]
        public void It_Should_Evaluate_A_Filter_Inside_A_Tag()
        {
            // Arrange
            var templateContext = new TemplateContext().WithAllFilters();
            templateContext.DefineLocalVariable("myvar", LiquidString.Create("hello"));
            String result = GenerateAndRender("Result : {% if myvar | upcase == \"HELLO\" %}TRUE{% else %}FALSE{% endif %}", templateContext);

            // Act
            Logger.Log("result is " + result);

            // Assert
            Assert.That(result, Is.EqualTo("Result : TRUE"));

        }

        [Test]
        public void It_Should_Evaluate_An_Index_Inside_A_Tag()
        {
            // Arrange
            var templateContext = new TemplateContext().WithAllFilters();
            templateContext.DefineLocalVariable("myvar", new LiquidCollection{LiquidString.Create("hello")});
            String result = GenerateAndRender("Result : {% if myvar[0] | upcase == \"HELLO\" %}TRUE{% else %}FALSE{% endif %}", templateContext);

            // Act
            Logger.Log("result is " + result);

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
            
            templateContext.DefineLocalVariable("mydict", new LiquidHash{ { "test1", LiquidString.Create("test element") } } );

            // Arrange
            String result = GenerateAndRender("Result : {{ mydict.test1 }}", templateContext);

            // Assert
            Assert.That(result, Is.EqualTo("Result : test element"));

        }

        [Test]
        public void It_Should_Render_A_Nested_Dictionary_Element()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();
            
            var dict3 = new LiquidHash { { "test2", LiquidString.Create("test element") } };
            var dict2 = new LiquidHash { { "test1", dict3 } };
            var dict1 = new LiquidHash{ { "test", dict2 } };

            templateContext.DefineLocalVariable("mydict", dict1);

            // Act
            String result = GenerateAndRender("Result : {{ mydict.test.test1.test2 }}", templateContext);

            // Assert
            Assert.That(result, Is.EqualTo("Result : test element"));

        }

        [Test]
        public void It_Should_Return_Nothing_When_Nested_Dictionaries_Pipe_Into_Each_Other_But_Have_Missing_Key()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();

            //var dict = new Dictionary<String, String> {{"test", "test element"}};
            var dict3 = new LiquidHash { { "test2", LiquidString.Create("test element") } };
            var dict2 = new LiquidHash { { "test1", dict3 } };
            var dict1 = new LiquidHash { { "test", dict2 } };

            templateContext.DefineLocalVariable("mydict",dict1);

            // Arrange
            String result = GenerateAndRender("Result : {{ mydict.zzz.test1.test2 }}", templateContext);

            // Assert
            Assert.That(result, Is.EqualTo("Result : "));

        }

        [Test]
        public void It_Should_Render_An_Array_In_A_Dictionary()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();
            var list = new LiquidCollection{ LiquidString.Create("aaa"), LiquidNumeric.Create(123m) };
            var dict = new LiquidHash { { "test", list } };

            templateContext.DefineLocalVariable("mydict", dict);

            // Arrange
            String result = GenerateAndRender("Result : {{ mydict.test[0] }}", templateContext);

            // Act

            // Assert
            Assert.That(result, Is.EqualTo("Result : aaa"));

        }

        [Test]
        public void It_Should_Render_An_Array()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();
            var list = new LiquidCollection {LiquidString.Create("aaa"), LiquidNumeric.Create(123m)};         
            templateContext.DefineLocalVariable("myarray", list);

            // Arrange
            String result = GenerateAndRender("Result : {{ myarray }}", templateContext);

            // Assert
            //Assert.That(result, Is.EqualTo("Result : [ \"aaa\", 123 ]"));
            Assert.That(result, Is.EqualTo("Result : aaa123.0"));

        }

        [Test]
        public void It_Should_Render_An_Array_Element()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();
            var list = new LiquidCollection{ LiquidString.Create("aaa"), LiquidNumeric.Create(123m) };

            templateContext.DefineLocalVariable("myarray", list);

            // Act
            String result = GenerateAndRender("Result : {{ myarray[1] }}", templateContext);

            // Assert
            Assert.That(result, Is.EqualTo("Result : 123.0"));

        }

        [Test]
        public void It_Should_Render_An_Array_Element_From_A_Nested_Index()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();
            var indexlist = new LiquidCollection { LiquidNumeric.Create(0), LiquidNumeric.Create(1) };
            var list = new LiquidCollection { LiquidString.Create("aaa"), LiquidString.Create("bbb") };

            templateContext.DefineLocalVariable("indexes", indexlist);
            templateContext.DefineLocalVariable("myarray", list);

            // Act
            String result = GenerateAndRender("Result : {{ myarray[indexes[1]] }}", templateContext);

            // Assert
            Assert.That(result, Is.EqualTo("Result : bbb"));

        }

        [Test]
        public void It_Should_Render_An_Array_Element_From_A_Variable_in_A_Nested_Element_In_a_Tag()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();
            var arrayofbool = new LiquidCollection { new LiquidBoolean(true), new LiquidBoolean(false) };
            var array1 = new LiquidCollection{ arrayofbool };
            var array2 = new LiquidCollection{ array1 };
            

            templateContext.DefineLocalVariable("idx1", LiquidNumeric.Create(0));
            templateContext.DefineLocalVariable("idx2", LiquidNumeric.Create(1));
            templateContext.DefineLocalVariable("array1", array1);
            templateContext.DefineLocalVariable("array2", array2);
            templateContext.DefineLocalVariable("arrayofbool", arrayofbool);
            
            // Act
            String result = GenerateAndRender("Result : {% if array2[0][0][idx1] %}first is TRUE{% endif %}{%if array2[0][0][idx2] %}second is TRUE{% endif %}", templateContext);
            
            Logger.Log(result);
            // Assert
            Assert.That(result, Is.EqualTo("Result : first is TRUE"));

        }

        [Test]
        public void It_Should_Render_An_Array_Element_From_A_Crazy_Chain_of_Nested_Indexes()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();
            var arrayofnums = new LiquidCollection { LiquidNumeric.Create(0), LiquidNumeric.Create(1) };
            var array1 = new LiquidCollection { arrayofnums };
            var array2 = new LiquidCollection{ array1 };
            var arrayofstr = new LiquidCollection { LiquidString.Create("aaa"), LiquidString.Create("bbb") };

            //templateContext.Define("arrayofnums", new LiquidNumeric(1));
            templateContext.DefineLocalVariable("arrayofnums", arrayofnums);
            templateContext.DefineLocalVariable("array1", array1);
            templateContext.DefineLocalVariable("array2", array2);
            templateContext.DefineLocalVariable("arrayofstr", arrayofstr);

            // Act
            String result = GenerateAndRender("Result : {{ arrayofstr[array2[0][0][arrayofnums[arrayofnums[1]]]] }}", templateContext);

            // Assert
            Assert.That(result, Is.EqualTo("Result : bbb"));

        }

        [Test]
        public void It_Should_Return_Empty_When_Invalid_Index()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();
            var arrayofnums = new LiquidCollection { LiquidNumeric.Create(0), LiquidNumeric.Create(1) };

            templateContext.DefineLocalVariable("arrayofnums", arrayofnums);

            // Act
            String result = GenerateAndRender("Result : {{ arrayofnums[4] }}", templateContext);

            // Assert
            Assert.That(result, Is.EqualTo("Result : "));

        }

        [Test]
        public void It_Should_Return_Empty_When_Index_Is_Empty()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();
            var arrayofnums = new LiquidCollection { LiquidNumeric.Create(0), LiquidNumeric.Create(1) };

            templateContext.DefineLocalVariable("arrayofnums", arrayofnums);

            // Act
            String result = GenerateAndRender("Result : {{ arrayofnums[4][arrayofnums[4]] }}", templateContext);

            // Assert
            Assert.That(result, Is.EqualTo("Result : "));

        }

        [Test]
        public void It_Should_Return_An_Error_If_A_Number_Is_Treated_Like_An_Array()
        {
            // Arrange
            TemplateContext templateContext = new TemplateContext();
            var arrayofnums = new LiquidCollection{ LiquidNumeric.Create(0), LiquidNumeric.Create(1) };

            templateContext.DefineLocalVariable("arrayofnums", arrayofnums);
            templateContext.DefineLocalVariable("numeric", LiquidNumeric.Create(1));

            // Act
            String result = GenerateAndRender("Result : {{ arrayofnums[4][numeric[4]] }}", templateContext);

            // Assert
            Assert.That(result, Is.StringContaining("cannot apply an index to a numeric."));

        }

        private static string Render(ITemplateContext templateContext, LiquidAST liquidAst, Action<LiquidError> onError = null)
        {
            if (onError == null)
            {
                onError = error => { };
            }
            String result = "";
            var renderingVisitor = new RenderingVisitor(templateContext);
            renderingVisitor.StartWalking(liquidAst.RootNode, str => result += str);
            if (renderingVisitor.HasErrors)
            {
                foreach (var error in renderingVisitor.RenderingErrors)
                {
                    onError(error);
                }
                //throw new LiquidRendererException(renderingVisitor.RenderingErrors);
            }
            return result;
        }

        [Test]
        public void It_Should_Not_Parse_Matching_Text_Off_Island()
        {
            // Arrange
            String result = GenerateAndRender("123 : {{ \"HELLO\" }}");

            // Act
            Logger.Log("result is " + result);

            // Assert
            Assert.That(result, Is.EqualTo("123 : HELLO"));

        }

        [Test]
        public void It_Should_Allow_Tags_To_Span_Lines()
        {
            // Arrange
            var templateContext = new TemplateContext();
   
            LiquidCollection liquidCollection = new LiquidCollection{
                DataFixtures.CreateDictionary(1, "Title 1", "Value 1 B"), 
                DataFixtures.CreateDictionary(2, "Title 2", "Value 2 B"), 
                DataFixtures.CreateDictionary(3, "Title 3", "Value 3 B"), 
                DataFixtures.CreateDictionary(4, "Title 4", "Value 4 B"),
            };

            templateContext.DefineLocalVariable("posts", liquidCollection);
            const string tmpl = @"{%
               for post in posts %}{{
                     post.field1 }}{%
                endfor
              %}";

            String result = GenerateAndRender(tmpl, templateContext);

            // Act
            Logger.Log("result is " + result);

            // Assert
            Assert.That(result, Is.EqualTo("Title 1Title 2Title 3Title 4"));

        }
        private string GenerateAndRender(string template, ITemplateContext ctx = null)
        {
            ctx = ctx ?? new TemplateContext();
            var ast = _generator.Generate(template).LiquidAST;

            // Act
            String result = Render(ctx, ast);
            return result;
        }

    }

}
