using System;
using System.Collections.Generic;
using System.Linq;

namespace Liquid.NET
{
    /// <summary>
    /// Communicate parsing exceptions that are created from 
    /// additional logic in the AST generator.  These should
    /// only appear internally.
    /// </summary>
    internal class LiquidParserException : Exception
    {
        private readonly IList<LiquidError> _liquidErrors;

        public override string Message
        {
            get
            {
                return String.Join("\r\n", _liquidErrors.Select(x => x.Message));
            }
        }

        internal LiquidParserException(IList<LiquidError> liquidErrors)
        {
            _liquidErrors = liquidErrors;
        }

        internal IList<LiquidError> LiquidErrors
        {
            get { return _liquidErrors; }
        }

    }
}