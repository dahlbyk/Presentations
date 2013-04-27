using System;
using System.Collections;
using C = System.Console;

namespace AbstractToYield
{
    public delegate bool TryParser<T>(string s, out T result);

    interface IParseable<out T>
    {
        T Parse(string s);
    }

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

    static class CannotBeInstantiated
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
                C.WriteLine(ReferenceEquals(@this, null) ? "(null)" : @this.ToString());
            }
        }

        public static void Dump<T>(this T @this, string format, IFormatProvider formatProvider = null) where T : IFormattable
        {
            C.WriteLine(ReferenceEquals(@this, null) ? "(null)" : @this.ToString(format, formatProvider));
        }

        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        public static T? ParseOrDefault<T>(
            this string s, TryParser<T> parser) where T : struct
        {
            T result;
            return parser(s, out result) ? result : default(T?);
        }
    }

    sealed internal class CannotBeExtended { }

    abstract class MustBeExtended
    {
        private readonly int value;
        private readonly string notOverridenMessage;

        protected MustBeExtended(int value)
        {
            this.value = value;
            notOverridenMessage = "Not overriden: " + value;
        }

        protected abstract string MustBeImplemented { get; }

        protected virtual void MayBeOverriden()
        {
            notOverridenMessage.Dump();
        }

        protected virtual int this[int i]
        {
            get { return value * i; }
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

        void Test()
        {
            this.Dump();

            MayBeOverriden();
            MustBeImplemented.Dump();

            var changeMeMaybe = "before";
            CanHazARef(ref changeMeMaybe);
            changeMeMaybe.Dump();

            "".Dump();
            ParamsExample("foo", "bar", "baz");

            DynamicLength(new[] { 1, 2, this[3] });
        }

        public new string ToString()
        {
            return "I am CanBeExtended, hear me rawr!";
        }

        protected override string MustBeImplemented
        {
            get { return "An implementation!"; }
        }

        public string this[string name, int i]
        {
            get { return "{0} asked for {1}.".FormatWith(name, i); }
            set { "{0} tried to set {1} to {2}.".FormatWith(name, i, value).Dump(); }
        }

        public void CanHazARef(ref string thing)
        {
            // Don't have to write to thing
        }

        public void CanHazAnOut(out string thing)
        {
            thing = "Compile error if I don't set thing";
        }

        public void ParamsExample(params string[] listOfThings)
        {
            "I like all these things:".Dump();
            listOfThings.Dump();
        }

        public void DynamicLength(dynamic thingWithLength)
        {
            C.WriteLine(string.Format("Length of {0} = {1}", thingWithLength, thingWithLength.Length));
        }
    }

    public partial struct Money : IParseable<Money?>
    {
        public static readonly Money Zero = default(Money);

        private readonly decimal value;

        private Money(decimal value)
        {
            this.value = value;
            OnInit();
        }

        partial void OnInit();

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is Money && Equals((Money)obj);
        }

        public override string ToString()
        {
            return ToString(null, null);
        }

        public Money? Parse(string s)
        {
            return s.ParseOrDefault<decimal>(decimal.TryParse);
        }
    }

    public partial struct Money : IEquatable<Money>, IComparable<Money>, IFormattable
    {
        #region IEquatable<Money>

        public bool Equals(Money other)
        {
            return value == other.value;
        }

        #endregion

        #region IComparable<Money>

        public int CompareTo(Money other)
        {
            return value.CompareTo(other.value);
        }

        #endregion

        #region IFormattable

        public string ToString(string format)
        {
            return ToString(format, null);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return value.ToString(format ?? "F2", formatProvider);
        }

        #endregion
    }

}
