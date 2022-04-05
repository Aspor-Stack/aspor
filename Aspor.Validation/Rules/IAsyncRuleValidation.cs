using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace Aspor.Validation.Rules
{
    public interface IAsyncRuleValidation
    {

        public abstract Task Validate(IServiceProvider services, ModelStateDictionary state, object instance);

    }
}
