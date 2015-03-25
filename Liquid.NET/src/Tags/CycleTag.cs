using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;

namespace Liquid.NET.Tags
{
    public class CycleTag : IASTNode
    {
        public IList<IExpressionConstant> CycleList = new List<IExpressionConstant>();

        public String Group = "";


        public int Length { get { return CycleList.Count; } }
        //private int _index = 0;

        public IExpressionConstant ElementAt(int index)
        {
           return CycleList[index];

        }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }



    }
}
