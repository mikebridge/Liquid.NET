using System;
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

        /// <summary>
        /// This function will be called once for each error encountered
        /// during parsing
        /// </summary>
        /// <param name="onErrorFn"></param>
        /// <returns></returns>
        public LiquidParsingResult OnParsingError(Action<LiquidError> onErrorFn)
        {
            if (onErrorFn != null)
            {
                foreach (var err in ParsingErrors)
                {
                    onErrorFn(err);
                }
            }
            return this;
        }


    }
}
