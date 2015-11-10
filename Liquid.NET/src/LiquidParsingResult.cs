using System.Collections.Generic;
using System.Linq;

namespace Liquid.NET
{
    public class LiquidParsingResult
    {
        public static LiquidParsingResult Create(LiquidTemplate liquidTemplate, IList<LiquidError> parsingErrors)
        {
            return new LiquidParsingResult
            {
                LiquidTemplate = liquidTemplate,
                ParsingErrors = parsingErrors
            };
            
        }

        public bool HasParsingErrors
        {
            get { return ParsingErrors.Any(); }
        }

        private LiquidParsingResult()
        {
            
        }

        public LiquidTemplate LiquidTemplate { get; private set; }

        public IList<LiquidError> ParsingErrors { get; private set; }


    }
}
