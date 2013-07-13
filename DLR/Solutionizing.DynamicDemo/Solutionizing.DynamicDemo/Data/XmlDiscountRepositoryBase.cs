using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Solutionizing.DynamicDemo.Data
{
    public abstract class XmlDiscountRepositoryBase : IDiscountRepository
    {
        protected readonly Dictionary<string, Discount> discounts;

        protected XmlDiscountRepositoryBase(XDocument doc)
        {
            discounts = GetDiscounts(doc).ToDictionary(d => d.Code);
        }

        protected abstract IEnumerable<Discount> GetDiscounts(XDocument doc);

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
