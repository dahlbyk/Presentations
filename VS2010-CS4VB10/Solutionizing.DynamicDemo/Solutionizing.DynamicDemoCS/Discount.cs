using System;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace Solutionizing.DynamicDemoCS
{
    public class Discount
    {
        public Discount(dynamic discount)
        {
            Id = discount.Id;
            Code = discount.Code;
            ExpirationDate = discount.ExpirationDate;

            IsValid = GetVariableFromPython((string)discount.ValidationScript, "isValid");
        }

        public int Id { get; private set; }

        public string Code { get; private set; }

        public Func<Order, bool> IsValid { get; private set; }

        public DateTime? ExpirationDate { get; private set; }

        #region Python

        private static ScriptEngine pythonEngine = Python.CreateEngine();

        private static dynamic GetVariableFromPython(string script, string name)
        {
            var scope = pythonEngine.CreateScope();
            pythonEngine.Execute(script, scope);
            return scope.GetVariable(name);
        }

        #endregion
    }
}
