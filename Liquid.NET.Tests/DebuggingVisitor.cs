using System;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Symbols;
using Liquid.NET.Tags;
using Liquid.NET.Utils;

namespace Liquid.NET.Tests
{
    public class DebuggingVisitor : IASTVisitor
    {
        private String _result = "";

        public void Visit(LiquidExpression liquidExpression)
        {
            _result += liquidExpression.ToString();
        }

        public void Visit(LiquidExpressionTree liquidExpressionTree)
        {
            _result += liquidExpressionTree.ToString();
        }

        public void Visit(RawBlockTag rawBlockTag)
        {
            _result += rawBlockTag.ToString();
        }


        public void Visit(CustomTag customTag)
        {
            _result += customTag.ToString();
        }

        public void Visit(CustomBlockTag caseWhenElseBlockTag)
        {
            _result += caseWhenElseBlockTag.ToString();
        }

        public void Visit(ForBlockTag forBlockTag)
        {
            _result += forBlockTag.ToString();
        }

        public void Visit(IfThenElseBlockTag ifThenElseBlockTag)
        {
            _result += ifThenElseBlockTag.ToString();
            _result += ifThenElseBlockTag.IfElseClauses.Select(x => VisitIfTag(x.LiquidExpressionTree));
        }

        public void Visit(CycleTag cycleTag)
        {
            _result += cycleTag.ToString();
        }

        public void Visit(AssignTag assignTag)
        {
            _result += assignTag.ToString();
        }

        public void Visit(CaptureBlockTag captureBlockTag)
        {
            _result += captureBlockTag.ToString();
        }

        public void Visit(DecrementTag decrementTag)
        {
            _result += decrementTag.ToString();
        }

        public void Visit(IncrementTag incrementTag)
        {
            _result += incrementTag.ToString();
        }

        public void Visit(IncludeTag includeTag)
        {
            _result += includeTag.ToString();
        }

        public void Visit(CaseWhenElseBlockTag caseWhenElseBlockTag)
        {
            _result += caseWhenElseBlockTag.ToString();
        }

        public void Visit(ContinueTag continueTag)
        {
            _result += continueTag.ToString();
        }

        public void Visit(BreakTag breakTag)
        {
            _result += breakTag.ToString();
        }

        public void Visit(MacroBlockTag macroBlockTag)
        {
            _result += macroBlockTag.ToString();
        }

//        public void Visit(ErrorNode errorNode)
//        {
//            _result += errorNode.ToString();
//        }

        public void Visit(IfChangedBlockTag ifChangedBlockTag)
        {
            _result += ifChangedBlockTag.ToString();
        }

        public void Visit(TableRowBlockTag tableRowBlockTag)
        {
            _result += tableRowBlockTag.ToString();
        }

        private String VisitIfTag(TreeNode<IExpressionDescription> exprNode)
        {
            String result = " IF => " + exprNode.Data;
            return result + "    -> " + exprNode.Children.Select(x => VisitIfTag(exprNode));
        }

        public void Visit(RootDocumentNode rootDocumentNode)
        {
            _result += rootDocumentNode.ToString();
        }

        public void Visit(VariableReference variableReference)
        {
            _result += variableReference.ToString();
        }

        public void Visit(LiquidString liquidString)
        {
            _result += liquidString.ToString();
        }

        public String Result()
        {
            return _result;
        }


    }
}
