using System;
using System.Collections.Generic;
using System.Linq;

namespace Liquid.NET
{
    public class LiquidParsingResult
    {
        public static LiquidParsingResult Create(LiquidAST ast, IList<LiquidError> errors)
        {
            return new LiquidParsingResult
            {
                LiquidAST = ast,
                ParsingErrors = errors
            };
            
        }

        public bool HasParsingErrors
        {
            get { return ParsingErrors.Any(); }
        }

        private LiquidParsingResult()
        {
            
        }

        public LiquidAST LiquidAST { get; private set; }

        public IList<LiquidError> ParsingErrors { get; private set; }


    }
}
