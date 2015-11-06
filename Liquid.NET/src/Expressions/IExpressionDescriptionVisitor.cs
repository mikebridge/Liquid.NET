using Liquid.NET.Constants;

namespace Liquid.NET.Expressions
{
    public interface IExpressionDescriptionVisitor
    {
        void Visit(VariableReference variableReference);

        void Visit(LiquidString liquidString);

        void Visit(LiquidBoolean liquidBoolean);

        void Visit(LiquidNumeric liquidNumeric);

        void Visit(LiquidDate liquidDate);

        void Visit(AndExpression expressionDescription);
        
        void Visit(OrExpression expressionDescription);

        void Visit(EqualsExpression expressionDescription);

        void Visit(GroupedExpression expressionDescription);
        
        void Visit(NotExpression expressionDescription);

        void Visit(GreaterThanExpression expressionDescription);

        void Visit(LessThanExpression expressionDescription);

        void Visit(LessThanOrEqualsExpression expressionDescription);

        void Visit(GreaterThanOrEqualsExpression expressionDescription);

        void Visit(IsEmptyExpression expressionDescription);

        void Visit(NotEqualsExpression expressionDescription);

        void Visit(ContainsExpression expressionDescription);

        void Visit(IsBlankExpression expressionDescription);

        void Visit(IsPresentExpression expressionDescription);
    }
}
