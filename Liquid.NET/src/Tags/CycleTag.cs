using System.Collections.Generic;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Tags
{
    public class CycleTag : IASTNode
    {
        public CycleTag()
        {
            CycleList = new List<TreeNode<LiquidExpression>>();
        }
        //public IList<IExpressionConstant> CycleList = new List<IExpressionConstant>();
        public  IList<TreeNode<LiquidExpression>> CycleList { get; set; }

        //public String Group = "";
        public TreeNode<LiquidExpression> GroupNameExpressionTree { get; set; }

        public int Length { get { return CycleList.Count; } }
        //private int _index = 0;

        public TreeNode<LiquidExpression> ElementAt(int index)
        {
           return CycleList[index];

        }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }



    }
}
