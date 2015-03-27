using System;
using System.Collections.Generic;
using Liquid.NET.Expressions;
using Liquid.NET.Symbols;

namespace Liquid.NET.Constants
{
    public abstract class ExpressionConstant : ExpressionDescription, IExpressionConstant
    {
//        public ExpressionConstant()
//        {
//            IsUndefined=true;
//        }

        public abstract object Value
        {
            get;
        }

        public abstract bool IsTrue
        {
            get;
        }

        public bool IsUndefined { get; set; }

        public bool IsNil { get; set; }

        /// <summary>
        /// Monadic bind: don't execute the function if there's an error.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public IExpressionConstant Bind(Func<IExpressionConstant, IExpressionConstant> f)
        {
            //Console.WriteLine("Old Bind");
            if (HasError)
            {
                Console.WriteLine("Bind Sees an error: " + ErrorMessage);
            }
            if (IsUndefined || GetType() == typeof (Undefined))
            {
                //Console.WriteLine("Undefined---ignoring the rest.");
                //return this;
                return ConstantFactory.CreateUndefined(f, "Undefined field");
            }
            return HasError ? this : f(this);
        }

        /// <summary>
        /// Monadic bind: don't execute the function if there's an error.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public TOut Bind<TOut>(Func<IExpressionConstant, TOut> f)
            where TOut : IExpressionConstant
        {
            //Console.WriteLine("New Bind");
            if (HasError)
            {
                Console.WriteLine("Bind Sees an error: " + ErrorMessage);
            }
            if (IsUndefined || GetType() == typeof(Undefined))
            {
                Console.WriteLine("Undefined---ignoring the rest.");
                //return this;
                return ConstantFactory.CreateUndefined(f, "Undefined field");
            }
            return HasError ? ConstantFactory.CreateError<TOut>(this.ErrorMessage) : f(this);

        }

    }
}
