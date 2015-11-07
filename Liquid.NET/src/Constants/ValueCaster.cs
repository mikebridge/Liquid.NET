using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using Liquid.NET.Utils;

namespace Liquid.NET.Constants
{
    public static class ValueCaster
    {
        public static LiquidExpressionResult Cast<TSource, TDest>(TSource src)
            where TDest : ILiquidValue
            where TSource : ILiquidValue
        {
            if (src == null)
            {
                return LiquidExpressionResult.Success(new None<ILiquidValue>());
            }
            if (src is TDest)
            {
                //var result = (TDest) ((dynamic) src);                
                //ILiquidValue success = (TDest)((dynamic)src);
                ILiquidValue success = (TDest) (object) src;
                return LiquidExpressionResult.Success(new Some<ILiquidValue>(success));
            }
            if (typeof (TDest) == typeof (LiquidString))
            {
                return LiquidExpressionResult.Success(LiquidString.Create(src.ToString()));
            }
            var str = src as LiquidString;
            if (str != null)
            {
                return Convert<TDest>(str);
            }
            var num = src as LiquidNumeric;
            if (num != null)
            {
                return Convert<TDest>(num);
            }
            var boo = src as LiquidBoolean;
            if (boo != null)
            {
                return Convert<TDest>(boo);
            }
            var dict = src as LiquidHash;
            if (dict != null)
            {
                return Convert<TDest>(dict);
            }
            var arr = src as LiquidCollection;
            if (arr != null)
            {
                return Convert<TDest>(arr);
            }
            var date = src as LiquidDate;
            if (date != null)
            {
                return Convert<TDest>(date);
            }
            //return Convert<TDest>(src);
            throw new ApplicationException("Unknown type: "+ src.GetType());
        }

        /// <summary>
        /// This applies the liquid casting rules, e.g. "null is zero when LiquidNumeric" or 
        /// "null is empty string when LiquidString".
        /// </summary>
        /// <typeparam name="TDest"></typeparam>
        /// <returns></returns>
        public static LiquidExpressionResult ConvertFromNull<TDest>()
            where TDest : ILiquidValue
        {
            var destType = typeof(TDest);
            if (destType == typeof(LiquidNumeric))
            {
                return LiquidExpressionResult.Success(new Some<ILiquidValue>(LiquidNumeric.Create(0)));
            }
            if (destType == typeof(LiquidString))
            {
                return LiquidExpressionResult.Success(LiquidString.Create(String.Empty));
            }
            return LiquidExpressionResult.Success(new None<ILiquidValue>());
        }

        private static LiquidExpressionResult Convert<TDest>(LiquidNumeric num)
            where TDest : ILiquidValue
        {
            var destType = typeof (TDest);
            if (destType == typeof (LiquidDate))
            {
                var longVal = num.LongValue;
                var dateTime = new DateTime(longVal);
                return LiquidExpressionResult.Success(new LiquidDate(dateTime));
            }

            return LiquidExpressionResult.Success(LiquidNumeric.Create(0)); // Liquid seems to convert unknowns to numeric.
           
        }

        private static LiquidExpressionResult Convert<TDest>(LiquidBoolean liquidBoolean)
            where TDest : ILiquidValue
        {
            var destType = typeof (TDest);
//            if (destType == typeof (LiquidBoolean))
//            {
//                return LiquidExpressionResult.Success(boolean);
//            }

            return LiquidExpressionResult.Error("Can't convert from boolean to " + destType);
       
        }

        // ReSharper disable once UnusedParameter.Local
        private static LiquidExpressionResult Convert<TDest>(LiquidDate liquidDate)
             where TDest : ILiquidValue
        {
            var destType = typeof(TDest);
            if (destType == typeof(LiquidNumeric))
            {
                LiquidNumeric ticks;
                if (liquidDate == null || !liquidDate.DateTimeValue.HasValue)
                {
                    ticks = LiquidNumeric.Create(0L);
                }
                else
                {
                    ticks = LiquidNumeric.Create(liquidDate.DateTimeValue.Value.Ticks);
                }
                return LiquidExpressionResult.Success(ticks);
            }

            return LiquidExpressionResult.Error("Can't convert from date to " + destType);

        }

        private static LiquidExpressionResult Convert<TDest>(LiquidHash liquidHash)
           where TDest : ILiquidValue
        {
            var destType = typeof(TDest);

            // So, according to https://github.com/Shopify/liquid/wiki/Liquid-for-Designers, a hash value will be iterated
            // as an array with two indices.
            if (destType == typeof (LiquidCollection))
            {
                var newArray = new LiquidCollection();
                var dictarray = liquidHash.Keys.Select(
                    k => (Option<ILiquidValue>) new Some<ILiquidValue>(new LiquidCollection {
                            LiquidString.Create(k),
                            liquidHash[k]
                    })).ToList();
                foreach (var item in dictarray)
                {
                    newArray.Add(item);
                }
                return LiquidExpressionResult.Success(newArray);
            }
            // TODO: Should this return the default value for whatever TDest is requested?
            return LiquidExpressionResult.Error("Can't convert from a LiquidHash to " + destType);
        }

        // ReSharper disable once UnusedParameter.Local
        private static LiquidExpressionResult Convert<TDest>(LiquidCollection liquidCollection)
              where TDest : ILiquidValue
        {
            //Console.WriteLine("Rendering array");
            var destType = typeof(TDest);

            // TODO: Should this return the default value for whatever TDest is requested?
            return LiquidExpressionResult.Error("Can't convert from an LiquidCollection to " + destType);
        }

        

        private static LiquidExpressionResult Convert<TDest>(LiquidString str)
            where TDest : ILiquidValue
        {
            var destType = typeof (TDest);
//            if (destType == typeof (LiquidString))
//            {
//                return LiquidExpressionResult.Success(str);
//            }

            if (destType == typeof (LiquidNumeric))
            {
                try
                {
                    var stringVal = str.StringVal;
                    if (stringVal == null)
                    {
                        return LiquidExpressionResult.Success(LiquidNumeric.Create(0));  // liquid to_numeric seems to convert these to 0.
                    }
                    if (stringVal.Contains("."))
                    {
                        var val = ToDecimalCultureInvariant(stringVal);

                        return LiquidExpressionResult.Success(LiquidNumeric.Create(val));
                    }
                    else
                    {
                        try
                        {
                            var val = int.Parse(stringVal);
                            return LiquidExpressionResult.Success(LiquidNumeric.Create(val));
                        }
                        catch (OverflowException)
                        {
                            var val = ToDecimalCultureInvariant(stringVal);

                            return LiquidExpressionResult.Success(LiquidNumeric.Create(val));
                        }
                    }
                   
                    
                }
                catch
                {
                    // https://github.com/Shopify/liquid/blob/master/lib/liquid/standardfilters.rb
                    return LiquidExpressionResult.Success(LiquidNumeric.Create(0));  // liquid to_numeric seems to convert these to 0.
                }
            }

            if (destType == typeof (LiquidCollection))
            {
                var expressionConstants = new Some<ILiquidValue>(str);
                // IN liquid, it doesn't seem to cast a string to an array of chars---it casts to an array of one element.
                //var expressionConstants = str.StringVal.Select(x => (Option<ILiquidValue>) new Some<ILiquidValue>(LiquidString.Create(x.ToString())));
                return LiquidExpressionResult.Success(new LiquidCollection{expressionConstants});
            }
            return LiquidExpressionResult.Error("Can't convert from string to " + destType);
           
        }

        private static decimal ToDecimalCultureInvariant(string stringVal)
        {
            return decimal.Parse(stringVal, NumberStyles.Any, CultureInfo.InvariantCulture);
        }


//        private static LiquidExpressionResult Convert<TDest>(ILiquidValue source)
//            where TDest : ILiquidValue
//        {
//            var destType = typeof (TDest);
//            if (destType == typeof (LiquidString))
//            {
//                return LiquidExpressionResult.Success(LiquidString.Create(source.ToString()));
//            }
//            if (destType == typeof (LiquidNumeric))
//            {
//                return LiquidExpressionResult.Success(LiquidNumeric.Create(0));
//            }
//            return LiquidExpressionResult.Error("Can't convert from " + source.GetType() + " to " + destType);
//
//        }

        /// <summary>
        /// Use instead of Convert.ToInt32.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int ConvertToInt(decimal val)
        {
            return (int) Math.Round(val, MidpointRounding.AwayFromZero);
        }

        public static long ConvertToLong(decimal val)
        {
            return (long)Math.Round(val, MidpointRounding.AwayFromZero);
        }

        public static BigInteger ConvertToBigInt(decimal val)
        {
            return new BigInteger(Math.Round(val, MidpointRounding.AwayFromZero));
        }

        public static string RenderAsString(Option<ILiquidValue> val)
        {

            return val.HasValue ? RenderAsString(val.Value) : "";

        }

        public static string RenderAsString(ILiquidValue val)
        {

            if (val == null)
            {
                return "";
            }
            return val.ToString();

        }


    }
}
