using System;
using System.Collections.Generic;

namespace Liquid.NET
{
    public class LiquidRendererExceptionOLD : Exception
    {
        private readonly IList<LiquidError> _liquidErrors;

        public LiquidRendererExceptionOLD(IList<LiquidError> liquidErrors)
        {
            _liquidErrors = liquidErrors;
        }

        public IList<LiquidError> LiquidErrors {
            get { return _liquidErrors; }
        }

    }
}