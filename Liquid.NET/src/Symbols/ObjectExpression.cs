using System.Collections.Generic;
using Liquid.NET.Expressions;

namespace Liquid.NET.Symbols
{
    // TODO: Change this to "ObjectFilterChain" or something.
    public class ObjectExpression : IASTNode
    {

        public IExpressionDescription Expression { get; set; }

        public IList<FilterSymbol> FilterSymbols { get {return _filterSymbols;} }

        private readonly IList<FilterSymbol> _filterSymbols = new List<FilterSymbol>();

        public void AddFilterSymbol(FilterSymbol filterSymbol)
        {
            _filterSymbols.Add(filterSymbol);
        }

        public void AddFilterSymbols(IEnumerable<FilterSymbol> filterSymbols)
        {
            foreach (var filterSymbol in filterSymbols)
            {
                _filterSymbols.Add(filterSymbol);
            }
        }


        public void Accept(IASTVisitor visitor)
        {            
            visitor.Visit(this);
        }

    }
}
