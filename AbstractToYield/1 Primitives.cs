using System;

namespace AbstractToYield
{
    class Primitives
    {
        void ValueTypes()
        {
            bool @bool = true;

            // Integral
            sbyte @sbyte = SByte.MaxValue;          // 127
            byte @byte = Byte.MaxValue;             // 255
            char @char = Char.MaxValue;             // U+ffff
            short @short = Int16.MaxValue;          // 32,767S
            ushort @ushort = UInt16.MaxValue;       // 65,535US
            int @int = Int32.MaxValue;              // 2,147,483,647
            uint @uint = UInt32.MaxValue;           // 4,294,967,295U
            long @long = Int64.MaxValue;            // 9,223,372,036,854,775,807L
            ulong @ulong = UInt64.MaxValue;         // 18,446,744,073,709,551,615UL

            // Other Numeric
            float @float = Single.MaxValue;         // 3.4e38f
            double @double = Double.MaxValue;       // 1.7e308d
            decimal @decimal = Decimal.MaxValue;    // 7.9e28m

            var awesomeness = (Awesomeness)2;       // Lots
            var dwight = Likes.Bears | Likes.Beats | Likes.BattlestarGalactica;
            dwight.Dump();
        }
    }

    enum Awesomeness
    {
        None = 0,
        Some = 1,
        Lots = 2,
    }

    [Flags]
    enum Likes
    {
        Bears = 1,
        Beats = 2,
        BattlestarGalactica = 4,
    }
}
