using System;
using System.Collections.Generic;
using Liquid.NET.Expressions;
using Liquid.NET.Utils;

namespace Liquid.NET.Symbols
{
    public class FilterSymbol
    {

        public String Name { get; private set; }

        public String RawArgs { get; set; }

        //public readonly List<IExpressionDescription> Args = new List<IExpressionDescription>();
        public readonly IList<TreeNode<IExpressionDescription>> Args = new List<TreeNode<IExpressionDescription>>();

        public FilterSymbol(String name)
        {
            Name = name;
        }

        public void AddArg(TreeNode<IExpressionDescription> obj)
        {
            Args.Add(obj);
        }

    }
}
