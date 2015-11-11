using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Filters;
using Liquid.NET.Grammar;
using Liquid.NET.Rendering;
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
        private readonly IDictionary<String, Option<ILiquidValue>> _variableDictionary;
        private readonly IDictionary<String, Object> _localRegistry; // available to tags & filters only
        private readonly IDictionary<String, MacroBlockTag> _macroRegistry;

        public SymbolTable(
            IDictionary<string, Option<ILiquidValue>> variableDictionary = null, 
            FilterRegistry filterRegistry = null, 
            Registry<ICustomTagRenderer> customTagRendererRegistry = null,
            Registry<ICustomBlockTagRenderer> customBlockTagRendererRegistry = null,
            IDictionary<String, Object> localRegistry = null)
        {
            _customBlockTagRendererRegistry = customBlockTagRendererRegistry ?? new Registry<ICustomBlockTagRenderer>();
            _customTagRendererRegistry = customTagRendererRegistry ?? new Registry<ICustomTagRenderer>();
            _variableDictionary = variableDictionary ?? new Dictionary<string, Option<ILiquidValue>>();
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

        public void DefineLocalVariable(String key, Option<ILiquidValue> obj)
        {
            var fixedKey = KeyStrategy(key);
            if (ReferenceEquals(obj,null))
            {
                //throw new ArgumentNullException("obj");
                obj = Option<ILiquidValue>.None();
            }
            if (_variableDictionary.ContainsKey(fixedKey))
            {
                _variableDictionary[fixedKey] = obj;
                SaveIncludeWhereReDefined(obj);
            }
            else
            {
                _variableDictionary.Add(fixedKey, obj);
                SaveIncludeWhereDefined(obj);
            }
        }

        private void SaveIncludeWhereReDefined(Option<ILiquidValue> obj)
        {
            if (HasLocalRegistryVariableReference(IncludeRenderer.LOCALREGISTRY_FILE_KEY))
            {
                if (obj.HasValue && obj.Value.MetaData!= null && !obj.Value.MetaData.ContainsKey("reassigned"))
                {
                    obj.Value.MetaData.Add("reassigned", ReferenceLocalRegistryVariable(IncludeRenderer.LOCALREGISTRY_FILE_KEY));
                }
            }
        }

        private void SaveIncludeWhereDefined(Option<ILiquidValue> obj)
        {
            if (HasLocalRegistryVariableReference(IncludeRenderer.LOCALREGISTRY_FILE_KEY))
            {
                if (obj.HasValue && obj.Value.MetaData != null && !obj.Value.MetaData.ContainsKey("assigned"))
                {
                    obj.Value.MetaData.Add("assigned", ReferenceLocalRegistryVariable(IncludeRenderer.LOCALREGISTRY_FILE_KEY));
                }
            }
        }

        public void DefineLocalRegistryVariable(String key, Object obj)
        {
            var fixedKey = KeyStrategy(key);
            if (_localRegistry.ContainsKey(fixedKey))
            {
                _localRegistry[fixedKey] = obj;
            }
            else
            {
                _localRegistry.Add(fixedKey, obj);
            }
        }

        public void DefineMacro(String key, MacroBlockTag macro)
        {
            var fixedKey = KeyStrategy(key);
            if (_macroRegistry.ContainsKey(fixedKey))
            {
                _macroRegistry[fixedKey] = macro;
            }
            else
            {
                _macroRegistry.Add(fixedKey, macro);
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
            var fixedKey = KeyStrategy(key);
            return _variableDictionary.ContainsKey(fixedKey);
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
            var fixedKey = KeyStrategy(key);
            if (HasVariableReference(fixedKey))
            {
                //var expressionConstant = _variableDictionary[key] == null ? new None<ILiquidValue>() : _variableDictionary[key].ToOption();

                return LiquidExpressionResult.Success(_variableDictionary[fixedKey]);
            }
            else
            {
                //return LiquidExpressionResult.Success(new None<ILiquidValue>());
                return LiquidExpressionResult.Error(NotFoundError(fixedKey));
            }
        }

        public static String NotFoundError(String key)
        {
            return key + " is undefined";
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

        private String KeyStrategy(String rawkey)
        {
            return rawkey.ToLowerInvariant().Trim();
        }
    }
}
