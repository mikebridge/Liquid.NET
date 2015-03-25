using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Symbols;

namespace Liquid.NET.Constants
{
    public class DictionaryValue : ExpressionConstant
    {
        private readonly IDictionary<String, IExpressionConstant> _value;

        public DictionaryValue(IDictionary<String, IExpressionConstant> dictionary)
        {
            _value = dictionary;
        }

        public override IExpressionConstant Eval(SymbolTableStack symbolTableStack, IEnumerable<IExpressionConstant> expressions)
        {
            return this;
        }

        public override object Value
        {
            get { return _value; }
        }

        public IDictionary<String, IExpressionConstant> DictValue { get { return _value; } }

        // TODO: not sure what this should do
        public override bool IsTrue
        {
            get { return _value.Keys.Any(); }
        }

        public IExpressionConstant ValueAt(String key)
        {
            Console.WriteLine("VALUE AT " + key);
            var result = _value.ContainsKey(key) ? _value[key] : new Undefined(key);
            Console.WriteLine("IS " + result);
            return result;
        }
    }
}