using System;
using System.Globalization;
using C = System.Console;

namespace AbstractToYield
{
    /*  Primary
     *
     *  // overloadable
     *  x++
     *  x--
     *
     *  x.y
     *  f(x)
     *  a[x]
     *  new
     *  typeof
     *  checked
     *  unchecked
     *  default(T)
     *  delegate { … }
     *  sizeof
     *  ->              // unsafe: equivalent to (*x).y
     */

    /*  Unary
     *
     *  // overloadable
     *  +x
     *  -x
     *  !x
     *  ~x
     *  ++x
     *  --x
     *
     *  (T)x
     *  await
     *  &x            // unsafe: address of operand
     *  *x            // unsafe: declare and dereference pointer
     */

    /*  Multiplicative
     *
     *  // overloadable
     *  x * y
     *  x / y
     *  x % y
     */

    /*  Additive
     *
     *  // overloadable
     *  x + y
     *  x - y
     */

    /*  Shift
     *
     *  // overloadable
     *  x >> y
     *  x << y
     */

    /*  Relational and type testing
     *
     *  // overloadable
     *  x < y
     *  x > y
     *  x <= y
     *  x >= y
     *
     *  is
     *  as
     */

    /*  Equality
     *
     *  // overloadable
     *  x == y
     *  x != y
     */

    /*  Logical
     *  // in order of precedence
     *
     *  // overloadable
     *  x & y
     *  x ^ y
     *  x | y
     */

    /*  Conditional (short-circuit)
     *  // in order of precedence
     *
     *  x && y
     *  x || y
     */

    /*  Null-coalescing and Conditional
     *  // in order of precedence
     *
     *  x ?? y
     *  b ? t : f
     */

    /*  Assignment and lambda expression
     *  x = y
     *
     *  x += y
     *  x -= y
     *  x *= y
     *  x /= y
     *  x %= y
     *  x &= y
     *  x |= y
     *  x ^= y
     *  x <<= y
     *  x >>= y
     *
     *  x => y
     *  x => { … }
     *  () => …
     *  (x) => …
     *  (x, y) => …
     *  (T x, U y) => …
     */

    /*  Misc Operators
     *  ::              // Namespace alias qualifier
     *                  // global: http://msdn.microsoft.com/en-us/library/c3ay4x3d.aspx
     *                  // extern alias: http://msdn.microsoft.com/en-us/library/ms173212.aspx
     *
     *  true, false     // Overload to indicate operand is true/false
     *                  // Type can be used in conditional expressions and if/do/while/for statements
     *                  // http://msdn.microsoft.com/en-us/library/6x6y6z4d.aspx
     */

    /*  Visual Basic
     * 
     *  Operator Statement: http://msdn.microsoft.com/en-us/library/hddt295a.aspx
     *                      Narrowing/Widening vs explicit/implicit (slightly different semantics)
     *
     *  Operator differences:
     *  VB                  C#
     *  Mod                 %
     *  Not                 ! or ~      // Both boolean and bitwise negation
     *  And                 &           // Logical or bitwise "conjunction"
     *  Or                  |           // Logical or bitwise "inclusive disjunction"
     *  Xor                 ^           // Logical or bitwise "exclusive disjunction"
     *  IsTrue              true
     *  IsFalse             false
     *  AndAlso             &&
     *  OrElse              ||
     *
     *  If(x,y)             x ?? y
     *  If(b,t,f)           b ? t : f
     *
     *  ^                               // Exponentiation via Math.Pow()
     *  \                               // Integer division, overloadable
     *  &                   +           // String concatenation, with automatic conversion
     *  Like                            // String pattern match
     *  Function            =>          // Lambda expression returning value
     *  Sub                 =>          // Lambda expression returning void
     *
     *  DirectCast(x, T)    (T)x        // x must inherit from or implement T
     *  TryCast(x, T)       x as T
     *  TypeOf x Is T       x is T
     *
     *  AddressOf                       // Reference to procedure
     *  GetType             typeof
     *  Is                              // Reference equality
     *  IsNot                           // Reference inequality
     *  GetXmlNamespace                 // http://msdn.microsoft.com/en-us/library/bb385056.aspx
     */

    public class Operators
    {
        private delegate decimal Halver(int value);

        public void Lambda()
        {
            Halver h = delegate(int value) { return value / 2m; };
            h(3).Dump();
        }

        public void Sandbox()
        {
            Money m1 = 15;
            Money m2 = -17.5m;
            Money m3 = -20;

            var sum = m1 + m2 + +m3;
            sum.Dump();

            sum.Dump("C", CultureInfo.InvariantCulture);
        }
    }

    public partial struct Money
    {
        public static readonly Money Zero = default(Money);

        private readonly decimal value;

        private Money(decimal value)
        {
            this.value = value;
        }

        #region Conversion (Unary)

        public static implicit operator Money(decimal d)
        {
            return new Money(d);
        }

        public static implicit operator Money?(decimal? d)
        {
            return d == null ? default(Money?) : new Money(d.Value);
        }

        public static explicit operator decimal(Money m)
        {
            return m.value;
        }

        public static explicit operator decimal?(Money? m)
        {
            return m == null ? default(decimal?) : m.Value.value;
        }

        #endregion

        #region Unary

        public static Money operator +(Money m)
        {
            return Math.Max(m.value, 0m);
        }

        public static Money operator -(Money m)
        {
            return -m.value;
        }

        public static Money operator ++(Money m)
        {
            return m.value + 1;
        }

        public static Money operator --(Money m)
        {
            return m - 1;
        }

        #endregion

        #region Multiplicative

        public static Money operator *(Money m1, decimal m2)
        {
            return m1.value * m2;
        }

        public static Money operator /(Money m1, decimal m2)
        {
            return m1.value / m2;
        }

        public static Money operator %(Money m1, decimal m2)
        {
            return m1.value % m2;
        }

        #endregion

        #region Additive

        public static Money operator +(Money m1, Money m2)
        {
            return m1.value + m2.value;
        }

        public static Money operator -(Money m1, Money m2)
        {
            return m1.value - m2.value;
        }

        #endregion

        #region Relational

        public static bool operator >(Money m1, Money m2)
        {
            return m1.value > m2.value;
        }

        public static bool operator <(Money m1, Money m2)
        {
            return m1.value < m2.value;
        }

        public static bool operator >=(Money m1, Money m2)
        {
            return m1.value >= m2.value;
        }

        public static bool operator <=(Money m1, Money m2)
        {
            return m1.value <= m2.value;
        }

        #endregion

        #region Equality

        public static bool operator ==(Money m1, Money m2)
        {
            return m1.value == m2.value;
        }

        public static bool operator !=(Money m1, Money m2)
        {
            return m1.value != m2.value;
        }

        #endregion
    }
}
