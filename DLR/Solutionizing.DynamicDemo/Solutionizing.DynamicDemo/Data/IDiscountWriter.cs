using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solutionizing.DynamicDemo.Data
{
    public interface IDiscountWriter
    {
        void Save(IDiscountDefinition discount);
    }
}
