using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using Dapper;

namespace Solutionizing.DynamicDemo.Data
{
    public class DapperDiscountRepository : IDiscountRepository, IDiscountWriter
    {
        public Discount GetByCode(string code)
        {
            using (var conn = OpenConnection())
                return conn.Query("SELECT * FROM Discounts WHERE Code = @code", new { code }).Select(row => new Discount(row)).First();
        }

        public IEnumerable<Discount> GetAll()
        {
            using (var conn = OpenConnection())
                return conn.Query("SELECT * FROM Discounts").Select(row => new Discount(row));
        }

        public void Save(IDiscountDefinition discount)
        {
            using (var conn = OpenConnection())
                conn.Execute(
                    "INSERT INTO Discounts (Code, ValidationScript, ValidationScriptType, ExpirationDate) VALUES (@Code, @ValidationScript, @ValidationScriptType, @ExpirationDate)",
                    discount);
        }

        private static IDbConnection OpenConnection()
        {
            var css = ConfigurationManager.ConnectionStrings["DynamicDemo"];
            var factory = DbProviderFactories.GetFactory(css.ProviderName);

            var conn = factory.CreateConnection();
            conn.ConnectionString = css.ConnectionString;
            conn.Open();
            return conn;
        }
    }
}