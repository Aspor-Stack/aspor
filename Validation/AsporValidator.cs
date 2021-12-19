using Aspor.Validation.Definition;
using Aspor.Validation.Rules;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.OData.Deltas;
using System;
using System.Threading.Tasks;

namespace Aspor.Validation
{
    public static class AsporValidator
    {

        public static void ValidateModel(ModelStateDictionary state, ValidationAction action, object instance)
        {
            Delta delta = instance as Delta;
            if (delta != null)
            {
                ValidationDefinitionCache.Get(delta.GetType().GetGenericArguments()[0]).ValidateModelDelta(state, delta, action);
            }
            else
            {
                ValidationDefinitionCache.Get(instance.GetType()).ValidateModelInstance(state, instance, action);
            }
        }

        public static void ValidateRules(IServiceProvider services, ModelStateDictionary state, object instance)
        {
            if (instance is IObjectRuleValidation) ((IObjectRuleValidation)instance).Validate(services, state);
            else if (instance is IAsyncObjectRuleValidation) ((IAsyncObjectRuleValidation)instance).Validate(services, state).Wait();
            else if (instance is IRuleValidation) ((IRuleValidation)instance).Validate(services, state, instance);
            else if (instance is IAsyncRuleValidation) ((IAsyncRuleValidation)instance).Validate(services, state, instance).Wait();
            ValidationDefinitionCache.Get(instance.GetType()).ValidateRules(services, state, instance);
        }

        public async static Task ValidateRulesAsync(IServiceProvider services, ModelStateDictionary state, object instance)
        {
            if (instance is IAsyncObjectRuleValidation) await ((IAsyncObjectRuleValidation)instance).Validate(services, state);
            else if (instance is IObjectRuleValidation) ((IObjectRuleValidation)instance).Validate(services, state);
            else if (instance is IAsyncRuleValidation) await ((IAsyncRuleValidation)instance).Validate(services, state, instance);
            else if (instance is IRuleValidation) ((IRuleValidation)instance).Validate(services, state, instance);
            ValidationDefinitionCache.Get(instance.GetType()).ValidateRules(services, state, instance);
        }

    }
}
