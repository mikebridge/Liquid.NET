using System;
using System.Collections.Generic;
using Liquid.NET.Expressions;
using Liquid.NET.Symbols;

namespace Liquid.NET.Constants
{
    public abstract class ExpressionConstant : ExpressionDescription, IExpressionConstant
    {
        public abstract object Value
        {
            get;
        }

        public abstract bool IsTrue
        {
            get;
        }


        /// <summary>
        /// Monadic bind: don't execute the function if there's an error.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public IExpressionConstant Bind(Func<IExpressionConstant, IExpressionConstant> f)
        {
            if (HasError)
            {
                Console.WriteLine("Bind Sees an error: " + ErrorMessage);
            }
            if (GetType() == typeof (Undefined))
            {
                Console.WriteLine("Undefined---ignoring the rest.");
                return this;
            }
            return HasError ? this : f(this);
        }


        public static T CreateError<T>(String errorMessage)
            where T : IExpressionConstant
        {
            Console.WriteLine("Creating error "+ errorMessage);
            var error = (T) Activator.CreateInstance(typeof(T), default(T));
            error.ErrorMessage = errorMessage;
            return error;
        }
    }
}
