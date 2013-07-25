using System;
using System.Collections;

namespace GitViaCS
{
    static class Xtensions
    {
        public static void Dump<T>(this T @this)
        {
            if (@this is IEnumerable && !(@this is string))
            {
                foreach (var x in (IEnumerable)@this)
                    x.Dump();
            }
            else
            {
                Console.WriteLine(ReferenceEquals(@this, null) ? "(null)" : @this.ToString());
            }
        }

        public static void Dump<T>(this T @this,
                                   string format,
                                   IFormatProvider formatProvider = null)
            where T : IFormattable
        {
            Console.WriteLine(ReferenceEquals(@this, null) ? "(null)"
                            : @this.ToString(format, formatProvider));
        }

        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }
    }
}
