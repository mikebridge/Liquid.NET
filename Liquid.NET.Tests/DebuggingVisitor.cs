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

        public void Visit(ObjectExpression objectExpression)
        {
            _result += objectExpression.ToString();
        }

        public void Visit(ObjectExpressionTree objectExpressionTree)
        {
            _result += objectExpressionTree.ToString();
        }

        public void Visit(RawBlock rawBlock)
        {
            _result += rawBlock.ToString();
        }

        public void Visit(CommentBlock commentBlock)
        {
            _result += commentBlock.ToString();
        }

        public void Visit(CustomTag customTag)
        {
            _result += customTag.ToString();
        }

        public void Visit(ForBlock forBlock)
        {
            _result += forBlock.ToString();
        }

        public void Visit(IfThenElseBlock ifThenElseBlock)
        {
            _result += ifThenElseBlock.ToString();
            //_result += IfThenElseBlock.IfExpressions.Select( x => Visit(x));
            _result += ifThenElseBlock.IfExpressions.Select(x => VisitIfTag(x.ObjectExpressionTree));
        }

        public void Visit(CycleTag cycleTag)
        {
            _result += cycleTag.ToString();
        }

        public void Visit(UnlessBlock unlessBlock)
        {
            _result += unlessBlock.ToString();
        }

        public void Visit(CaseBlock caseBlock)
        {
            _result += caseBlock.ToString();
        }

        public void Visit(AssignTag assignTag)
        {
            _result += assignTag.ToString();
        }

        public void Visit(CaptureBlock captureBlock)
        {
            _result += captureBlock.ToString();
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

        public void Visit(CaseWhenElseBlock caseWhenElseBlock)
        {
            _result += caseWhenElseBlock.ToString();
        }

        private String VisitIfTag(TreeNode<ObjectExpression> exprNode)
        {
            String result = " IF => " + exprNode.Data;
            return result + "    -> " + exprNode.Children.Select(x => VisitIfTag(exprNode));
        }

        //public void 

//        public void Visit(ExpressionDescription predicateDescription)
//        {
//            throw new NotImplementedException();
//        }

        //public void Visit(IExpressionDescription predicateDescription)
        //{
          //  _result += predicateDescription.ToString();
            //_result += predicateDescription.Eval(); // TODO: figure out how to get the children here....

            //_result += "    " + predicateDescription.Arguments;
        //}

        public void Visit(RootDocumentSymbol rootDocumentSymbol)
        {
            _result += rootDocumentSymbol.ToString();
        }

        public void Visit(VariableReference variableReference)
        {
            _result += variableReference.ToString();
        }

        public void Visit(StringValue stringValue)
        {
            _result += stringValue.ToString();
        }

//        public void Visit(IfThenElseTag IfThenElseBlock)
//        {
//            _result += IfThenElseBlock.ToString();
//        }

        public String Result()
        {
            return _result;
        }


    }
}
