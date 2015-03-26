using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Filters;

namespace Liquid.NET.Symbols
{
    public class SymbolTable
    {
        private readonly Dictionary<String, IExpressionConstant> _variables = new Dictionary<string, IExpressionConstant>();

        private readonly FilterRegistry _filterRegistry;

        public SymbolTable()
        {
            _filterRegistry = new FilterRegistry();
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
            if (_variables.ContainsKey(key))
            {
                _variables[key] = obj;
            }
            else
            {
                _variables.Add(key, obj);
            }
        }



        public bool HasVariableReference(String key)
        {
            return _variables.ContainsKey(key);
        }

        public IExpressionConstant ReferenceVariable(String key)
        {

            if (HasVariableReference(key))
            {
                return _variables[key];
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
