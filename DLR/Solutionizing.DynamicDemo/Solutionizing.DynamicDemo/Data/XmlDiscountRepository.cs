using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Solutionizing.DynamicDemo.Data
{
    public class XmlDiscountRepository : XmlDiscountRepositoryBase
    {
        public XmlDiscountRepository(XDocument doc)
            : base(doc)
        {
        }

        protected override IEnumerable<Discount> GetDiscountsByCode(XDocument doc)
        {
            return from d in doc.Element("Discounts").Elements("Discount")
                   select new Discount(
                       new
                       {
                           Id = (int)d.Element("Id"),
                           Code = (string)d.Element("Code"),
                           ExpirationDate = (DateTime?)d.Element("ExpirationDate"),
                           ValidationScript = (string)d.Element("ValidationScript")
                       });
        }
    }
}
