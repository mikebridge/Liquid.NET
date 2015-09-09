using System;
using System.Collections.Generic;
using Liquid.NET.Expressions;
using Liquid.NET.Utils;


using ExpressionResult = Liquid.NET.Utils.Either<Liquid.NET.LiquidError, Liquid.NET.Utils.Option<Liquid.NET.Constants.IExpressionConstant>>;

namespace Liquid.NET.Constants
{
    public abstract class ExpressionConstant : ExpressionDescription, IExpressionConstant
    {
        protected ExpressionConstant()
        {
            MetaData = new Dictionary<string, object>();
        }


        public abstract object Value
        {
            get;
        }

        /// <summary>
        /// This should correspond to this:
        /// https://docs.shopify.com/themes/liquid-documentation/basics/true-and-false
        /// </summary>
        public abstract bool IsTrue
        {
            get;
        }

        public abstract string LiquidTypeName { get; }

        /// <summary>
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public LiquidExpressionResult Bind(Func<IExpressionConstant, LiquidExpressionResult> f)
        {
            throw new Exception("NEED TO MOVE THIS");

            //            //Console.WriteLine("Old Bind");
//            if (HasError)
//            {
//                Console.WriteLine("Bind Sees an error: " + ErrorMessage);
//            }
//            //if (IsUndefined || GetType() == typeof (Undefined))
////            if (IsNil)
////            {
////                //Console.WriteLine("Undefined---ignoring the rest.");
////                //return this;
////                return ConstantFactory.CreateNilValueOfType(f, "Undefined field");
////            }
//            return HasError ? this : f(this);
        }

        /// <summary>
        /// Monadic bind: don't execute the function if there's an error.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public TOut Bind<TOut>(Func<LiquidExpressionResult, TOut> f)
            where TOut : LiquidExpressionResult
        {
            //return f(this);
            throw new Exception("NEED TO MOVE THIS");
            //Console.WriteLine("New Bind");
//            if (HasError)
//            {
//                Console.WriteLine("Bind Sees an error: " + ErrorMessage);
//            }
////            if (IsUndefined || GetType() == typeof(Undefined))
////            {
////                Console.WriteLine("Undefined---ignoring the rest.");
////                //return this;
////                return ConstantFactory.CreateNilValueOfType(f, "Undefined field");
////            }
//            return HasError ? ConstantFactory.CreateError<TOut>(this.ErrorMessage) : f(this);

        }

        public T ValueAs<T>()
        {
            return (T) Value;
        }

        public Option<IExpressionConstant> ToOption()
        {
            if (this.Value != null)
            {
                return new Some<IExpressionConstant>(this);
            }
            else
            {
                return new None<IExpressionConstant>();
            }
        }

        public IDictionary<string, object> MetaData { get; private set; }

        public static implicit operator Option<IExpressionConstant>(ExpressionConstant t)
        {
            if (t == null)
            {
                return new None<IExpressionConstant>();
            }
            else
            {
                return new Some<IExpressionConstant>(t);
            }
        }

        public override bool Equals(object otherObj)
        {
            if (otherObj == null)
            {
                return false;
            }
            else
            {
                var exprConstant = otherObj as ExpressionConstant;
                if (exprConstant != null)
                {
                    return exprConstant.Value.Equals(Value);
                }
                else
                {
                    return this.Value == null;   
                }
            }
        }


        public override int GetHashCode()
        {
            return Value == null ? 0 : Value.GetHashCode();
        }

        public override LiquidExpressionResult Eval(
            ITemplateContext templateContext,
            IEnumerable<Option<IExpressionConstant>> childresults)
        {
            return LiquidExpressionResult.Success(this);
        }

//        public override string ToString()
//        {
//            return ValueCaster.RenderAsString((IExpressionConstant)this);
//        }
    }
}
