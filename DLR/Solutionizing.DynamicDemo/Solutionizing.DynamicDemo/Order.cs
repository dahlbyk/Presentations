using System;

namespace Solutionizing.DynamicDemo
{
    public class Order
    {
        public int ItemCount { get; set; }
        public decimal TotalAmount { get; set; }

        public override string ToString()
        {
            return string.Format("{0} items for {1:C2}", ItemCount, TotalAmount);
        }
    }
}
