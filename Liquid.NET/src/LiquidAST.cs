using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET
{
    // ReSharper disable once InconsistentNaming
    public class LiquidAST
    {
        //private readonly IList<FilterSymbol> _currentFilterSymbols = new List<FilterSymbol>();
        //private readonly IList<Action<TemplateContext>> _templateActions = new List<Action<TemplateContext>>();
        //private ObjectSymbol _currentObjectSymbol;

        //private IEnumerable<Func<String,String>> _transformers = new List<Func<string,STring>>(); 

        public TreeNode<IASTNode> RootNode { get; private set; }

        public LiquidAST()
        {
            RootNode = new TreeNode<IASTNode>(new RootDocumentSymbol());  
        } 

        //private readonly IList<TreeNode<IASTNode>> _children = new List<TreeNode<IASTNode>>();


//        internal void AddChild(TreeNode<IASTNode> child)
//        {
//            RootNode.Children.Add(child);
//        }
//
//        public IEnumerable<TreeNode<IASTNode>> Children
//        {
//            get { return RootNode.Children; }
//        } 

    }
}