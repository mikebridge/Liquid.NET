using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;

namespace Liquid.NET
{
    // TODO: Merge with symbolTableStack
    public class TemplateContext
    {
        private readonly IDictionary<String, IExpressionConstant> _varDictionary = new Dictionary<string, IExpressionConstant>();

        public void Define(String name, IExpressionConstant constant)
        {
            if (_varDictionary.ContainsKey(name))
            {
                _varDictionary[name] = constant;
            }
            else
            {
                _varDictionary.Add(name, constant);
            }
        }

        public IExpressionConstant Reference(String name)
        {
            if (_varDictionary.ContainsKey(name))
            {
                return _varDictionary[name];
            }
            return new Undefined(name);
        }

    }
}
