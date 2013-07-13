using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Xml.Linq;

namespace Solutionizing.DynamicDemo.Xml
{
    public class DXElement : DynamicObject
    {
        readonly XElement element;

        public DXElement(XElement element)
        {
            this.element = element;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return element == null ? Enumerable.Empty<string>() :
                from e in element.Elements()
                select e.Name.LocalName;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = element == null ? null : element.Element(binder.Name).AsDynamic();
            return result != null;
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            var mapper = typeMap.GetOrDefault(binder.Type);
            if (mapper == null)
                return base.TryConvert(binder, out result);

            result = mapper(element);
            return true;
        }

        static readonly Dictionary<Type, Func<XElement, object>> typeMap = new Dictionary<Type,Func<XElement,object>>
        {
            { typeof(XElement), e => e },
            { typeof(string), e => (string)e },
            { typeof(DateTime), e => (DateTime)e },
            { typeof(DateTime?), e => (DateTime?)e },
            { typeof(int), e => (int)e },
            { typeof(int?), e => (int?)e }
        };
    }
}