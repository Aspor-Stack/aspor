using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Aspor.Validation.Rules
{
    public interface IObjectRuleValidation
    {

        public abstract void Validate(IServiceProvider services, ModelStateDictionary state);
    }
}
