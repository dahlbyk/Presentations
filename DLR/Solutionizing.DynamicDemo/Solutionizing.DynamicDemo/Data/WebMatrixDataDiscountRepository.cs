using System.Collections.Generic;
using System.Linq;
using WebMatrix.Data;

namespace Solutionizing.DynamicDemo.Data
{
    public class WebMatrixDataDiscountRepository : IDiscountRepository, IDiscountWriter
    {
        static readonly Database database = Database.Open("DynamicDemo");

        public Discount GetByCode(string code)
        {
            var sql = "SELECT TOP 1 * FROM Discounts WHERE Code = @0";

            var result = database.QuerySingle(sql, code);

            return new Discount(result);
        }

        public IEnumerable<Discount> GetAll()
        {
            var sql = "SELECT * FROM Discounts";

            var result = database.Query(sql);

            return result.Select(row => new Discount(row));
        }

        public void Save(IDiscountDefinition discount)
        {
            database.Execute(
                "INSERT INTO Discounts (Code, ValidationScript, ValidationScriptType, ExpirationDate) VALUES (@0, @1, @2, @3)",
                discount.Code, discount.ValidationScript, discount.ValidationScriptType, discount.ExpirationDate);
        }

        public void Delete(string code)
        {
            database.Execute("DELETE FROM Discounts WHERE Code = @0", code);
        }
    }
}
