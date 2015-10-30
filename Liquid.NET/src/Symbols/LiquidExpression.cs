﻿using System;
using System.Collections.Generic;
using Liquid.NET.Expressions;

namespace Liquid.NET.Symbols
{

    public class LiquidExpression : IASTNode
    {

        public IExpressionDescription Expression { get; set; }

        public IList<FilterSymbol> FilterSymbols { get {return _filterSymbols;} }

        private readonly IList<FilterSymbol> _filterSymbols = new List<FilterSymbol>();

        public void AddFilterSymbol(FilterSymbol filterSymbol)
        {
            _filterSymbols.Add(filterSymbol);
        }


        // this never gets called....
        public void Accept(IASTVisitor visitor)
        {                   
            visitor.Visit(this);
        }

    }
}
