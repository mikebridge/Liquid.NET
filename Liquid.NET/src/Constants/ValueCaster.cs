using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Liquid.NET.Filters;

namespace Liquid.NET.Constants
{
    public static class ValueCaster
    {

        public static TDest Cast<TSource, TDest>(TSource src)
            where TDest : IExpressionConstant
            where TSource : IExpressionConstant
        {
            Console.WriteLine("BAH");
            //if (src.GetType().IsAssignableFrom(typeof(TDest))) {
            if (src is TDest)
            {
                //Console.WriteLine("Cast return returning "+src+" as a "+typeof(TDest));
                return (TDest) ((dynamic) src);
            }

            return Convert<TDest>((dynamic)src);
        }

        private static IExpressionConstant Convert<TDest>(NumericValue num)
            where TDest : IExpressionConstant
        {
            var destType = typeof (TDest);
            if (destType == typeof (NumericValue))
            {
                return num;
            }

            if (destType == typeof (StringValue))
            {
                return new StringValue(num.Value.ToString());
            }
            return new NumericValue(0); // Liquid seems to convert unknowns to numeric.
            //return ConstantFactory.CreateError<TDest>("Can't convert from numeric to " + destType);
        }

        private static IExpressionConstant Convert<TDest>(BooleanValue boolean)
            where TDest : IExpressionConstant
        {
            var destType = typeof (TDest);
            if (destType == typeof (BooleanValue))
            {
                return boolean;
            }

            if (destType == typeof (StringValue))
            {
                return new StringValue(boolean.Value.ToString().ToLower());
            }
            return ConstantFactory.CreateError<TDest>("Can't convert from boolean to " + destType);

        }

        private static IExpressionConstant Convert<TDest>(Undefined undef)
            where TDest : IExpressionConstant
        {
            var destType = typeof(TDest);
            if (destType == typeof(Undefined))
            {
                return undef;
            }

            if (destType == typeof(StringValue))
            {
                return new StringValue(undef.Value.ToString());
            }           
            // TODO: Should this return the default value for whatever TDest is requested?
            return ConstantFactory.CreateError<TDest>("Can't convert from an undefined ("+undef.Value+") to " + destType);
            //return ExpressionConstant.CreateError<TDest>("Can't convert from an undefined to " + destType);
        }

        private static IExpressionConstant Convert<TDest>(DictionaryValue dictionaryValue)
           where TDest : IExpressionConstant
        {
            //Console.WriteLine("Rendering dictionary");
            var destType = typeof(TDest);
            if (destType == typeof(Undefined))
            {
                return dictionaryValue;
            }

            if (destType == typeof(StringValue))
            {
                //Console.WriteLine("Converting dict to string");
//                foreach (var key in dictionaryValue.DictValue.Keys)
//                {
//                    Console.WriteLine("KEY " + key + "=" + dictionaryValue.DictValue[key]);
//                }

                return new StringValue(
                    dictionaryValue.DictValue
                        .Keys
                        .Aggregate("", (current, key) => current + FormatKvPair(key, dictionaryValue.DictValue[key])));
            }
            // So, according to https://github.com/Shopify/liquid/wiki/Liquid-for-Designers, a hash value will be iterated
            // as an array with two indices.
            if (destType == typeof (ArrayValue))
            {
                var dictarray = dictionaryValue.DictValue.Keys.Select(k =>
                {
                    var list = new List<IExpressionConstant> { new StringValue(k), dictionaryValue.DictValue[k] };
                    return (IExpressionConstant) new ArrayValue(list);

                }).ToList();
                return new ArrayValue(dictarray);
            }
            // TODO: Should this return the default value for whatever TDest is requested?
            return ConstantFactory.CreateError<TDest>("Can't convert from a DictionaryValue to " + destType);
        }

        private static IExpressionConstant Convert<TDest>(ArrayValue arrayValue)
              where TDest : IExpressionConstant
        {
            //Console.WriteLine("Rendering array");
            var destType = typeof(TDest);

            if (destType == typeof(StringValue))
            {
                //Console.WriteLine("Converting array to string");

                return new StringValue(FormatArray(arrayValue));
            }
            // TODO: Should this return the default value for whatever TDest is requested?
            return ConstantFactory.CreateError<TDest>("Can't convert from an ArrayValue to " + destType);
        }

        private static string FormatArray(ArrayValue arrayValue)
        {
            var strs = arrayValue.ArrValue.Select(x => Quote(x.GetType(), Convert<StringValue>((dynamic) x).Value.ToString()));
            return "[ " + String.Join(", ", strs) + " ]"; 

        }


        private static String FormatKvPair(string key, IExpressionConstant expressionConstant)
        {
            var strSymbol = Convert<StringValue>((dynamic) expressionConstant);
            
            return "{ " + Quote(typeof(StringValue), key) + " : " + Quote(expressionConstant.GetType(), (String) strSymbol.Value) + " }";
        }

        // TODO: quote JSON here
        private static String Quote(Type origType, String str)
        {
            if (origType.IsAssignableFrom(typeof (NumericValue)))
            {
                return str;
            }
            else
            {
                return "\"" + str + "\"";
            }
        }



        private static IExpressionConstant Convert<TDest>(StringValue str)
            where TDest : IExpressionConstant
        {
            var destType = typeof (TDest);
            if (destType == typeof (StringValue))
            {
                return str;
            }

            if (destType == typeof (NumericValue))
            {
                try
                {
                    return new NumericValue(decimal.Parse(str.StringVal));
                }
                catch
                {
                    // https://github.com/Shopify/liquid/blob/master/lib/liquid/standardfilters.rb
                    return new NumericValue(0);  // liquid to_numeric seems to convert these to 0.
                }
            }
            return ConstantFactory.CreateError<TDest>("Can't convert from string to " + destType);
           
        }

        

        // TODO: phase out Objectvalue.
        private static IExpressionConstant Convert<TDest>(ObjectValue source)
            where TDest : IExpressionConstant
        {
            var destType = typeof(TDest);
            if (destType == typeof(StringValue))
            {
                //return new StringValue(Stringify(source));
                return Convert<TDest>((dynamic) source.Value);
            }
            //source.
            return ConstantFactory.CreateError<TDest>("Can't convert from " + source.GetType() + " to " + destType);

        }


        private static IExpressionConstant Convert<TDest>(IExpressionConstant source)
            where TDest : IExpressionConstant
        {
            var destType = typeof (TDest);
            if (destType == typeof (StringValue))
            {
                return new StringValue(source.ToString());
            }
            if (destType == typeof (NumericValue))
            {
                return new NumericValue(0);
            }
            return ConstantFactory.CreateError<TDest>("Can't convert from " + source.GetType() + " to " + destType);

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

        public static IFilterExpression CreateCastFilter(Type sourceType, Type resultType)
        {
            // TODO: Move this to FilterFactory.Instantiate
            Type genericClass = typeof(CastFilter<,>);
            // MakeGenericType is badly named
            //Console.WriteLine("FilterChain Creating Converter from " + sourceType + " to " + resultType);
            Type constructedClass = genericClass.MakeGenericType(sourceType, resultType);
            return (IFilterExpression)Activator.CreateInstance(constructedClass);
        }


        public static string RenderAsString(IExpressionConstant val)
        {
            var stringResult = Cast<IExpressionConstant, StringValue>(val);

            if (stringResult.StringVal == null)
            {
                return "";
            }
            else
            {
                return stringResult.StringVal;
            }
        }

    }
}
