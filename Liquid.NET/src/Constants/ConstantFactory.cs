using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.NET.Constants
{
    public static class ConstantFactory
    {
        public static T CreateError<T>(String errorMessage)
            where T : IExpressionConstant
        {
            Console.WriteLine("Creating error "+ errorMessage);
            var error = (T) Activator.CreateInstance(typeof(T), default(T));
            error.ErrorMessage = errorMessage;
            return error;
        }

        public static T CreateUndefined<T>(String errorMessage)
            where T : IExpressionConstant
        {
            Console.WriteLine("Creating undefined " + errorMessage);
            var undefined = (T)Activator.CreateInstance(typeof(T), default(T));
            undefined.IsUndefined = true;
            return undefined;
        }

        public static TOut CreateUndefined<TIn, TOut>(Func<TIn, TOut> f, String errorMessage)
            where TOut : IExpressionConstant
        {
            Console.WriteLine("Creating undefined " + errorMessage);
            return CreateUndefined<TOut>(errorMessage);
            //undefined.IsUndefined = true;
            //return undefined;
        }

//        public static IExpressionConstant CreateUndefined(Type t, String errorMessage) 
//        {
//            Console.WriteLine("Creating undefined " + errorMessage);
//            var undefined = (IExpressionConstant) Activator.CreateInstance(t, GetDefault(t));
//            undefined.IsUndefined = true;
//            return undefined;
//        }
//
//        private  static object GetDefault(Type type)
//        {
//            if (type.IsValueType)
//            {
//                return Activator.CreateInstance(type);
//            }
//            return null;
//        }
        public static Type GetReturnType<TIn,TOut>(Func<TIn, TOut> f)
        {
            var t = f.Method.ReturnType;
            return t;
        }
    }
}
