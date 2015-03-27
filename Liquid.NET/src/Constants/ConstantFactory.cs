using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.NET.Constants
{
    public static class ConstantFactory
    {
        public static T CreateError<T>(String errorMessage)
            where T : IExpressionConstant
        {
            //Console.WriteLine("Creating error "+ errorMessage);
            var error = (T) Activator.CreateInstance(typeof(T), default(T));
            error.ErrorMessage = errorMessage;
            return error;
        }

        public static T CreateUndefined<T>(String errorMessage)
            where T : IExpressionConstant
        {
            Console.WriteLine("Creating undefined " +typeof(T) + ": "+ errorMessage);
            T result;
            if (!typeof (T).GetConstructors().Any())
            {                
                result =  (T)Activator.CreateInstance(typeof(StringValue), default(String));
            }
            else
            {
                result = (T) Activator.CreateInstance(typeof (T), default(T)); // is this correct??
            }
            result.IsUndefined = true;
            return result;

        }

       

        public static TOut CreateUndefined<TIn, TOut>(Func<TIn, TOut> f, String errorMessage)
            where TOut : IExpressionConstant
        {

            return CreateUndefined<TOut>(errorMessage);
        }

        public static Type GetReturnType<TIn,TOut>(Func<TIn, TOut> f)
        {
            var t = f.Method.ReturnType;
            return t;
        }
    }
}
