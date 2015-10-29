using System;

namespace Liquid.NET.Constants
{
    [Obsolete]
    public static class ConstantFactory
    {
        /*
        public static T CreateErrorX<T>(String errorMessage)
            where T : IExpressionConstant
        {
            //Console.WriteLine("Creating error "+ errorMessage);
            var error = (T) Activator.CreateInstance(typeof(T), default(T));
            //error.ErrorMessage = errorMessage;
            return error;
        }

        public static T CreateNilValueOfTypeX<T>(String errorMessage)
            where T : IExpressionConstant
        {
            Console.WriteLine("Creating undefined " +typeof(T) + ": "+ errorMessage);
            T result;
            if (!typeof (T).GetConstructors().Any())
            {                
                //result =  (T)Activator.CreateInstance(typeof(StringValue), default(String));
                // TODO: Replace this with an option type
                //result = (T)Activator.CreateInstance(typeof(StringValue), null);
            }
            else
            {
                //result = (T) Activator.CreateInstance(typeof (T), default(T)); // is this correct??
                result = (T)Activator.CreateInstance(typeof(T), default(T)); // is this correct??
            }
            result.IsNil = true;
            return result;

        }

       

        public static TOut CreateNilValueOfTypeX<TIn, TOut>(Func<TIn, TOut> f, String errorMessage)
            where TOut : IExpressionConstant
        {

            return CreateNilValueOfType<TOut>(errorMessage);
        }

        public static Type GetReturnTypeX<TIn,TOut>(Func<TIn, TOut> f)
        {
            var t = f.Method.ReturnType;
            return t;
        }
         */
    }
}
