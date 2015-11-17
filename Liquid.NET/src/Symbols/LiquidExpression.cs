using System.Collections.Generic;
using Liquid.NET.Expressions;

namespace Liquid.NET.Symbols
{
    /// <summary>
    ///  TODO: Make this an IExpressionDescription
    /// </summary>
    public class LiquidExpression : IExpressionDescription
    {

        public IExpressionDescription Expression { get; set; }

        public IList<FilterSymbol> FilterSymbols { get {return _filterSymbols;} }

        private readonly IList<FilterSymbol> _filterSymbols = new List<FilterSymbol>();

        public void AddFilterSymbol(FilterSymbol filterSymbol)
        {
            _filterSymbols.Add(filterSymbol);
        }


        public void Accept(ILiquidExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
