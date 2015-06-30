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

        /// <summary>
        /// TODO: what is the point of this again?
        /// </summary>
        /// <param name="symbolTableStack"></param>
        /// <param name="expressions"></param>
        /// <returns></returns>

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
        /*
        private readonly IExpressionConstant _start;
        private readonly IExpressionConstant _end;

        // TODO: this should be a little more useful;
//        public GeneratorValue(IExpressionConstant start, IExpressionConstant end)
//        {
//            _start = start;
//            _end = end;
//        }

        public void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor)
        {
            throw new NotImplementedException();
        }

        public override IExpressionConstant Eval(SymbolTableStack symbolTableStack, IEnumerable<IExpressionConstant> expressions)
        {
            // I think this needs two values...?
            return this;
            //throw new NotImplementedException("NEED TO CREATE A LIST BETWEEN THE TWO ARGS");
            
        }

        public bool HasError
        {
            get { throw new NotImplementedException(); }
        }

        public string ErrorMessage
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool HasWarning
        {
            get { throw new NotImplementedException(); }
        }

        public string WarningMessage
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override object Value
        {
            get { throw new NotImplementedException(); }
        }

        public override bool IsTrue
        {
            get { return true; }
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
         * */


    }
}
