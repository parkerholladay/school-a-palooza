using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SchoolAPalooza.Infrastructure
{
    public class Maybe<T> : IEnumerable<T>
    {
        readonly IEnumerable<T> values;

        public static Maybe<T> Some(T value)
        {
            return new Maybe<T>(value);
        }

        public static Maybe<T> None()
        {
            return new Maybe<T>();
        }

        Maybe()
        {
            values = new T[0];
        }

        Maybe(T value)
        {
            values = new[] { value };
        }

        public IEnumerator<T> GetEnumerator()
        {
            return values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool HasValue
        {
            get { return values.Any(); }
        }

        public T Value()
        {
            if (!HasValue) throw new InvalidOperationException("Maybe does not have a value");

            return values.Single();
        }

        public T ValueOrDefault(T @default)
        {
            return values.DefaultIfEmpty(@default).Single();
        }

        public U Case<U>(Func<T, U> some, Func<U> none)
        {
            return HasValue
                ? some(Value())
                : none();
        }
    }

    public static class MaybeExtensions
    {
        public static Maybe<T> ToMaybe<T>(this T value) where T : class
        {
            return value != null
                ? Maybe<T>.Some(value)
                : Maybe<T>.None();
        }

        public static Maybe<T> ToMaybe<T>(this T? nullable) where T : struct
        {
            return nullable.HasValue
                ? Maybe<T>.Some(nullable.Value)
                : Maybe<T>.None();
        }

        public static IEnumerable<T> SkipMissingValues<T>(this IEnumerable<Maybe<T>> collection) where T : class
        {
            return collection.SelectMany(value => value);
        }
    }
}
