using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace Aspor.Validation.Rules
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RuleValidationAttribute : Attribute, IAsyncRuleValidation
    {

        private readonly object _validation;

        public RuleValidationAttribute(Type validationType)
        {
            _validation = Activator.CreateInstance(validationType);
        }

        public async Task Validate(IServiceProvider services, ModelStateDictionary state, object instance)
        {
            if (_validation is IAsyncRuleValidation) await ((IAsyncRuleValidation)_validation).Validate(services, state, instance);
            else if (_validation is IRuleValidation) ((IRuleValidation)_validation).Validate(services, state, instance);
            else throw new InvalidOperationException(_validation.GetType() + " is not a valid validation class");
        }
    }
}
