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
        public static IExpressionConstant CreateArrayFromJson(String json)
        {
            var result = JsonConvert.DeserializeObject<JArray>(json);
            //var result = JsonConvert.DeserializeObject(json);
            //return TransformRoots((dynamic) result);
            return Transform(result);
        }

        public static IList<Tuple<String, IExpressionConstant>> CreateStringMapFromJson(String json)
        {
            if (String.IsNullOrEmpty(json))
            {
                return new List<Tuple<String, IExpressionConstant>>();
            }
            var result = JsonConvert.DeserializeObject<JObject>(json);
            //var result = JsonConvert.DeserializeObject(json);
            //return TransformRoots((dynamic) result);
            return TransformRoots(result);
        }

//        public static IList<Tuple<String, IExpressionConstant>> TransformRoots(JArray obj)
//        {
//            return Transform(obj);
//            //return obj.Properties().Select(p => new Tuple<String, IExpressionConstant>(p.Name, Transform((dynamic)p.Value))).ToList();
//        }

        public static IList<Tuple<String,IExpressionConstant>> TransformRoots(JObject obj)
        {
            return obj.Properties().Select(p => new Tuple<String, IExpressionConstant>(p.Name, Transform((dynamic)p.Value))).ToList();            
        }

        public static IExpressionConstant Transform(Object obj)
        {
            throw new Exception("Don't know how to transform a "+ obj.GetType()+  " yet:" + obj);
        }

        public static IExpressionConstant Transform(JArray arr)
        {
            var result = new ArrayValue();
            var list = arr.Select(el => Transform((dynamic) el))
                .Cast<IExpressionConstant>();
            foreach (var item in list)
            {
                result.Add(item == null ? Option<IExpressionConstant>.None() : Option<IExpressionConstant>.Create(item));
            }
            return result;
        }

        public static IExpressionConstant Transform(JObject obj)
        {
            var result =new DictionaryValue();
            var dict = obj.Properties().ToDictionary(k => k.Name, v => (IExpressionConstant) Transform((dynamic)v.Value));
            foreach (var kvp in dict)
            {
                result.Add(kvp.Key,kvp.Value != null? 
                    (Option<IExpressionConstant>) new Some<IExpressionConstant>(kvp.Value) :
                    new None<IExpressionConstant>());
            }
            return result;
        }

        public static IExpressionConstant Transform(JValue obj)
        {
            // TODO: make this do something
            //var val = obj.Value;
            if (obj.Type.Equals(JTokenType.Integer)) 
            {
                return NumericValue.Create(obj.ToObject<int>());
            } 
            else if (obj.Type.Equals(JTokenType.Float))
            {
                return NumericValue.Create(obj.ToObject<decimal>());
            }
            else if (obj.Type.Equals(JTokenType.String))
            {
                return new StringValue(obj.ToObject<String>());
            }
            else if (obj.Type.Equals(JTokenType.Boolean))
            {
                return new BooleanValue(obj.ToObject<bool>());
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
