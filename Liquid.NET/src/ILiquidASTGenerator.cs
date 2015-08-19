using System;

namespace Liquid.NET
{
    public interface ILiquidASTGenerator
    {
        LiquidAST Generate(String template);
    }
}