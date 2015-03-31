using System;
using System.Collections.Generic;
using System.Linq;

using Liquid.NET.Constants;
using Liquid.NET.Filters;

namespace Liquid.NET.Symbols
{
    public class SymbolTableStack
    {
        private readonly IList<SymbolTable> _symbolTables = new List<SymbolTable>();

        //public TemplateContext TemplateContext { get; private set; }

//        public SymbolTableStack()
//        {
//            //TemplateContext = templateContext;
//        }

        public void Push(SymbolTable symbolTable)
        {
            _symbolTables.Add(symbolTable);
        }

        public SymbolTable Pop()
        {
            var last = _symbolTables.Last();
             _symbolTables.RemoveAt(_symbolTables.Count()-1);
            return last;
        }

        public IExpressionConstant Reference(String reference)
        {
            for (int i = _symbolTables.Count()-1; i >= 0; i--)
            {
                Console.WriteLine("Looking up" + reference);
                if (_symbolTables[i].HasVariableReference(reference))
                {
                    return _symbolTables[i].ReferenceVariable(reference);
                }
            }
            // TODO: Can the template context be merged with a symbolstack?
            //var result = TemplateContext.Reference(reference);
            //if (result.GetType() != typeof (Undefined))
            //{
                //return result;
            //}
            
            return new Undefined(reference); 
        }

        public void Define(string reference, IExpressionConstant obj)
        {
            //Console.WriteLine("Adding " + reference + " to current scope");
            _symbolTables.Last().DefineVariable(reference, obj);
        }

        public void DefineGlobal(string key, IExpressionConstant obj)
        {
            _symbolTables[0].DefineVariable(key, obj);
        }

        public IFilterExpression CreateFilter(string filterName, IEnumerable<IExpressionConstant> args)
        {
            // TODO: make an iterator for this kind of thing
            var filterType = LookupFilter(filterName);

            //var filterType= _symbolTables.Last().ReferenceFilter(name);
            if (filterType == null)
            {
                //TODO: make this return an error filter or something?
                throw new Exception("Invalid filter: " + filterName);
            }
            return FilterFactory.InstantiateFilter(filterName, filterType, args);
        }

        public bool HasFilter(String name)
        {
            for (var i = _symbolTables.Count() - 1; i >= 0; i--)
            {
                if (_symbolTables[i].HasFilterReference(name))
                {
                    return true;
                }
            } 
            return false;
        }

        private Type LookupFilter(string filterName)
        {
            
            for (var i = _symbolTables.Count() - 1; i >= 0; i--)
            {
                if (_symbolTables[i].HasFilterReference(filterName))
                {
                    return _symbolTables[i].ReferenceFilter(filterName);
                }
            }
            return null;
        }
    }
}
