using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Solutionizing.DynamicDemo
{
    class MyExpandoObject : DynamicObject
    {
        private readonly Dictionary<string, object> data = new Dictionary<string, object>();

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return data.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            data[binder.Name] = value;
            return true;
        }
    }
}
