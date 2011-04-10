using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Solutionizing.DynamicDemo.Data
{
    public abstract class XmlDiscountRepositoryBase : IDiscountRepository
    {
        protected readonly Dictionary<string, Discount> discounts;

        public XmlDiscountRepositoryBase(XDocument doc)
        {
            discounts = GetDiscountsByCode(doc).ToDictionary(d => d.Code);
        }

        protected abstract IEnumerable<Discount> GetDiscountsByCode(XDocument doc);

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
