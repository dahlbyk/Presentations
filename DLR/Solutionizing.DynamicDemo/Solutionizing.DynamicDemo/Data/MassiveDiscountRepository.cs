using System;
using System.Collections.Generic;
using System.Linq;
using WebMatrix.Data;
using Massive;

namespace Solutionizing.DynamicDemo.Data
{
    public class MassiveDiscountRepository : IDiscountRepository, IDiscountWriter
    {
        public class Discounts : DynamicModel
        {
            public Discounts()
                : base("DynamicDemo")
            {
                PrimaryKeyField = "Id";
            }
        }

        public Discount GetByCode(string code)
        {
            var table = new Discounts();

            var result = table.All(where: "WHERE Code = @0", limit: 1, args: code);

            return new Discount(result);
        }

        public IEnumerable<Discount> GetAll()
        {
            var table = new Discounts();

            var result = table.All();

            return result.Select(row => new Discount(row));
        }

        public void Save(dynamic discount)
        {
            var table = new Discounts();
            table.Insert(new {
                Code = (string)discount.Code,
                ValidationScript = (string)discount.ValidationScript,
                ExpirationDate = (DateTime?)discount.ExpirationDate,
            });
        }
    }
}
