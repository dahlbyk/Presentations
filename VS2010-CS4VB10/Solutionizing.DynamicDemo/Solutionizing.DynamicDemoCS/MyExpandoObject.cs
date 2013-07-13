using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;

namespace Solutionizing.DynamicDemoCS.More
{
    class MyExpandoObject : DynamicObject
    {
        private Dictionary<string, object> _data = new Dictionary<string, object>();

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return _data.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _data[binder.Name] = value;
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            dynamic target = _data[binder.Name];
            if (target == null)
                throw new InvalidOperationException("Could not invoke member.");
            switch (args.Length)
            {
                case 0: result = target();
                    return true;
                case 1: result = target(args[0]);
                    return true;
                default: throw new NotImplementedException();
            }
        }
    }
}
