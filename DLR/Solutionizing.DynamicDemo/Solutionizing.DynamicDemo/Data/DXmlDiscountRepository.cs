using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Solutionizing.DynamicDemo.Xml;

namespace Solutionizing.DynamicDemo.Data
{
    public class DXmlDiscountRepository : XmlDiscountRepositoryBase
    {
        public DXmlDiscountRepository(XDocument doc)
            : base(doc)
        {
        }

        protected override IEnumerable<Discount> GetDiscounts(XDocument doc)
        {
            return from d in doc.Root.Elements("Discount")
                   select new Discount(d.AsDynamic());
        }
    }
}
