using System;
using System.Collections.Generic;
using System.Linq;

namespace Liquid.NET
{
    public class LiquidASTGenerationResult
    {
        public static LiquidASTGenerationResult Create(LiquidAST ast, IList<LiquidError> errors)
        {
            return new LiquidASTGenerationResult
            {
                LiquidAST = ast,
                ParsingErrors = errors
            };
            
        }

        public bool HasParsingErrors
        {
            get { return ParsingErrors.Any(); }
        }

        private LiquidASTGenerationResult()
        {
            
        }

        public LiquidAST LiquidAST { get; private set; }

        public IList<LiquidError> ParsingErrors { get; private set; }


    }
}
