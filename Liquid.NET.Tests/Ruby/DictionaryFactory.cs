using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Liquid.NET.Constants;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework.Constraints;

namespace Liquid.NET.Tests.Ruby
{
    public static class ConstantFactory
    {
        public static IExpressionConstant CreateFromJson(String json)
        {
            var result = JsonConvert.DeserializeObject(json);
            return Transform((dynamic) result);
        }

        public static IExpressionConstant Transform(Object obj)
        {
            throw new Exception("Don't know how to transform this yet." + obj.GetType());
        }

        public static IExpressionConstant Transform(JArray arr)
        {
            var list = arr.Select(el => Transform((dynamic) el))
                .Cast<IExpressionConstant>().ToList();
            return new ArrayValue(list);
        }



        public static IExpressionConstant Transform(JValue obj)
        {
            // TODO: make this do something
            return new StringValue("TEST");
        }

    }
}
