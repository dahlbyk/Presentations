using System;
using ImpromptuInterface;
using IronPython.Hosting;
using IronRuby;
using Microsoft.Scripting.Hosting;

namespace Solutionizing.DynamicDemo
{
    public class Discount
    {
        public Discount(dynamic def)
            : this((IDiscountDefinition)Impromptu.ActLike<IDiscountDefinition>(def))
        {
        }

        public Discount(IDiscountDefinition discount)
        {
            Id = discount.Id;
            Code = discount.Code;
            ExpirationDate = discount.ExpirationDate;

            IsValid = GetValidatorFromScript(discount.ValidationScriptType, discount.ValidationScript);
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
                case "text/ruby":
                    return GetValidatorFromRuby(script);
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

        #region Ruby

        private static readonly ScriptEngine rubyEngine = Ruby.CreateEngine();

        private static Func<Order, bool> GetValidatorFromRuby(string script)
        {
            var scope = rubyEngine.CreateScope();
            rubyEngine.Execute(script, scope);
            return scope.GetVariable("isValid");
        }

        #endregion
    }
}
