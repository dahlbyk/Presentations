using System;
using IronPython.Hosting;

namespace Solutionizing.DynamicDemoCS
{
    public class Discount
    {
        private readonly string script;

        public Discount(dynamic discount)
        {
            Id = discount.Id;
            Code = discount.Code;
            script = discount.ValidationScript;

            var engine = Python.CreateEngine();
            var scope = engine.CreateScope();
            engine.Execute(script, scope);
            IsValid = scope.GetVariable("isValid");
        }

        public int Id { get; private set; }

        public string Code { get; private set; }

        public Func<Order, bool> IsValid { get; private set; }
    }
}
