using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Liquid.NET.Constants;
using Liquid.NET.Filters;
using Liquid.NET.Symbols;
using Liquid.NET.Tags.Custom;
using Liquid.NET.Utils;

namespace Liquid.NET
{
    public interface ITemplateContext
    {
        ITemplateContext WithStandardFilters();
        ITemplateContext WithShopifyFilters();
        ITemplateContext WithAllFilters();
        ITemplateContext WithFilter<T>(String name) where T : IFilterExpression;
        ITemplateContext DefineLocalVariable(String name, Option<ILiquidValue> constant);
        ITemplateContext WithCustomTagRenderer<T>(string echoargs) where T: ICustomTagRenderer;
        ITemplateContext WithCustomTagBlockRenderer<T>(string echoargs)  where T: ICustomBlockTagRenderer;
        ITemplateContext WithFileSystem(IFileSystem fileSystem);
        ITemplateContext WithRegisters(IDictionary<String, Object> kv);
        ITemplateContext WithLocalVariables(IDictionary<String, Option<ILiquidValue>> kv);
        ITemplateContext WithNoForLimit();
        ITemplateContext WithASTGenerator(Func<string, Action<LiquidError>, LiquidAST> astGeneratorFunc);


        IFileSystem FileSystem { get; }
        IDictionary<String, Object> Registers { get; }
        SymbolTableStack SymbolTableStack { get; }
        LiquidOptions Options { get; }

        Func<string, Action<LiquidError>, LiquidAST> ASTGenerator { get; }
        //bool RenderErrorsInline { get; set; }
    }
}