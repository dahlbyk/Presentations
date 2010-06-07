using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Solutionizing.DynamicDemoCS
{
    public class DiscountRepository
    {
        private readonly Dictionary<string, Discount> discounts;

        public DiscountRepository(XDocument doc)
        {
            this.discounts = (
                from d in doc.Element("Discounts").Elements("Discount")
                select new Discount(
                    new
                    {
                        Code = (string)d.Element("Code"),
                        ValidationScript = (string)d.Element("ValidationScript")
                    })
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
