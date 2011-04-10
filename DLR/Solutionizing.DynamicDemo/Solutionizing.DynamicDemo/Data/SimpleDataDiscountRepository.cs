using System;
using System.Collections.Generic;
using System.Linq;
using Simple.Data;

namespace Solutionizing.DynamicDemo.Data
{
    public class SimpleDataDiscountRepository : IDiscountRepository, IDiscountWriter
    {
        readonly dynamic database = Database.Open();

        public Discount GetByCode(string code)
        {
            var result = database.Discounts.FindByCode(code);

            return new Discount(result);
        }

        public IEnumerable<Discount> GetAll()
        {
            IEnumerable<dynamic> result = database.Discounts.FindAll();

            return result.Select(row => new Discount(row));
        }

        public void Save(dynamic discount)
        {
            database.Discounts.Insert(
                Code: (string)discount.Code,
                ValidationScript: (string)discount.ValidationScript,
                ExpirationDate: (DateTime)discount.ExpirationDate
            );
        }
    }
}
