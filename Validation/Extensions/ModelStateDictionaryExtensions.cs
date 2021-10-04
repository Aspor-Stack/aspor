using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Aspor.Validation.Extensions
{
    public static class ModelStateDictionaryExtensions
    {

        public static bool IsValid(this ModelStateDictionary state, object instance, ValidationAction action = ValidationAction.CREATE, IServiceProvider? services = null)
        {
            return IsModelValid(state, instance, action) && AreRulesValid(state, instance, services);
        }

        public static bool IsModelValid(this ModelStateDictionary state, object instance, ValidationAction action = ValidationAction.CREATE)
        {
            AsporValidator.ValidateModel(state, action, instance);
            return state.IsValid;
        }

        public static bool AreRulesValid(this ModelStateDictionary state, object instance, IServiceProvider? services = null)
        {
            AsporValidator.ValidateRules(services, state, instance);
            return state.IsValid;
        }
    }
}
