using System;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace Solutionizing.DynamicDemo
{
    public class Discount
    {
        public Discount(dynamic discount)
        {
            Id = discount.Id;
            Code = discount.Code;
            ExpirationDate = discount.ExpirationDate;

            IsValid = GetValidatorFromScript((string)discount.ValidationScriptType, (string)discount.ValidationScript);
        }

        public int Id { get; private set; }

        public string Code { get; private set; }

        public Func<Order, bool> IsValid { get; private set; }

        public DateTime? ExpirationDate { get; private set; }

        private static Func<Order, bool> GetValidatorFromScript(string scriptType, string script)
        {
            switch (scriptType)
            {
                case "text/python":
                    return GetValidatorFromPython(script);
                default:
                    throw new InvalidOperationException("I do not speak " + scriptType);
            }
        }

        #region Python

        private static readonly ScriptEngine pythonEngine = Python.CreateEngine();

        private static Func<Order, bool> GetValidatorFromPython(string script)
        {
            var scope = pythonEngine.CreateScope();
            pythonEngine.Execute(script, scope);
            return scope.GetVariable("isValid");
        }

        #endregion
    }
}
