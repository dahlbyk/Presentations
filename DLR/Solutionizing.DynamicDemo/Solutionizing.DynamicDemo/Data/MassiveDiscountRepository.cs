using System;
using System.Collections.Generic;
using System.Linq;
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

        public void Save(IDiscountDefinition discount)
        {
            throw new NotSupportedException("Massive insert does not like SQL CE");

            // but it looks like this
            var table = new Discounts();

            table.Insert(new
            {
                discount.Code,
                discount.ExpirationDate,
                discount.ValidationScript,
                discount.ValidationScriptType,
            });
        }
    }
}
