using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Dfa;
using Liquid.NET.Expressions;
using Liquid.NET.Symbols;

namespace Liquid.NET.Constants
{
    public class GeneratorValue : ExpressionConstant, IEnumerable<ExpressionConstant>
    {
        private readonly NumericValue _start;
        private readonly NumericValue _end;

        public GeneratorValue(NumericValue start, NumericValue end)
        {
            _start = start;
            _end = end;
        }

        public override object Value
        {
            get { throw new NotImplementedException(); }
        }

        public override bool IsTrue
        {
            get { return true; }
        }

        public override string LiquidTypeName
        {
            get { return "range"; }
        }

        public int Length
        {
            get
            {
                return Math.Abs(_start.IntValue - _end.IntValue) + 1;
            }
        }

        public IEnumerator<ExpressionConstant> GetEnumerator()
        {
            var start = _start.IntValue;
            var end = _end.IntValue;

            if (end < start)
            {
                return CreateEnumerable(end, start).Reverse().GetEnumerator();
            }
            else
            {
                return CreateEnumerable(start, end).GetEnumerator();
            }

        }

        private IEnumerable<NumericValue> CreateEnumerable(int start, int end)
        {
            var length = end - start + 1;
            return Enumerable.Range(start, length)
                .Select(x => new NumericValue(x));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
