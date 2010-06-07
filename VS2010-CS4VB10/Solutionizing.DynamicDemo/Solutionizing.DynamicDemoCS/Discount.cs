using System;
using IronPython.Hosting;

namespace Solutionizing.DynamicDemoCS
{
    public class Discount
    {
        public Discount(dynamic discount)
        {
            Code = discount.Code;

            var engine = Python.CreateEngine();
            var scope = engine.CreateScope();
            engine.Execute(discount.ValidationScript, scope);
            IsValid = scope.GetVariable("isValid");
        }

        public string Code { get; private set; }

        public Func<Order, bool> IsValid { get; private set; }
    }
}
