using System;
using System.Collections.Generic;

namespace Liquid.NET
{
    public class LiquidParserException : Exception
    {
        private readonly IList<LiquidError> _liquidErrors;

        public LiquidParserException(IList<LiquidError> liquidErrors)
        {
            _liquidErrors = liquidErrors;
        }

        public IList<LiquidError> LiquidErrors {
            get { return _liquidErrors; }
        }

    }
}