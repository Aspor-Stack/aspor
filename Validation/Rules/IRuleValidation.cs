using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Aspor.Validation.Rules
{
    public interface IRuleValidation
    {

        public abstract void Validate(IServiceProvider services, ModelStateDictionary state, object instance);

    }
}
