using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using C = System.Console;

namespace AbstractToYield
{
    /*  Accessibility
     *
     *  private
     *  protected
     *  internal
     *  public
     *
     *  protected internal
     *
     *  Defaults
     *
     *  class           internal
     *  nested class    private
     *  member          private
     */

    class MyClass
    {
        string myField;

        public MyClass(string myParam)
        {
            this.myField = myParam ?? "";
        }

        MyClass()
            : this(null)
        {
        }

        ~MyClass()
        {
            C.WriteLine("Finish him!");
        }

        public void MyMethod() { Debug.Assert(myField != null, "myField is null!"); }

        public double MyProperty { get; set; }

        public event EventHandler MyEvent;

        public char this[int i]
        {
            get { return myField[i]; }
            private set
            {
                var copy = new char[myField.Length];
                myField.CopyTo(0, copy, 0, myField.Length);
                copy[i] = value;
                myField = new string(copy);
            }
        }

        public static MyClass Build(object field = null, double prop = 0)
        {
            return new MyClass
            {
                myField = field == null ? null : field.ToString(),
                MyProperty = prop,
            };
        }

        static void Test()
        {
            var c = Build(prop: MyStatic.TheAnswerToLifeTheUniverseAndEverything);
            c.MyMethod();
        }
    }

    static partial class MyStatic
    {
        public const int TheAnswerToLifeTheUniverseAndEverything = 42;
        public static readonly double PiInIndiana = GetPi();

        static MyStatic()
        {
            C.WriteLine("Welcome to Indiana, where pi is {0}.", PiInIndiana);
        }

        static partial void PrePiCalculation();
        static partial void PostPiCalculation(double pi);

        private static double GetPi()
        {
            PrePiCalculation();
            const int pi = 3;
            PostPiCalculation(pi);
            return pi;
        }

        public static NestedClass MyProperty { get; set; }

        internal class NestedClass
        {
        }
    }

    static partial class MyStatic
    {
        static partial void PrePiCalculation()
        {
            C.WriteLine("About to calculate pi...");
        }
    }

    public partial struct Money : IEquatable<Money>, IComparable<Money>, IFormattable
    {
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is Money && Equals((Money)obj);
        }

        #region IEquatable<Money>

        public bool Equals(Money other)
        {
            return value == other.value;
        }

        #endregion

        #region IComparable<Money>

        int IComparable<Money>.CompareTo(Money other)
        {
            return value.CompareTo(other.value);
        }

        #endregion

        public override string ToString()
        {
            return ToString(null, null);
        }

        #region IFormattable

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return value.ToString(format ?? "F2", formatProvider);
        }

        #endregion
    }

    interface IAmAnInterface<in T, out U>
    {
        void MyMethod(T value);
        bool MyProperty { get; set; }
        U MyReadOnlyProperty { get; }
        event EventHandler MyEvent;
    }

    public delegate bool TryParser<T>(string s, out T result);

    // delegate void Action();
    // delegate void Action<in T>(T arg);
    // delegate void Action<in T1, in T2>(T1 arg1, T2 arg2);
    // delegate void Action<in T1, in T2, …>(T1 arg1, T2 arg2, …);

    // delegate R Func<out R>();
    // delegate R Func<in T, out R>(T arg);
    // delegate R Func<in T1, in T2, out R>(T1 arg1, T2 arg2);
    // delegate R Func<in T1, in T2, …, out R>(T1 arg1, T2 arg2, …);

    class Sandbox
    {
        void Generic_variance()
        {
            Func<string> f = () => "hi";
            Func<object> g = f;
            g().Dump();

            Action<object> a = o => o.Dump();
            Action<string> b = a;
            b("hi");

            Func<object, string> x = o => o.ToString();
            Func<string, string> y = x;
            Func<object, object> z = x;
            y("pi").Dump();
            z("pi2").Dump();
        }
    }

    static class MyExtensions
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

        public static string ToString<T>(this T @this, string format)
            where T : IFormattable
        {
            return @this.ToString(format, null);
        }

        public static T? ParseOrDefault<T>(
            this string s, TryParser<T> parser) where T : struct
        {
            T result;
            return parser(s, out result) ? result : default(T?);
        }
    }

    #region Inheritance

    sealed class CannotBeExtended { }

    abstract class MustBeExtended
    {
        private readonly int value;
        protected readonly string notOverridenMessage;

        protected MustBeExtended(int value)
        {
            this.value = value;
            notOverridenMessage = "Not overriden: " + value;
        }

        public abstract override int GetHashCode();

        protected virtual void MayBeOverriden()
        {
            notOverridenMessage.Dump();
        }
    }

    class CanBeExtended : MustBeExtended
    {
        public CanBeExtended()
            : this(5)
        {
        }

        public CanBeExtended(int value)
            : base(value)
        {
        }

        public override int GetHashCode()
        {
            return 42;
        }

        public void CanHazARef(ref string thing)
        {
            // Don't have to write to thing
        }

        public void CanHazAnOut(out string thing)
        {
            thing = "Compile error if I don't set thing";
        }

        void Test()
        {
            this.Dump();
            this.ToString().Dump();

            MayBeOverriden();

            var changeMeMaybe = "before";
            CanHazARef(ref changeMeMaybe);
            changeMeMaybe.Dump();
        }

        public new string ToString()
        {
            return "I am CanBeExtended, hear me rawr!";
        }
    }

    #endregion

    #region Generics

    static class MyStatic<TClass, TStruct, TNewComparable, TEnumerable>
        where TClass : class
        where TStruct : struct
        where TNewComparable : IComparable, new()
        where TEnumerable : IEnumerable<TNewComparable>
    {
        private static readonly HashSet<Type> types = BuildTypeSet();

        private static HashSet<Type> BuildTypeSet()
        {
            Console.WriteLine("Building type set for {0}...", typeof(TClass));
            return new HashSet<Type>();
        }

        public static void Touch<T>()
        {
            types.Add(typeof (T));
            Console.WriteLine("Current type sets...");
            types.Dump();
        }

        public static TNewComparable GetFirstGreaterThan(TEnumerable comparables, TNewComparable other)
        {
            return comparables.FirstOrDefault(c => c.CompareTo(other) > 0);
        }
    }

    class GenericSandbox
    {
        void Sandbox()
        {
            MyStatic<string, int, decimal, IEnumerable<decimal>>.Touch<bool>();
            MyStatic<string, int, decimal, IEnumerable<decimal>>.Touch<int>();
        }
    }

    #endregion
}
