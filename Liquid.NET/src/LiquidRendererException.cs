using System;
using System.Collections.Generic;

namespace Liquid.NET
{
    public class LiquidRendererException : Exception
    {
        private readonly IList<LiquidError> _liquidErrors;

        public LiquidRendererException(IList<LiquidError> liquidErrors)
        {
            _liquidErrors = liquidErrors;
        }

        public IList<LiquidError> LiquidErrors {
            get { return _liquidErrors; }
        }

    }
}