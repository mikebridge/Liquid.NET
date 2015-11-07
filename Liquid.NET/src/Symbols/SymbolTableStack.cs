using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Tags;
using Liquid.NET.Utils;

namespace Liquid.NET.Symbols
{
    public class SymbolTableStack
    {
        private readonly IList<SymbolTable> _symbolTables = new List<SymbolTable>();

        public void Push(SymbolTable symbolTable)
        {
            _symbolTables.Add(symbolTable);
        }

        public SymbolTable Pop()
        {
            var last = _symbolTables.Last();
             _symbolTables.RemoveAt(_symbolTables.Count-1);
            return last;
        }

        /// <summary>
        /// To ignore the current stack, set skiplevels=1.
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="skiplevels"></param>
        /// <returns></returns>
        public LiquidExpressionResult Reference(String reference, int skiplevels = 0)
        {
            for (int i = _symbolTables.Count - 1 - skiplevels; i >= 0; i--)
            {
                //Console.WriteLine("Looking up" + reference);
                if (_symbolTables[i].HasVariableReference(reference))
                {
                    return _symbolTables[i].ReferenceLocalVariable(reference);
                }
            }
            //return LiquidExpressionResult.Success(new None<ILiquidValue>());
            return LiquidExpressionResult.Error(SymbolTable.NotFoundError(reference));
        }


        public Object ReferenceLocalRegistryVariable(String reference, int skiplevels = 0)
        {
            for (int i = _symbolTables.Count - 1 - skiplevels; i >= 0; i--)
            {
                if (_symbolTables[i].HasLocalRegistryVariableReference(reference))
                {
                    return _symbolTables[i].ReferenceLocalRegistryVariable(reference);
                }
            }
            return LiquidExpressionResult.Success(new None<ILiquidValue>());
        }

        public void FindVariable(String reference, 
            Action<SymbolTable, Option<ILiquidValue>> ifFoundAction, 
            Action ifNotFoundAction)
        {

            for (int i = _symbolTables.Count - 1; i >= 0; i--) // iterate backwards from most-specific scope
            {
                //Console.WriteLine("Looking up" + reference);
                if (_symbolTables[i].HasVariableReference(reference))
                {
                    var liquidExpressionResult = _symbolTables[i].ReferenceLocalVariable(reference);
                    if (liquidExpressionResult.IsError)
                    {
                        ifNotFoundAction();
                    }
                    else
                    {
                        ifFoundAction(_symbolTables[i], liquidExpressionResult.SuccessResult);
                    }
                    return;
                }
            }

            ifNotFoundAction();
            
        }

        public void Define(string reference, Option<ILiquidValue> obj)
        {
            //Console.WriteLine("Adding " + reference + " to current scope");
            _symbolTables.Last().DefineLocalVariable(reference, obj);
        }

        public void DefineLocalRegistry(string reference, Object obj)
        {
            _symbolTables.Last().DefineLocalRegistryVariable(reference, obj);
        }

        public void DefineGlobal(string key, Option<ILiquidValue> obj)
        {
            _symbolTables[0].DefineLocalVariable(key, obj);
        }

        public bool HasFilter(String name)
        {
            for (var i = _symbolTables.Count - 1; i >= 0; i--)
            {
                if (_symbolTables[i].HasFilterReference(name))
                {
                    return true;
                }
            } 
            return false;
        }

        // TODO: Dry these out:

        public Type LookupFilterType(string filterName)
        {
            
            for (var i = _symbolTables.Count - 1; i >= 0; i--)
            {
                if (_symbolTables[i].HasFilterReference(filterName))
                {
                    return _symbolTables[i].ReferenceFilter(filterName);
                }
            }
            return null;
        }

        public Type LookupCustomTagRendererType(string tagName)
        {
            for (var i = _symbolTables.Count - 1; i >= 0; i--)
            {
                if (_symbolTables[i].HasCustomTagReference(tagName))
                {
                    return _symbolTables[i].ReferenceCustomTag(tagName);
                }
            }
            return null;
        }

        public Type LookupCustomBlockTagRendererType(string tagName)
        {
            for (var i = _symbolTables.Count - 1; i >= 0; i--)
            {
                if (_symbolTables[i].HasCustomBlockTagReference(tagName))
                {
                    return _symbolTables[i].ReferenceCustomBlockTag(tagName);
                }
            }
            return null;
        }

        public void DefineMacro(string name, MacroBlockTag macro)
        {
            _symbolTables.Last().DefineMacro(name, macro);
        }


        public MacroBlockTag LookupMacro(string tagName)
        {
            for (var i = _symbolTables.Count - 1; i >= 0; i--)
            {
                if (_symbolTables[i].HasMacro(tagName))
                {
                    return _symbolTables[i].ReferenceMacro(tagName);
                }
            }
            return null;
        }

    }
}
