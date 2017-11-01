using System;
using System.Collections.Generic;
using System.Linq;

namespace ACSWebUI.Extensions {
    public static class EnumerableExtensions {
        public static void ForEach<TValue>(this IEnumerable<TValue> enumerable, Action<TValue> action) {
            foreach (var value in enumerable)
                action(value);
        }

        public static void ForEach<TValue>(this IEnumerable<TValue> enumerable, Action<TValue, int> action) {
            var i = 0;
            foreach (var value in enumerable)
                action(value, i++);
        }

        public static TValue[] ToArrayIfNotNull<TValue>(this IEnumerable<TValue> enumerable) {
            return enumerable == null ? null : (enumerable as TValue[] ?? enumerable.ToArray());
        }
    }
}