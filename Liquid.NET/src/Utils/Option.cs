using System;

namespace Liquid.NET.Utils
{
    public abstract class Option<T> : IEquatable<T> 
    {
        public abstract T Value { get; protected set; }

        public abstract bool HasValue { get; }

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

        public static bool operator ==(Option<T> option1, Option<T> option2)
        {
            if (ReferenceEquals(null, option1)) throw new ArgumentNullException("option1");
            if (ReferenceEquals(null, option2)) throw new ArgumentNullException("option2");

            return option1.Equals(option2);
        }

        public static bool operator !=(Option<T> lhs, Option<T> rhs)
        {
            return !(lhs == rhs);
        }

        public bool Equals(T other)
        {
            return Equals((object) other);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                var option = obj as Option<T>;
                if (ReferenceEquals(option, null))
                {
                    return true;
                }
                else
                {
                    var rhs = option;
                    return HasValue && rhs.HasValue
                        ? Value.Equals(rhs.Value)
                        : !HasValue && !rhs.HasValue;
                }
            }
        }

        public override int GetHashCode()
        {
            return HasValue
                ? Value == null ? 0 : Value.GetHashCode()
                : new None<T>().GetHashCode();
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
