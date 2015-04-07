using System;
using System.Collections.Generic;
using Liquid.NET.Expressions;
using Liquid.NET.Symbols;

namespace Liquid.NET.Constants
{
    /// <summary>
    /// TODO: Is this a type (as it is now?) or a property of a type (like an errormessage)?
    /// </summary>
    [Obsolete]
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
            IsUndefined = true;
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
