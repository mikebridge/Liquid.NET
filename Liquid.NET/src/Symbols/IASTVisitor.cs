using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Tags;

namespace Liquid.NET.Symbols
{

    public interface IASTVisitor
    {
        void Visit(LiquidExpression liquidExpression);

        void Visit(LiquidExpressionTree liquidExpressionTree);

        void Visit(RawBlockTag rawBlockTag);

        void Visit(CommentBlockTag commentBlockTag);

        void Visit(CustomTag customTag);

        void Visit(CustomBlockTag caseWhenElseBlockTag);

        void Visit(RootDocumentNode rootDocumentNode);

        void Visit(VariableReference variableReference);

        void Visit(StringValue stringValue);

        void Visit(ForBlockTag forBlockTag);

        void Visit(IfThenElseBlockTag ifThenElseBlockTag);

        void Visit(CycleTag cycleTag);

        void Visit(AssignTag assignTag);

        void Visit(CaptureBlockTag captureBlockTag);

        void Visit(DecrementTag decrementTag);

        void Visit(IncrementTag incrementTag);

        void Visit(IncludeTag includeTag);

        void Visit(CaseWhenElseBlockTag caseWhenElseBlockTag);

        void Visit(ContinueTag continueTag);

        void Visit(BreakTag breakTag);

        void Visit(MacroBlockTag macroBlockTag);

        void Visit(ErrorNode errorNode);
    }
}
