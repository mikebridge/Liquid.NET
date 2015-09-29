using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Filters;
using Liquid.NET.Symbols;
using Liquid.NET.Tags.Custom;

namespace Liquid.NET
{
    public interface ITemplateContext
    {
        ITemplateContext WithStandardFilters();
        ITemplateContext WithShopifyFilters();
        ITemplateContext WithAllFilters();
        ITemplateContext WithFilter<T>(String name) where T : IFilterExpression;
        ITemplateContext DefineLocalVariable(String name, IExpressionConstant constant);
        ITemplateContext WithCustomTagRenderer<T>(string echoargs) where T: ICustomTagRenderer;
        ITemplateContext WithCustomTagBlockRenderer<T>(string echoargs)  where T: ICustomBlockTagRenderer;
        ITemplateContext WithFileSystem(IFileSystem fileSystem);
        ITemplateContext WithRegisters(IDictionary<String, Object> kv);
        ITemplateContext WithLocalVariables(IDictionary<String, IExpressionConstant> kv);
        ITemplateContext WithNoForLimit();

        IFileSystem FileSystem { get; }
        IDictionary<String, Object> Registers { get; }
        SymbolTableStack SymbolTableStack { get; }
        LiquidOptions Options { get; }

        Func<string, LiquidAST> ASTGenerator { get; }
    }
}