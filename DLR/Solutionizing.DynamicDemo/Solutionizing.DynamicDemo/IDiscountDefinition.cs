using System;

namespace Solutionizing.DynamicDemo
{
    public interface IDiscountDefinition
    {
        int Id { get; }
        string Code { get; }
        DateTime? ExpirationDate { get; }
        string ValidationScript { get; }
        string ValidationScriptType { get; }
    }
}