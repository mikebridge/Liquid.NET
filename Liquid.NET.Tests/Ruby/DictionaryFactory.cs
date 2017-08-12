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
        public static Option<ILiquidValue> CreateArrayFromJson(String json)
        {
            var result = JsonConvert.DeserializeObject<JArray>(json);
            //var result = JsonConvert.DeserializeObject(json);
            //return TransformRoots((dynamic) result);
            return Transform(result);
        }

        public static IList<Tuple<String, Option<ILiquidValue>>> CreateStringMapFromJson(String json)
        {
            if (String.IsNullOrEmpty(json))
            {
                return new List<Tuple<String, Option<ILiquidValue>>>();
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

        public static IList<Tuple<String, Option<ILiquidValue>>> TransformRoots(JObject obj)
        {
            // MB: Refactored to remove dynamic
            return obj.Properties().Select(p => new Tuple<String, Option<ILiquidValue>>(p.Name, Transform(p.Value))).ToList();
            
        }

        public static Option<ILiquidValue> Transform(JToken obj)
        {
            if (obj.Type.Equals(JTokenType.Array))
            {
                return Transform(obj as JArray);
            } else if (obj.Type.Equals(JTokenType.Object))
            {
                return Transform(obj as JObject);
            }
            else
            {
                return Transform(obj as JValue);
            }
            //throw new Exception("Don't know how to transform a "+ obj.GetType()+  " yet:" + obj);
        }

        public static Option<ILiquidValue> Transform(JArray arr)
        {
            var result = new LiquidCollection();
            // MB: Refactored to remove dynamic
            var list = arr.Select(el => (Option<ILiquidValue>)Transform(el));
            //                .Cast<Option<ILiquidValue>>();
            foreach (var item in list)
            {
                //result.Add(Option<ILiquidValue>.Create(item));
                result.Add(item);
            }
            return result;
        }

        public static Option<ILiquidValue> Transform(JObject obj)
        {
            var result =new LiquidHash();

            // MB: Refactored to remove dynamic
            var dict = obj.Properties().ToDictionary(k => k.Name, v => (Option<ILiquidValue>)Transform(v.Value));
            foreach (var kvp in dict)
            {
                result.Add(kvp);
//                result.Add(kvp.Key,kvp.Value != null? 
//                    (Option<ILiquidValue>) new Some<ILiquidValue>(kvp.Value) :
//                    new None<ILiquidValue>());
            }
            return result;
        }

        public static Option<ILiquidValue> Transform(JValue obj)
        {
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
                return LiquidString.Create(obj.ToObject<String>());
            }
            else if (obj.Type.Equals(JTokenType.Boolean))
            {
                return new LiquidBoolean(obj.ToObject<bool>());
            }
            else if (obj.Type.Equals(JTokenType.Null))
            {
                //throw new ApplicationException("NULL Not implemented yet");
                //return null; // TODO: Change this to an option
                return Option<ILiquidValue>.None();
            }
            else
            {
                throw new Exception("Don't know how to transform a " +obj.GetType()+ ".");
            }
        }

    }
}
