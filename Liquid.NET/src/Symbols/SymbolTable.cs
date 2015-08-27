using System;
using System.Collections.Generic;

using Liquid.NET.Constants;
using Liquid.NET.Filters;
using Liquid.NET.Tags;
using Liquid.NET.Tags.Custom;
using Liquid.NET.Utils;

namespace Liquid.NET.Symbols
{
    public class SymbolTable
    {
        private readonly FilterRegistry _filterRegistry;
        private readonly Registry<ICustomTagRenderer> _customTagRendererRegistry;
        private readonly Registry<ICustomBlockTagRenderer> _customBlockTagRendererRegistry;
        private readonly IDictionary<String, IExpressionConstant> _variableDictionary;
        private readonly IDictionary<String, Object> _localRegistry; // available to tags & filters only
        private readonly IDictionary<String, MacroBlockTag> _macroRegistry;

        public SymbolTable(
            IDictionary<string, IExpressionConstant> variableDictionary = null, 
            FilterRegistry filterRegistry = null, 
            Registry<ICustomTagRenderer> customTagRendererRegistry = null,
            Registry<ICustomBlockTagRenderer> customBlockTagRendererRegistry = null,
            IDictionary<String, Object> localRegistry = null)
        {
            _customBlockTagRendererRegistry = customBlockTagRendererRegistry ?? new Registry<ICustomBlockTagRenderer>();
            _customTagRendererRegistry = customTagRendererRegistry ?? new Registry<ICustomTagRenderer>();
            _variableDictionary = variableDictionary ?? new Dictionary<string, IExpressionConstant>();
            _localRegistry = localRegistry ?? new Dictionary<string, Object>();
            _filterRegistry = filterRegistry ?? new FilterRegistry();
            _macroRegistry = new Dictionary<string, MacroBlockTag>();
        }

        public void DefineFilter<T>(String name)
            where T: IFilterExpression
        {
            _filterRegistry.Register<T>(name);
        }

        public void DefineCustomTag<T>(String name)
            where T:ICustomTagRenderer
        {
            _customTagRendererRegistry.Register<T>(name);
        }

        public void DefineCustomBlockTag<T>(String name)
            where T : ICustomBlockTagRenderer
        {
            _customBlockTagRendererRegistry.Register<T>(name);
        }

        public void DefineLocalVariable(String key, IExpressionConstant obj)
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

        public void DefineLocalRegistryVariable(String key, Object obj)
        {
            if (_localRegistry.ContainsKey(key))
            {
                _localRegistry[key] = obj;
            }
            else
            {
                _localRegistry.Add(key, obj);
            }
        }

        public void DefineMacro(String key, MacroBlockTag macro)
        {
            if (_macroRegistry.ContainsKey(key))
            {
                _macroRegistry[key] = macro;
            }
            else
            {
                _macroRegistry.Add(key, macro);
            }
        }

        public bool HasMacro(string filterName)
        {
            return _macroRegistry.ContainsKey(filterName);
        }

        public bool HasFilterReference(string filterName)
        {
            return _filterRegistry.HasKey(filterName);
        }

        public bool HasCustomTagReference(string tagName)
        {
            return _customTagRendererRegistry.HasKey(tagName);
        }

        public bool HasCustomBlockTagReference(string tagName)
        {
            return _customBlockTagRendererRegistry.HasKey(tagName);
        }

        public bool HasVariableReference(String key)
        {
            return _variableDictionary.ContainsKey(key);
        }


        public bool HasLocalRegistryVariableReference(string key)
        {
            return _localRegistry.ContainsKey(key);
        }

        public Type ReferenceCustomBlockTag(String key)
        {
            return _customBlockTagRendererRegistry.Find(key);
        }

        public Type ReferenceCustomTag(String key)
        {
            return _customTagRendererRegistry.Find(key);
        }

        public Type ReferenceFilter(String key)
        {
            return _filterRegistry.Find(key);
        }

        public MacroBlockTag ReferenceMacro(String key)
        {
            if (HasMacro(key))
            {
                return _macroRegistry[key];
            }
            else
            {
                return null;
            }
        }

        public LiquidExpressionResult ReferenceLocalVariable(String key)
        {
            if (HasVariableReference(key))
            {
                var expressionConstant = _variableDictionary[key] == null ? new None<IExpressionConstant>() : _variableDictionary[key].ToOption();

                return LiquidExpressionResult.Success(expressionConstant);
            }
            else
            {
                return LiquidExpressionResult.Success(new None<IExpressionConstant>());
            }
        }

        public Object ReferenceLocalRegistryVariable(string key)
        {
            if (HasLocalRegistryVariableReference(key))
            {
                return _localRegistry[key];
            }
            else
            {
                return null;
            }
        }
    }
}
