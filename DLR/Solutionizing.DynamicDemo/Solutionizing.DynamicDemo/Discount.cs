using System;
using IronJS;
using IronJS.Hosting;
using IronPython.Hosting;
using IronRuby;
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
                case "text/javascript":
                    return GetValidatorFromJavaScript(script);
                case "text/python":
                    return GetValidatorFromPython(script);
                case "text/ruby":
                    return GetValidatorFromRuby(script);
                default:
                    throw new InvalidOperationException("I do not speak " + scriptType);
            }
        }

        #region JavaScript

        private static Func<Order, bool> GetValidatorFromJavaScript(string script)
        {
            var ctx = new CSharp.Context();
            ctx.Execute(script);

            var function = ctx.GetGlobalAs<FunctionObject>("isValid");
            var @delegate = function.MetaData.GetDelegate<Func<FunctionObject, CommonObject, Order, BoxedValue>>(function);
            return o => (bool)@delegate.Invoke(function, ctx.Globals, o).ClrBoxed;

            // Easier in vNext:
            // return ctx.GetFunctionAs<Func<Order, bool>>("isValid");
        }

        #endregion

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
