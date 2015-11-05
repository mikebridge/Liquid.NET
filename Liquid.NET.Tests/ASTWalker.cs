using System;
using System.Linq;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Tests
{
    public class ASTWalker
    {
        public String Walk(LiquidAST liquidAST)
        {
            return Walk(liquidAST.RootNode, 0);
        }

        public String Walk(TreeNode<IASTNode> treeNode, int level)
        {
            String result = new String('>', level) + treeNode.Data;
            return result + "\r\n" + String.Join("\r\n", treeNode.Children.Select(x => Walk(x, level + 1)));
        }

    }
}
