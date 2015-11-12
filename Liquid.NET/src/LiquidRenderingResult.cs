using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.NET
{
    public class LiquidRenderingResult
    {
        private Action<LiquidError> _onParsingError;
        private Action<LiquidError> _onRenderingError;
        private Action<LiquidError> _onAnyError = err => { };

        private Action<LiquidError> AnyError { get { return _onAnyError;} }

        public static LiquidRenderingResult Create(
            String result, 
            IList<LiquidError> renderingErrors,
            IList<LiquidError> parsingErrors
            )
        {
            return new LiquidRenderingResult
            {
                Result = result,
                RenderingErrors = renderingErrors,
                ParsingErrors = parsingErrors,
            };
            
        }


        private LiquidRenderingResult()
        {
            _onParsingError = AnyError;
            _onRenderingError = AnyError;
        }

        public String Result { get; private set; }

        public IList<LiquidError> ParsingErrors { get; private set; }

        public IList<LiquidError> RenderingErrors { get; set; }

        public bool HasParsingErrors
        {
            get { return ParsingErrors.Any(); }
        }

        public bool HasRenderingErrors
        {
            get { return RenderingErrors.Any(); }
        }

        public LiquidRenderingResult OnParsingError(Action<LiquidError> onErrorFn)
        {
            EvalWithErrors(onErrorFn, ParsingErrors);
            return this;
        }

        public LiquidRenderingResult OnRenderingError(Action<LiquidError> onErrorFn)
        {
            EvalWithErrors(onErrorFn, RenderingErrors);
            return this;
        }

        public LiquidRenderingResult OnAnyError(Action<LiquidError> onErrorFn)
        {
            EvalWithErrors(onErrorFn, RenderingErrors);
            EvalWithErrors(onErrorFn, ParsingErrors);
            return this;
        }

        private void EvalWithErrors(Action<LiquidError> onErrorFn, IList<LiquidError> parsingErrors)
        {
            if (onErrorFn != null)
            {
                foreach (var err in parsingErrors)
                {
                    onErrorFn(err);
                }
            }
          
        }
    }
}
