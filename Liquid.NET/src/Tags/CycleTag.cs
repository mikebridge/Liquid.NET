using System.Collections.Generic;
using Liquid.NET.Expressions;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Tags
{
    public class CycleTag : IASTNode
    {
        public CycleTag()
        {
            CycleList = new List<TreeNode<IExpressionDescription>>();
        }
        //public IList<ILiquidValue> CycleList = new List<ILiquidValue>();
        public IList<TreeNode<IExpressionDescription>> CycleList { get; set; }

        //public String Group = "";
        public TreeNode<IExpressionDescription> GroupNameExpressionTree { get; set; }

        public int Length { get { return CycleList.Count; } }
        //private int _index = 0;

        public TreeNode<IExpressionDescription> ElementAt(int index)
        {
           return CycleList[index];

        }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }



    }
}
