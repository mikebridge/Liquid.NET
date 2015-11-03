using System;

namespace Liquid.NET.Utils
{
    public abstract class Try<T>
    {
        public abstract T Value { get; }

        public abstract Exception Exception { get; }

        public abstract bool IsSuccess { get; }

        public abstract bool IsFailure { get; }

    }

    public sealed class Success<T> : Try<T>
    {
        private readonly T _val;

        public Success(T val) { _val = val; }

        public override T Value { get { return _val; } }

        public override Exception Exception { get { throw new InvalidOperationException("No exception was generated."); } }

        public override bool IsSuccess { get { return true; } }

        public override bool IsFailure { get { return false; } }
    }

    public sealed class Failure<T> : Try<T>
    {
        private readonly Exception _ex;

        public Failure(Exception ex) { _ex = ex; }

        public override Exception Exception { get { return _ex; } }

        public override T Value { get { throw new InvalidOperationException("This has no value"); } }

        public override bool IsSuccess { get { return false; } }

        public override bool IsFailure { get { return true; } }
    }


}
