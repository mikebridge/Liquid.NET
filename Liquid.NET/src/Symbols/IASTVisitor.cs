using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Tags;

namespace Liquid.NET.Symbols
{

    public interface IASTVisitor
    {
        void Visit(ObjectExpression objectExpression);

        void Visit(ObjectExpressionTree objectExpressionTree);

        void Visit(RawBlockTag rawBlockTag);

        void Visit(CommentBlockTag commentBlockTag);

        void Visit(CustomTag customTag);

        void Visit(RootDocumentNode rootDocumentNode);

        void Visit(VariableReference variableReference);

        void Visit(StringValue stringValue);

        void Visit(ForTagBlock forTagBlock);

        void Visit(IfThenElseBlockTag ifThenElseBlockTag);

        void Visit(CycleTag cycleTag);

        void Visit(AssignTag assignTag);

        void Visit(CaptureBlockTag captureBlockTag);

        void Visit(DecrementTag decrementTag);

        void Visit(IncrementTag incrementTag);

        void Visit(IncludeTag includeTag);

        void Visit(CaseWhenElseBlockTag caseWhenElseBlockTag);
    }
}
