using System;
using System.Collections.Generic;
using Liquid.NET.Expressions;

namespace Liquid.NET.Symbols
{
    public class FilterSymbol
    {

        public String Name { get; private set; }

        public String RawArgs { get; set; }

        public readonly List<IExpressionDescription> Args = new List<IExpressionDescription>();

        public FilterSymbol(String name)
        {
            Name = name;
        }

        public void AddArg(IExpressionDescription obj)
        {
            Args.Add(obj);
        }

    }
}
