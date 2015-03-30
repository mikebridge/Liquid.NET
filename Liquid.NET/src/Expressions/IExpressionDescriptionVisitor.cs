using Liquid.NET.Constants;

namespace Liquid.NET.Expressions
{
    public interface IExpressionDescriptionVisitor
    {
        void Visit(VariableReference variableReference);

        void Visit(StringValue stringValue);

        void Visit(BooleanValue booleanValue);

        void Visit(NumericValue numericValue);

        void Visit(DateValue dateValue);

        void Visit(AndExpression expressionDescription);
        
        void Visit(OrExpression expressionDescription);

        void Visit(EqualsExpression expressionDescription);

        void Visit(GroupedExpression expressionDescription);
        
        void Visit(NotExpression expressionDescription);


    }
}
