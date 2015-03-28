using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Tags;

namespace Liquid.NET.Symbols
{

    public interface IASTVisitor
    {
        void Visit(ObjectExpression objectExpression);

        void Visit(ObjectExpressionTree objectExpressionTree);

        void Visit(RawBlock rawBlock);

        void Visit(CommentBlock commentBlock);

        void Visit(CustomTag customTag);

        void Visit(RootDocumentSymbol rootDocumentSymbol);

        void Visit(VariableReference variableReference);

        void Visit(StringValue stringValue);

        void Visit(ForBlock forBlock);

        void Visit(IfThenElseBlock ifThenElseBlock);

        void Visit(CycleTag cycleTag);

        void Visit(UnlessBlock unlessBlock);

        void Visit(CaseBlock caseBlock);

        void Visit(AssignTag assignTag);

        void Visit(CaptureBlock captureBlock);

        void Visit(DecrementTag decrementTag);

        void Visit(IncrementTag incrementTag);

        void Visit(IncludeTag includeTag);

        void Visit(CaseWhenElseBlock caseWhenElseBlock);
    }
}
