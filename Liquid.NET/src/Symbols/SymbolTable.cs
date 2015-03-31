using System;
using System.Collections.Generic;

using Liquid.NET.Constants;
using Liquid.NET.Filters;

namespace Liquid.NET.Symbols
{
    public class SymbolTable
    {
        private readonly IDictionary<String, IExpressionConstant> _variableDictionary;

        private readonly FilterRegistry _filterRegistry;

        public SymbolTable(
            IDictionary<String, IExpressionConstant> variableDictionary = null,
            FilterRegistry filterRegistry = null)
        {
            _variableDictionary = variableDictionary ?? new Dictionary<string, IExpressionConstant>();
            _filterRegistry = filterRegistry ?? new FilterRegistry();
        }

        public void DefineFilter<T>(String name)
            where T: IFilterExpression
        {
            _filterRegistry.Register<T>(name);
        }

        public Type ReferenceFilter(String key)
        {
            return _filterRegistry.Find(key);
        }
        public bool HasFilterReference(string filterName)
        {
            return _filterRegistry.HasKey(filterName);
        }

        public void DefineVariable(String key, IExpressionConstant obj)
        {
            if (_variableDictionary.ContainsKey(key))
            {
                _variableDictionary[key] = obj;
            }
            else
            {
                _variableDictionary.Add(key, obj);
            }
        }



        public bool HasVariableReference(String key)
        {
            return _variableDictionary.ContainsKey(key);
        }

        public IExpressionConstant ReferenceVariable(String key)
        {

            if (HasVariableReference(key))
            {
                return _variableDictionary[key];
            }
            else
            {
                // TODO: REturn undefined reference
                //return new Undefined();
                return ConstantFactory.CreateError<StringValue>("Undefined variable: " + key);
            }
        }

    }
}
