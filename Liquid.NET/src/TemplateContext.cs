using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using Liquid.NET.Constants;
using Liquid.NET.Filters;
using Liquid.NET.Filters.Array;
using Liquid.NET.Filters.Math;
using Liquid.NET.Filters.Strings;
using Liquid.NET.Symbols;
using Liquid.NET.Tags.Custom;
using Liquid.NET.Utils;

namespace Liquid.NET
{
    /// <summary>
    /// The template context is the information that comes from the user for parsing a template.  It is
    /// used to create the SymbolTableStack that the renderer uses to render the template.
    /// 
    /// This data should only be modified by the caller---the renderer should leave it untouched.
    /// </summary>
    public class TemplateContext : ITemplateContext
    {


        public SymbolTableStack SymbolTableStack {get {return _symbolTablestack;}}
        private readonly SymbolTableStack _symbolTablestack = new SymbolTableStack();
        private readonly SymbolTable _globalSymbolTable;
        private readonly IDictionary<String, Object> _registers = new ConcurrentDictionary<String, Object>();
        public IDictionary<string, object> Registers { get { return _registers;} }
        private IFileSystem _fileSystem;
        private readonly LiquidOptions _options = new LiquidOptions();
        private Func<string, Action<LiquidError>, LiquidAST> _astGeneratorFunc;

        public LiquidOptions Options
        {
            get { return _options; }
        }

        public Func<string, Action<LiquidError>, LiquidAST> ASTGenerator
        {
            get { return _astGeneratorFunc; }
        }

        public TemplateContext()
        {            
            _globalSymbolTable = new SymbolTable();
            _symbolTablestack.Push(_globalSymbolTable);            
            _astGeneratorFunc = (snippet,errorfn) => new CachingLiquidASTGenerator(new LiquidASTGenerator()).Generate(snippet, errorfn);
        }        


        public ITemplateContext DefineLocalVariable(String name, Option<ILiquidValue> constant)
        {
            if (constant == null)
            {
                //throw new ArgumentNullException("constant");
                constant = Option<ILiquidValue>.None();
            }
            _globalSymbolTable.DefineLocalVariable(name, constant);
            return this;
        }


        public ITemplateContext DefineLocalRegistryVariable(string name, Object obj)
        {
            _globalSymbolTable.DefineLocalRegistryVariable(name, obj);
            return this;
        }

        public ITemplateContext WithCustomTagRenderer<T>(string name) where T : ICustomTagRenderer
        {
            _globalSymbolTable.DefineCustomTag<T>(name);
            return this;
        }

        public ITemplateContext WithCustomTagBlockRenderer<T>(string name) where T : ICustomBlockTagRenderer
        {
            _globalSymbolTable.DefineCustomBlockTag<T>(name);
            return this;
        }

        public ITemplateContext WithFileSystem(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            return this;
        }

        public ITemplateContext WithRegisters(IDictionary<string, object> dict)
        {
            foreach (var kv in dict)
            {
                _registers.Add(kv);
            }
            return this;
        }

        public ITemplateContext WithLocalVariables(IDictionary<string, Option<ILiquidValue>> dict)
        {
            foreach (var kv in dict)
            {
                _globalSymbolTable.DefineLocalVariable(kv.Key, kv.Value);
            }
            return this;
        }

        public IFileSystem FileSystem
        {
            get { return _fileSystem; }
        }

        public ITemplateContext WithStandardFilters()
        {

            // append
            _globalSymbolTable.DefineFilter<AppendFilter>("append");
            // capitalize
            _globalSymbolTable.DefineFilter<CapitalizeFilter>("capitalize");
            // ceil
            _globalSymbolTable.DefineFilter<CeilFilter>("ceil");
            // concat
            // date
            _globalSymbolTable.DefineFilter<DateFilter>("date");
            // default
            // divided_by
            _globalSymbolTable.DefineFilter<DividedByFilter>("divided_by");
            // downcase
            _globalSymbolTable.DefineFilter<DownCaseFilter>("downcase");
            // escape // NOTE: this is aliased "h"
            _globalSymbolTable.DefineFilter<EscapeFilter>("escape");
            _globalSymbolTable.DefineFilter<EscapeFilter>("h");
            // escape_once
            // first
            _globalSymbolTable.DefineFilter<FirstFilter>("first");
            // floor
            _globalSymbolTable.DefineFilter<FloorFilter>("floor");
            // join
            _globalSymbolTable.DefineFilter<JoinFilter>("join");
            // last
            _globalSymbolTable.DefineFilter<LastFilter>("last");
            // lstrip
            _globalSymbolTable.DefineFilter<LStripFilter>("lstrip");
            // map
            _globalSymbolTable.DefineFilter<MapFilter>("map");
            // minus
            _globalSymbolTable.DefineFilter<MinusFilter>("minus");
            // modulo
            _globalSymbolTable.DefineFilter<ModuloFilter>("modulo");
            // newline_to_br
            _globalSymbolTable.DefineFilter<NewlineToBrFilter>("newline_to_br");
            // plus
            _globalSymbolTable.DefineFilter<PlusFilter>("plus");
            // prepend
            _globalSymbolTable.DefineFilter<PrependFilter>("prepend");
            // remove
            _globalSymbolTable.DefineFilter<RemoveFilter>("remove");
            // remove_first
            _globalSymbolTable.DefineFilter<RemoveFirstFilter>("remove_first");
            // replace_first
            _globalSymbolTable.DefineFilter<ReplaceFirstFilter>("replace_first");
            // reverse
            _globalSymbolTable.DefineFilter<ReplaceFilter>("replace");
            // round
            _globalSymbolTable.DefineFilter<RoundFilter>("round");
            // rstrip
            _globalSymbolTable.DefineFilter<RStripFilter>("rstrip");
            // size
            _globalSymbolTable.DefineFilter<SizeFilter>("size");
            // slice
            _globalSymbolTable.DefineFilter<SliceFilter>("slice");
            // sort
            _globalSymbolTable.DefineFilter<SortFilter>("sort");
            // split
            _globalSymbolTable.DefineFilter<SplitFilter>("split");
            // strip
            _globalSymbolTable.DefineFilter<StripFilter>("strip");
            // strip_html
            _globalSymbolTable.DefineFilter<StripHtmlFilter>("strip_html");
            // strip_newlines
            _globalSymbolTable.DefineFilter<StripNewlinesFilter>("strip_newlines");
            // times
            _globalSymbolTable.DefineFilter<TimesFilter>("times");
            // to_date
            // to_number
            // truncate
            _globalSymbolTable.DefineFilter<TruncateFilter>("truncate");
            // truncatewords
            _globalSymbolTable.DefineFilter<TruncateWordsFilter>("truncatewords");
            // uniq
            _globalSymbolTable.DefineFilter<UniqFilter>("uniq");
            // upcase
            _globalSymbolTable.DefineFilter<UpCaseFilter>("upcase");
            // url_encode
            _globalSymbolTable.DefineFilter<UrlEscapeFilter>("url_escape");
            _globalSymbolTable.DefineFilter<DefaultFilter>("default");

            return this;
        }

        public ITemplateContext WithDebuggingFilters()
        {
            _globalSymbolTable.DefineFilter<TypeOfFilter>("type_of");
            _globalSymbolTable.DefineFilter<DebugFilter>("debug");
            return this;
            
        }

        public ITemplateContext WithShopifyFilters()
        {
            _globalSymbolTable.DefineFilter<CamelCaseFilter>("camelcase");
            //_globalSymbolTable.DefineFilter<PascalCaseFilter>("pascalcase");
            _globalSymbolTable.DefineFilter<HandleizeFilter>("handleize");
            _globalSymbolTable.DefineFilter<Md5Filter>("md5");
            _globalSymbolTable.DefineFilter<PluralizeFilter>("pluralize");
            _globalSymbolTable.DefineFilter<UrlParamEscapeFilter>("url_param_escape");
            return this;
        }

        public ITemplateContext WithAllFilters()
        {
            WithStandardFilters();
            WithShopifyFilters();
            WithDebuggingFilters();
            WithNoForLimit();
            //WithJekyllFilters();
            return this;

        }


        public ITemplateContext WithFilter<T>(String name)
            where T : IFilterExpression
        {
            _globalSymbolTable.DefineFilter<T>(name);
            return this;
        }

        /// <summary>
        /// Override the default transform text into a LiquidAST object.
        /// 
        /// Currently only applies to Includes, but TODO it should apply to all AST generation.
        /// </summary>
        /// <param name="astGeneratorFunc"></param>
        /// <returns></returns>
        public ITemplateContext WithASTGenerator(Func<string, Action<LiquidError>, LiquidAST> astGeneratorFunc)
        {
            if (astGeneratorFunc == null)
            {
                throw new ArgumentNullException("astGeneratorFunc");
            }
            _astGeneratorFunc = astGeneratorFunc;
            return this;
        }

        public ITemplateContext WithNoForLimit()
        {
            _options.NoForLimit = true;
            return this;
        }

        public ITemplateContext ErrorWhenValueMissing()
        {
            _options.ErrorWhenValueMissing = true;
            return this;
        }
    }

    public class LiquidOptions
    {
        public bool NoForLimit { get; internal set; }
        public bool ErrorWhenValueMissing { get; set; }
    }

}
