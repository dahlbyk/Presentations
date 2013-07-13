using System;
using ImpromptuInterface;
using Solutionizing.DynamicDemo.Data;

namespace Solutionizing.DynamicDemo
{
    static internal class DiscountExtensions
    {
        public static void Dump(this Discount discount)
        {
            Console.WriteLine("Discount {0}:", discount.Code);
            foreach (var order in orders)
                Console.WriteLine("Valid for order {0}?\t{1}", order,
                    discount.IsValid(order) ? "Yes" : "No");
            Console.WriteLine();
        }

        public static void DumpAll(this IDiscountRepository repo)
        {
            foreach (var discount in repo.GetAll())
                discount.Dump();
        }

        public static void Test(this IDiscountWriter writer, string code, int expectedItems)
        {
            writer.Save(new
            {
                Code = code,
                ValidationScript = "def isValid(order): return order.ItemCount == " + expectedItems,
                ValidationScriptType = "text/python",
                ExpirationDate = DateTime.Today.AddYears(1),
            }.ActLike<IDiscountDefinition>());
        }

        private static readonly Order[] orders = new[]
        {
            new Order { ItemCount = 2, TotalAmount = 15.0m },
            new Order { ItemCount = 7, TotalAmount = 7.0m },
            new Order { ItemCount = 10, TotalAmount = 2.0m },
        };
    }
}