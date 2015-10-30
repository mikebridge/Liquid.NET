using System.Linq;

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
                TokenSource = offendingSymbol.TokenSource.SourceName,
                //Context = offendingSymbol..SourceName,
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

//        public void ClearEvents()
//        {
//            if (ParsingErrorEventHandler != null)
//            {
//                foreach (var handler in ParsingErrorEventHandler.GetInvocationList().Cast<OnParsingErrorEventHandler>())
//                {
//                    ParsingErrorEventHandler -= handler;
//                }
//            }
//        }


    }
}
