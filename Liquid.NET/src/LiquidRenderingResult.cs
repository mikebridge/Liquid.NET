using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.NET
{
    public class LiquidRenderingResult
    {
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

    }
}
