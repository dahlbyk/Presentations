using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Solutionizing.Dynamic.Xml;

namespace Solutionizing.DynamicDemoCS
{
    public class DiscountRepository
    {
        private readonly Dictionary<string, Discount> discounts;

        public DiscountRepository(XDocument doc)
        {
            this.discounts = (
                from d in doc.Element("Discounts").Elements("Discount")
                //select new Discount(
                //    new
                //    {
                //        Id = (int)d.Element("Id"),
                //        Code = (string)d.Element("Code"),
                //        ExpirationDate = (DateTime?)d.Element("ExpirationDate"),
                //        ValidationScript = (string)d.Element("ValidationScript")
                //    })
                select new Discount(d.AsDynamic())
                ).ToDictionary(d => d.Code);
        }

        public Discount GetByCode(string code)
        {
            return discounts[code];
        }

        public IEnumerable<Discount> GetAll()
        {
            return discounts.Select(kvp => kvp.Value);
        }
    }
}
