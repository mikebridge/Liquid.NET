using System;

namespace Liquid.NET.Utils
{
    public static class Either
    {
        public static Either<TL, TR> Left<TL, TR>(TL left)
        {
            return new Either<TL, TR>(left);
        }
        public static Either<TL, TR> Right<TL, TR>(TR right)
        {
            return new Either<TL, TR>(right);
        }
    }

    public class Either<TL, TR> //: IEquatable<Either<TL, TR>>
    {
        internal Either(TL left)
        {
            
            _left = left;
            _right = default(TR);
            _isLeft = true;
        }
        internal Either(TR right)
        {

            _left = default(TL);
            _right = right;
            _isLeft = false;
        }

        private readonly TL _left;
        private readonly TR _right;
        private readonly bool _isLeft;


        public bool IsLeft
        {
            get { return _isLeft; }
        }

        public bool IsRight { get { return !IsLeft; } }

        public TR Right
        {
            get
            {
                if (!IsRight)
                {
                    throw new InvalidOperationException("Not in the right state");
                }
                return _right;
            }
        }

        public TL Left
        {
            get
            {
                if (!IsLeft)
                {
                    throw new InvalidOperationException("Not in the left state");
                }
                return _left;
            }
        }

        public bool Equals(Either<TL, TR> other)
        {
            return Equals((object) other);
        }
    }
}
