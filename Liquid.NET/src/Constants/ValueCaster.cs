using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using Liquid.NET.Filters;
using Liquid.NET.Utils;

namespace Liquid.NET.Constants
{
    public static class ValueCaster
    {
        public static LiquidExpressionResult Cast<TSource, TDest>(TSource src)
            where TDest : IExpressionConstant
            where TSource : IExpressionConstant
        {
            if (src == null)
            {
                return LiquidExpressionResult.Success(new None<IExpressionConstant>());
            }
            if (src is TDest)
            {
                //var result = (TDest) ((dynamic) src);                
                //IExpressionConstant success = (TDest)((dynamic)src);
                IExpressionConstant success = (TDest) (object) src;
                return LiquidExpressionResult.Success(new Some<IExpressionConstant>(success));
            }
            if (typeof (TDest) == typeof (StringValue))
            {
                //Console.WriteLine(src);
                return LiquidExpressionResult.Success(new StringValue(src.ToString()));
            }
            //return Convert<TDest>((dynamic)src);
            var str = src as StringValue;
            if (str != null)
            {
                return Convert<TDest>(str);
            }
            var num = src as NumericValue;
            if (num != null)
            {
                return Convert<TDest>(num);
            }
            var boo = src as BooleanValue;
            if (boo != null)
            {
                return Convert<TDest>(boo);
            }
            var dict = src as DictionaryValue;
            if (dict != null)
            {
                return Convert<TDest>(dict);
            }
            var arr = src as ArrayValue;
            if (arr != null)
            {
                return Convert<TDest>(arr);
            }
            var date = src as DateValue;
            if (date != null)
            {
                return Convert<TDest>(date);
            }
            return Convert<TDest>(src);
           
        }

        /// <summary>
        /// This applies the liquid casting rules, e.g. "null is zero when NumericValue" or 
        /// "null is empty string when StringValue".
        /// </summary>
        /// <typeparam name="TDest"></typeparam>
        /// <returns></returns>
        public static LiquidExpressionResult ConvertFromNull<TDest>()
            where TDest : IExpressionConstant
        {
            var destType = typeof(TDest);
            if (destType == typeof(NumericValue))
            {
                return LiquidExpressionResult.Success(new Some<IExpressionConstant>(NumericValue.Create(0)));
            }
            if (destType == typeof(StringValue))
            {
                return LiquidExpressionResult.Success(new Some<IExpressionConstant>(new StringValue(String.Empty)));
            }
            return LiquidExpressionResult.Success(new None<IExpressionConstant>());
        }

        private static LiquidExpressionResult Convert<TDest>(NumericValue num)
            where TDest : IExpressionConstant
        {
            var destType = typeof (TDest);
            if (destType == typeof (NumericValue))
            {
                return LiquidExpressionResult.Success(num);
            }

            return LiquidExpressionResult.Success(NumericValue.Create(0)); // Liquid seems to convert unknowns to numeric.
           
        }

        private static LiquidExpressionResult Convert<TDest>(BooleanValue boolean)
            where TDest : IExpressionConstant
        {
            var destType = typeof (TDest);
            if (destType == typeof (BooleanValue))
            {
                return LiquidExpressionResult.Success(boolean);
            }

            return LiquidExpressionResult.Error("Can't convert from boolean to " + destType);
       
        }

        // ReSharper disable once UnusedParameter.Local
        private static LiquidExpressionResult Convert<TDest>(DateValue date)
             where TDest : IExpressionConstant
        {
            var destType = typeof(TDest);

            return LiquidExpressionResult.Error("Can't convert from date to " + destType);

        }

        private static LiquidExpressionResult Convert<TDest>(DictionaryValue dictionaryValue)
           where TDest : IExpressionConstant
        {
            var destType = typeof(TDest);

            // So, according to https://github.com/Shopify/liquid/wiki/Liquid-for-Designers, a hash value will be iterated
            // as an array with two indices.
            if (destType == typeof (ArrayValue))
            {
                var dictarray = dictionaryValue.DictValue.Keys.Select(k =>
                {
                    var list = new List<Option<IExpressionConstant>> { new Some<IExpressionConstant>(new StringValue(k)), dictionaryValue.DictValue[k] };
                    return (Option<IExpressionConstant>) new Some<IExpressionConstant>(new ArrayValue(list));
                }).ToList();
                return LiquidExpressionResult.Success(new ArrayValue(dictarray));
            }
            // TODO: Should this return the default value for whatever TDest is requested?
            return LiquidExpressionResult.Error("Can't convert from a DictionaryValue to " + destType);
        }

        // ReSharper disable once UnusedParameter.Local
        private static LiquidExpressionResult Convert<TDest>(ArrayValue arrayValue)
              where TDest : IExpressionConstant
        {
            //Console.WriteLine("Rendering array");
            var destType = typeof(TDest);

            // TODO: Should this return the default value for whatever TDest is requested?
            return LiquidExpressionResult.Error("Can't convert from an ArrayValue to " + destType);
        }

        

        private static LiquidExpressionResult Convert<TDest>(StringValue str)
            where TDest : IExpressionConstant
        {
            var destType = typeof (TDest);
            if (destType == typeof (StringValue))
            {
                return LiquidExpressionResult.Success(str);
            }

            if (destType == typeof (NumericValue))
            {
                try
                {
                    var stringVal = str.StringVal;
                    if (stringVal == null)
                    {
                        return LiquidExpressionResult.Success(NumericValue.Create(0));  // liquid to_numeric seems to convert these to 0.
                    }
                    if (stringVal.Contains("."))
                    {
                        var val = ToDecimalCultureInvariant(stringVal);

                        return LiquidExpressionResult.Success(NumericValue.Create(val));
                    }
                    else
                    {
                        try
                        {
                            var val = int.Parse(stringVal);
                            return LiquidExpressionResult.Success(NumericValue.Create(val));
                        }
                        catch (OverflowException)
                        {
                            var val = ToDecimalCultureInvariant(stringVal);

                            return LiquidExpressionResult.Success(NumericValue.Create(val));
                        }
                    }
                   
                    
                }
                catch
                {
                    // https://github.com/Shopify/liquid/blob/master/lib/liquid/standardfilters.rb
                    return LiquidExpressionResult.Success(NumericValue.Create(0));  // liquid to_numeric seems to convert these to 0.
                }
            }

            if (destType == typeof (ArrayValue))
            {
                var expressionConstants = new Some<IExpressionConstant>(str);
                // IN liquid, it doesn't seem to cast a string to an array of chars---it casts to an array of one element.
                //var expressionConstants = str.StringVal.Select(x => (Option<IExpressionConstant>) new Some<IExpressionConstant>(new StringValue(x.ToString())));
                return LiquidExpressionResult.Success(new ArrayValue(new List<Option<IExpressionConstant>>{expressionConstants}));
            }
            return LiquidExpressionResult.Error("Can't convert from string to " + destType);
           
        }

        private static decimal ToDecimalCultureInvariant(string stringVal)
        {
            return decimal.Parse(stringVal, NumberStyles.Any, CultureInfo.InvariantCulture);
        }


        private static LiquidExpressionResult Convert<TDest>(IExpressionConstant source)
            where TDest : IExpressionConstant
        {
            var destType = typeof (TDest);
            if (destType == typeof (StringValue))
            {
                return LiquidExpressionResult.Success(new StringValue(source.ToString()));
            }
            if (destType == typeof (NumericValue))
            {
                return LiquidExpressionResult.Success(NumericValue.Create(0));
            }
            return LiquidExpressionResult.Error("Can't convert from " + source.GetType() + " to " + destType);

        }

        /// <summary>
        /// Use instead of Convert.ToInt32.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int ConvertToInt(decimal val)
        {
            return (int) Math.Round(val, MidpointRounding.AwayFromZero);
        }

        public static BigInteger ConvertToBigInt(decimal val)
        {
            return new BigInteger(Math.Round(val, MidpointRounding.AwayFromZero));
        }

        // Not sure where to put these yet
        /// <summary>
        /// Make a list of functions, each of which has the input of the previous function.  Interpolate a casting
        /// function if the input of one doesn't fit with the value of the next.
        /// 
        /// TODO: I think this should be part of the bind function.
        /// </summary>
        /// <param name="filterExpressions"></param>
        /// <returns></returns>
        public static IEnumerable<IFilterExpression> InterpolateCastFilters(IEnumerable<IFilterExpression> filterExpressions)
        {

            var result = new List<IFilterExpression>();

            Type expectedInputType = null;
            foreach (var filterExpression in filterExpressions)
            {
                // TODO: The expectedInputType might be a superclass of the output (not just equal type)
                //if (expectedInputType != null && filterExpression.SourceType != expectedInputType)
                if (expectedInputType != null && !filterExpression.SourceType.IsAssignableFrom(expectedInputType))
                {
                    //Console.WriteLine("Creating cast from " + filterExpression + " TO " + expectedInputType);
                    result.Add(CreateCastFilter(expectedInputType, filterExpression.SourceType));
                }
                result.Add(filterExpression);
                expectedInputType = filterExpression.ResultType;
            }

            return result;
        }

        private static IFilterExpression CreateCastFilter(Type sourceType, Type resultType)
        {
            // TODO: Move this to FilterFactory.Instantiate
            Type genericClass = typeof(CastFilter<,>);
            // MakeGenericType is badly named
            //Console.WriteLine("FilterChain Creating Converter from " + sourceType + " to " + resultType);
            Type constructedClass = genericClass.MakeGenericType(sourceType, resultType);
            return (IFilterExpression)Activator.CreateInstance(constructedClass);
        }

        public static string RenderAsString(Option<IExpressionConstant> val)
        {

            return val.HasValue ? RenderAsString(val.Value) : "";

//            // TODO: Does this render an error if it can't cast or an empty string?
//            if (stringResult.IsError)
//            {
//                return "";
//            }
//            return stringResult.SuccessResult.HasValue ? ((StringValue)stringResult.SuccessResult.Value).StringVal : "";
        }

        public static string RenderAsString(IExpressionConstant val)
        {

            if (val == null)
            {
                return "";
            }
            return val.ToString();
//            var stringResult = Cast<IExpressionConstant, StringValue>(val);
//
//            if (stringResult.IsError)
//            {
//                return "";
//            }
//            return stringResult.SuccessResult.HasValue ? ((StringValue)stringResult.SuccessResult.Value).StringVal : "";
        }

    }
}
