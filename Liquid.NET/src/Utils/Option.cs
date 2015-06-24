using System;
using System.Runtime.CompilerServices;

namespace Liquid.NET.Utils
{
    public abstract class Option<T> : IEquatable<T> //: IEquatable<Option<T>>
    {
        public abstract T Value { get; protected set; }

        public abstract bool HasValue { get; }

        public bool Equals(T other)
        {
            return Equals((object) other);
        }

        public static Option<T> Create(T val)
        {
            return val == null ?
                new None<T>() :
                new Some<T>(val) as Option<T>;
                
        }

        public static Option<T> None()
        {
            return new None<T>();
        }

    }

    public class Some<T> : Option<T>
    {
        public Some(T val)
        {
            Value = val;
        }

        public override sealed T Value { get; protected set; }

        public override bool HasValue
        {
            get { return true; }
        }
    }

    public class None<T> : Option<T>
    {
        public override T Value
        {
            get { throw new InvalidOperationException("This has no value"); }
            protected set { throw new InvalidOperationException("Can't set a value on None"); }
        }

        public override bool HasValue
        {
            get { return false; }
        }

        public override string ToString()
        {
            return "[Nothing]";
        }

    }

    public static class OptionExtensions
    {
        public static Option<T> WhenSome<T>(this Option<T> self, Action<T> fn)
        {
            if (self.HasValue)
            {
                fn(self.Value);
            }
            return self;
        }

        public static Option<T> WhenNone<T>(this Option<T> self, Action fn)
        {
            if (!self.HasValue)
            {
                fn();
            }
            return self;
        }

        /// <summary>
        /// A binder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        //public static Option<T> Bind<T>(this Option<T> self, Func<T, Option<T>> f)
        public static Option<T> Bind<T>(this Option<T> self, Func<T, Option<T>> f)
        {
            if (f == null) throw new ArgumentNullException("f");

            return self.HasValue
                ? f(self.Value)
                : new None<T>();
        }

    }

}
