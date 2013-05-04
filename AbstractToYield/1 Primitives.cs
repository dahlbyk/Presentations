using System;
using System.Diagnostics;

namespace AbstractToYield
{
    /*  Things you probably never need
     *
     *  unsafe                      // Member modifier
     *  unsafe { ... }
     *  stackalloc
     *  fixed (...) { ... }
     */

    class Primitives
    {
        void BuildInTypes()
        {
            Type @void = typeof(void);
            // void aVoid = default(void);

            object @object = null;
            string @string = "string";

            // Pointers!
            IntPtr ptr = IntPtr.Zero;
            UIntPtr uptr = UIntPtr.Zero;

            // Boolean
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

            // Enum
            var chocolatey = Awesomeness.Lots;
            chocolatey.Dump();

            var keith = Likes.Bears | Likes.BattlestarGalactica;
            keith.Dump();

            // Inferred vs Dynamic
            var inferMePlz = keith.ToString().Split(',');
            dynamic figureMeOutLater = inferMePlz;
            Console.WriteLine(figureMeOutLater.Length);
        }
    }

    enum Awesomeness
    {
        None,
        Some = 2,
        Lots,
    }

    [Flags]
    enum Likes
    {
        Bears = 1,
        Beets = 2,
        BattlestarGalactica = 4,
        Dwight = Bears | Beets | BattlestarGalactica,
    }
}
