using System;
using System.Collections.Generic;
using System.Linq;

namespace Liquid.NET
{
    public class LiquidTemplate
    {
        
        private readonly LiquidAST _liquidAst;

        public static LiquidParsingResult Create(String template)
        {
            IList<LiquidError> errors = new List<LiquidError>();
            var liquidAst = new LiquidASTGenerator().Generate(template, errors.Add);
            //if (errors.Any())
            //{
//                //throw new LiquidParserException(errors);
            //}
            //else
            //{
                var result = new LiquidTemplate(liquidAst);
            //}
            return LiquidParsingResult.Create(result, errors);
        }

        public LiquidTemplate(LiquidAST liquidAst)
        {           
            _liquidAst = liquidAst;
        }

        public LiquidRenderingResult Render(ITemplateContext ctx)
        {
            IList<LiquidError> renderingErrors = new List<LiquidError>();
            IList<LiquidError> parsingErrors = new List<LiquidError>();
            return LiquidRenderingResult.Create(Render(ctx, onRenderingError: renderingErrors.Add, onParsingError: parsingErrors.Add),
                renderingErrors, parsingErrors);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="onRenderingError">A handler for rendering errors (e.g divide by zero)</param>
        /// <param name="onParsingError">A handler for parsing errors (e.g. an include with a syntax error)</param>
        /// <returns></returns>
        public String Render(ITemplateContext ctx, 
            Action<LiquidError> onRenderingError = null,
            Action<LiquidError> onParsingError = null)
        {
            onRenderingError = onRenderingError ?? (err => { });
            onParsingError = onParsingError ?? (err => { });

            var result = "";

            var renderingVisitor = new RenderingVisitor(ctx);

            renderingVisitor.RenderingErrorEventHandler += (sender, err) => onRenderingError(err);
            renderingVisitor.ParsingErrorEventHandler += err => onParsingError(err);

            renderingVisitor.StartWalking(
                _liquidAst.RootNode, 
                str => result += str);

            return result;
        }
    }
}
