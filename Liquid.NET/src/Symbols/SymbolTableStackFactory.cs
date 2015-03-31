using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Filters;

namespace Liquid.NET.Symbols
{
    public class SymbolTableStackFactory
    {
        public static SymbolTableStack CreateSymbolTableStack(ITemplateContext templateContext)
        {
            var result = new SymbolTableStack();
          
            // TODO: Fix this cast (the interface was to shield the user from the dict and the registry...
            var globalScopeSymbolTable = new SymbolTable(
                ((TemplateContext) templateContext).VariableDictionary,
                ((TemplateContext) templateContext).FilterRegistry);

            // TODO: make this less prone to collision:
            globalScopeSymbolTable.DefineFilter<LookupFilter>("lookup");

            result.Push(globalScopeSymbolTable);

            return result;
        }
    }
}
