using System.Collections.Generic;

namespace Solutionizing.DynamicDemo.Data
{
    public interface IDiscountRepository
    {
        Discount GetByCode(string code);
        IEnumerable<Discount> GetAll();
    }
}
