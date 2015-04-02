using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;

namespace Liquid.NET
{
    public class LiquidErrorListener: BaseErrorListener
    {
        public event OnParsingErrorEventHandler ParsingErrorEventHandler;

        public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg,
            RecognitionException e)
        {
            base.SyntaxError(recognizer, offendingSymbol, line, charPositionInLine, msg, e);
            
            InvokeErrorEvent(new LiquidError
            {
                Message = msg, 
                Line = line, 
                CharPositionInLine = charPositionInLine,
                OffendingSymbol = offendingSymbol.Text
            });

        }



        public void InvokeErrorEvent(LiquidError liquidError)
        {
            OnParsingErrorEventHandler handler = ParsingErrorEventHandler;
            if (handler != null)
            {
                handler(liquidError);
            }
        }

        public void ClearEvents()
        {
            if (ParsingErrorEventHandler != null)
            {
                foreach (var handler in ParsingErrorEventHandler.GetInvocationList().Cast<OnParsingErrorEventHandler>())
                {
                    ParsingErrorEventHandler -= handler;
                }
            }
        }


    }
}
