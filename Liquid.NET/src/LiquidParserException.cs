using System;
using System.Collections.Generic;
using System.Linq;

namespace Liquid.NET
{
    public class LiquidParserException : Exception
    {
        private readonly IList<LiquidError> _liquidErrors;

        public override string Message
        {
            get
            {
                return String.Join("\r\n", _liquidErrors.Select(x => x.Message));
            }
        }

        public LiquidParserException(IList<LiquidError> liquidErrors)
        {
            _liquidErrors = liquidErrors;
        }

        public IList<LiquidError> LiquidErrors {
            get { return _liquidErrors; }
        }

    }
}