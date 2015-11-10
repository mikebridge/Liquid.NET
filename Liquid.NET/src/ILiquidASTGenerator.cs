using System;

namespace Liquid.NET
{
    public interface ILiquidASTGenerator
    {
        LiquidAST Generate(String template, Action<LiquidError> onParserError);

        LiquidParsingResult Generate(String template);
    }
}