using Aspor.Validation.Rules;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.OData.Deltas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aspor.Validation.Definition
{
    public class ValidationDefinition
    {

        public ValidationDefinition(IList<ValidationPropertyDefinition> properties, IList<object> ruleValidations)
        {
            Properties = properties;
            RuleValidations = ruleValidations;
        }

        public IList<ValidationPropertyDefinition> Properties { get; }

        public IList<object> RuleValidations { get; }

        public void ValidateProperty(ModelStateDictionary state, string propertyName, object? instance, ValidationAction action = ValidationAction.CREATE)
        {
            ValidationPropertyDefinition property = Properties.First(p => p.Name.Equals(propertyName, StringComparison.CurrentCultureIgnoreCase));
            var input = property.InstanceGetMethod.Invoke(instance, new object[0]);
            property.Validate(state, action, input);
        }

        public void ValidatePropertyValue(ModelStateDictionary state, string propertyName, object input, ValidationAction action = ValidationAction.CREATE)
        {
            ValidationPropertyDefinition property = Properties.First(p => p.Name.Equals(propertyName, StringComparison.CurrentCultureIgnoreCase));
            property.Validate(state, action, input);
        }

        public void ValidateModelInstance(ModelStateDictionary state, object instance, ValidationAction action = ValidationAction.CREATE)
        {
            foreach (ValidationPropertyDefinition property in Properties)
            {
                var input = property.InstanceGetMethod.Invoke(instance, new object[0]);
                property.Validate(state, action, input);
            }
        }

        public void ValidateModelDelta(ModelStateDictionary state, Delta delta, ValidationAction action = ValidationAction.CREATE)
        {
            foreach (string propertyName in delta.GetChangedPropertyNames())
            {
                ValidationPropertyDefinition property = Properties.First(p => p.Name.Equals(propertyName, StringComparison.CurrentCultureIgnoreCase));
                object? input;
                if (delta.TryGetPropertyValue(property.Name, out input))
                {
                    property.Validate(state, action, input);
                }
            }
        }

        public void ValidateRules(IServiceProvider services, ModelStateDictionary state, object instance)
        {
            foreach (object validation in RuleValidations)
            {
                if (validation is IRuleValidation) ((IRuleValidation)validation).Validate(services, state, instance);
                else if (validation is IAsyncRuleValidation) ((IAsyncRuleValidation)validation).Validate(services, state, instance).Wait();
            }
        }

        public async Task ValidateRulesAsync(IServiceProvider services, ModelStateDictionary state, object instance)
        {
            foreach (object validation in RuleValidations)
            {
                if (validation is IAsyncRuleValidation) await ((IAsyncRuleValidation)validation).Validate(services, state, instance);
                else if (validation is IRuleValidation) ((IRuleValidation)validation).Validate(services, state, instance);
            }
        }
    }
}
