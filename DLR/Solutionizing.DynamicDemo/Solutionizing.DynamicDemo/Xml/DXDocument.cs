using System;
using System.Xml.Linq;

namespace Solutionizing.DynamicDemo.Xml
{
    public class DXDocument : DXContainer<XDocument>
    {
        public DXDocument(XDocument element)
            : base(element)
        {
        }

        protected override Func<XDocument, object> GetConverterForType(Type type)
        {
            return null;
        }
    }
}
