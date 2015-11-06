using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Liquid.NET.Tests.Ruby
{
    public static class DictionaryFactory
    {
        public static ILiquidValue CreateArrayFromJson(String json)
        {
            var result = JsonConvert.DeserializeObject<JArray>(json);
            //var result = JsonConvert.DeserializeObject(json);
            //return TransformRoots((dynamic) result);
            return Transform(result);
        }

        public static IList<Tuple<String, ILiquidValue>> CreateStringMapFromJson(String json)
        {
            if (String.IsNullOrEmpty(json))
            {
                return new List<Tuple<String, ILiquidValue>>();
            }
            var result = JsonConvert.DeserializeObject<JObject>(json);
            //var result = JsonConvert.DeserializeObject(json);
            //return TransformRoots((dynamic) result);
            return TransformRoots(result);
        }

//        public static IList<Tuple<String, ILiquidValue>> TransformRoots(JArray obj)
//        {
//            return Transform(obj);
//            //return obj.Properties().Select(p => new Tuple<String, ILiquidValue>(p.Name, Transform((dynamic)p.Value))).ToList();
//        }

        public static IList<Tuple<String,ILiquidValue>> TransformRoots(JObject obj)
        {
            return obj.Properties().Select(p => new Tuple<String, ILiquidValue>(p.Name, Transform((dynamic)p.Value))).ToList();            
        }

        public static ILiquidValue Transform(Object obj)
        {
            throw new Exception("Don't know how to transform a "+ obj.GetType()+  " yet:" + obj);
        }

        public static ILiquidValue Transform(JArray arr)
        {
            var result = new LiquidCollection();
            var list = arr.Select(el => Transform((dynamic) el))
                .Cast<ILiquidValue>();
            foreach (var item in list)
            {
                result.Add(item == null ? Option<ILiquidValue>.None() : Option<ILiquidValue>.Create(item));
            }
            return result;
        }

        public static ILiquidValue Transform(JObject obj)
        {
            var result =new LiquidHash();
            var dict = obj.Properties().ToDictionary(k => k.Name, v => (ILiquidValue) Transform((dynamic)v.Value));
            foreach (var kvp in dict)
            {
                result.Add(kvp.Key,kvp.Value != null? 
                    (Option<ILiquidValue>) new Some<ILiquidValue>(kvp.Value) :
                    new None<ILiquidValue>());
            }
            return result;
        }

        public static ILiquidValue Transform(JValue obj)
        {
            // TODO: make this do something
            //var val = obj.Value;
            if (obj.Type.Equals(JTokenType.Integer)) 
            {
                return LiquidNumeric.Create(obj.ToObject<int>());
            } 
            else if (obj.Type.Equals(JTokenType.Float))
            {
                return LiquidNumeric.Create(obj.ToObject<decimal>());
            }
            else if (obj.Type.Equals(JTokenType.String))
            {
                return new LiquidString(obj.ToObject<String>());
            }
            else if (obj.Type.Equals(JTokenType.Boolean))
            {
                return new LiquidBoolean(obj.ToObject<bool>());
            }
            else if (obj.Type.Equals(JTokenType.Null))
            {
                //throw new ApplicationException("NULL Not implemented yet");
                return null; // TODO: Change this to an option
            }
            else
            {
                throw new Exception("Don't know how to transform a " +obj.GetType()+ ".");
            }
        }

    }
}
