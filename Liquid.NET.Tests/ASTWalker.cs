using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
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
