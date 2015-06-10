using System;
using System.Collections.Generic;
using System.Net;
using Liquid.NET.Constants;
using Liquid.NET.Filters;
using Liquid.NET.Filters.Array;
using Liquid.NET.Filters.Math;
using Liquid.NET.Filters.Strings;
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
        private readonly IDictionary<String, IExpressionConstant> _varDictionary = new Dictionary<string, IExpressionConstant>();

        private readonly FilterRegistry _filterRegistry = new FilterRegistry();

        private readonly Registry<ICustomTagRenderer> _customTagRegistry = new Registry<ICustomTagRenderer>();

        private readonly Registry<ICustomBlockTagRenderer> _customBlockTagRegistry = new Registry<ICustomBlockTagRenderer>();

        public ITemplateContext Define(String name, IExpressionConstant constant)
        {
            if (_varDictionary.ContainsKey(name))
            {
                _varDictionary[name] = constant;
            }
            else
            {
                _varDictionary.Add(name, constant);
            }
            return this;
        }

        public ITemplateContext WithCustomTagRenderer<T>(string name) where T : ICustomTagRenderer
        {
            _customTagRegistry.Register<T>(name);
            return this;
        }

        public ITemplateContext WithCustomTagBlockRenderer<T>(string name) where T : ICustomBlockTagRenderer
        {
            _customBlockTagRegistry.Register<T>(name);
            return this;
        }
       
        internal FilterRegistry FilterRegistry { get { return _filterRegistry; } }

        internal IDictionary<String, IExpressionConstant> VariableDictionary { get { return _varDictionary; } }

        internal Registry<ICustomTagRenderer> CustomTagRendererRegistry { get { return _customTagRegistry; } }

        internal Registry<ICustomBlockTagRenderer> CustomBlockTagRendererRegistry { get { return _customBlockTagRegistry; } }

        [Obsolete] // this should be transferred to the ScopeStack
        public IExpressionConstant Reference(String name)
        {
            if (_varDictionary.ContainsKey(name))
            {
                return _varDictionary[name];
            }
            return new Undefined(name);
        }

        public ITemplateContext WithStandardFilters()
        {

            // append
            _filterRegistry.Register<AppendFilter>("append");
            // capitalize
            _filterRegistry.Register<CapitalizeFilter>("capitalize");
            // ceil
            _filterRegistry.Register<CeilFilter>("ceil");
            // concat
            // date
            _filterRegistry.Register<DateFilter>("date");
            // default
            // divided_by
            _filterRegistry.Register<DividedByFilter>("divided_by");
            // downcase
            _filterRegistry.Register<DownCaseFilter>("downcase");
            // escape // NOTE: this is aliased "h"
            _filterRegistry.Register<EscapeFilter>("escape");
            _filterRegistry.Register<EscapeFilter>("h");
            // escape_once
            // first
            _filterRegistry.Register<FirstFilter>("first");
            // floor
            _filterRegistry.Register<FloorFilter>("floor");
            // join
            _filterRegistry.Register<JoinFilter>("join");
            // last
            _filterRegistry.Register<LastFilter>("last");
            // lstrip
            _filterRegistry.Register<LStripFilter>("lstrip");
            // map
            _filterRegistry.Register<MapFilter>("map");
            // minus
            _filterRegistry.Register<MinusFilter>("minus");
            // modulo
            _filterRegistry.Register<ModuloFilter>("modulo");
            // newline_to_br
            _filterRegistry.Register<NewlineToBrFilter>("newline_to_br");
            // plus
            _filterRegistry.Register<PlusFilter>("plus");
            // prepend
            _filterRegistry.Register<PrependFilter>("prepend");
            // remove
            _filterRegistry.Register<RemoveFilter>("remove");
            // remove_first
            _filterRegistry.Register<RemoveFirstFilter>("remove_first");
            // replace_first
            _filterRegistry.Register<ReplaceFirstFilter>("replace_first");
            // reverse
            _filterRegistry.Register<ReplaceFilter>("replace");
            // round
            _filterRegistry.Register<RoundFilter>("round");
            // rstrip
            _filterRegistry.Register<RStripFilter>("rstrip");
            // size
            _filterRegistry.Register<SizeFilter>("size");
            // slice
            _filterRegistry.Register<SliceFilter>("slice");
            // sort
            _filterRegistry.Register<SortFilter>("sort");
            // split
            _filterRegistry.Register<SplitFilter>("split");
            // strip
            _filterRegistry.Register<StripFilter>("strip");
            // strip_html
            _filterRegistry.Register<StripHtmlFilter>("strip_html");
            // strip_newlines
            _filterRegistry.Register<StripNewlinesFilter>("strip_newlines");
            // times
            _filterRegistry.Register<TimesFilter>("times");
            // to_date
            // to_number
            // truncate
            _filterRegistry.Register<TruncateFilter>("truncate");
            // truncatewords
            _filterRegistry.Register<TruncateWordsFilter>("truncatewords");
            // uniq
            _filterRegistry.Register<UniqFilter>("uniq");
            // upcase
            _filterRegistry.Register<UpCaseFilter>("upcase");
            // url_encode
            _filterRegistry.Register<UrlEscapeFilter>("url_escape");


            return this;   
        }

        public ITemplateContext WithShopifyFilters()
        {
            _filterRegistry.Register<CamelCaseFilter>("camelcase");
            //_filterRegistry.Register<PascalCaseFilter>("pascalcase");
            _filterRegistry.Register<HandleizeFilter>("handleize");
            _filterRegistry.Register<Md5Filter>("md5");
            _filterRegistry.Register<PluralizeFilter>("pluralize");
            _filterRegistry.Register<UrlParamEscapeFilter>("url_param_escape");
            return this;
        }

        public ITemplateContext WithAllFilters()
        {
            WithStandardFilters();
            WithShopifyFilters();
            //WithJekyllFilters();
            return this;

        }


        public ITemplateContext WithFilter<T>(String name)
            where T: IFilterExpression
        {
            _filterRegistry.Register<T>(name);
            return this;
        }


    }

    public interface ITemplateContext
    {
        ITemplateContext WithStandardFilters();
        ITemplateContext WithShopifyFilters();
        ITemplateContext WithAllFilters();
        ITemplateContext WithFilter<T>(String name) where T : IFilterExpression;
        ITemplateContext Define(String name, IExpressionConstant constant);
        ITemplateContext WithCustomTagRenderer<T>(string echoargs) where T: ICustomTagRenderer;
        ITemplateContext WithCustomTagBlockRenderer<T>(string echoargs)  where T: ICustomBlockTagRenderer;
    }
}
