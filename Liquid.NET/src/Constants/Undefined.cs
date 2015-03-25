using System;
using System.Collections.Generic;
using Liquid.NET.Expressions;
using Liquid.NET.Symbols;

namespace Liquid.NET.Constants
{
    /// <summary>
    /// TODO: Is this a type (as it is now?) or a property of a type (like an errormessage)?
    /// </summary>
    public class Undefined : ExpressionConstant
    {
        public static String CreateUndefinedMessage(String varname)
        {
            return "UNDEFINED: " + varname;
        }

        public string Name { get; private set; }

        public Undefined(String name)
        {
            Name = name;
        }

        public override IExpressionConstant Eval(SymbolTableStack symbolTableStack, IEnumerable<IExpressionConstant> childresults)
        {
            return this;
        }

        public override object Value
        {
            get { return CreateUndefinedMessage(Name); }
        }

        public override bool IsTrue
        {
            get { return false; }
        }

    }
}
