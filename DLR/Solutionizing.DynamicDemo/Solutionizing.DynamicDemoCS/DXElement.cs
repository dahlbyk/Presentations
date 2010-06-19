using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Xml.Linq;

namespace Solutionizing.DynamicDemoCS
{
    public class DXElement : DynamicObject
    {
        private readonly XElement element;

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
            if (binder.Type == typeof(string))
                result = (string)element;
            else if (binder.Type == typeof(int))
                result = (int)element;
            else
            {
                result = null;
                return false;
            }

            return true;
        }
    }

    public static class DXExtensions
    {
        public static dynamic AsDynamic(this XElement @this)
        {
            return new DXElement(@this);
        }
    }
}
