using System;
using System.Dynamic;

namespace AbstractToYield
{
    class Dynamic
    {
        void Demo()
        {
            var value = GetValue();

            int length = GetLength(value);

            length.Dump();
        }

        static dynamic GetValue()
        {
            var foo = CustomThingWithLength();

            return foo;
        }

        static int GetLength(dynamic value)
        {
            return value.Length;
        }

        static dynamic ThingWithLength()
        {
            dynamic expando = new ExpandoObject();
            expando.Length = 42;
            return expando;
        }

        static dynamic CustomThingWithLength()
        {
            return new CustomThing();
        }
    }

    class CustomThing : DynamicObject
    {
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (binder.Name == "Length")
            {
                result = DateTime.Now.Second;
                return true;
            }

            return base.TryGetMember(binder, out result);
        }
    }
}