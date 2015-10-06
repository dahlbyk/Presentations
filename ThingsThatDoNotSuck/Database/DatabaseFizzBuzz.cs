using System.Data.SqlClient;
using Interfaces;

namespace Database
{
    public class DatabaseFizzBuzz : IKnowFizzBuzz
    {
        public DatabaseFizzBuzz()
        {
            using (var conn = new SqlConnection("Server=.;Database=FizzBuzz;Integrated Security=True"))
            {
            }
        }

        public string Translate(int value)
        {
            return value.ToString();
        }
    }
}