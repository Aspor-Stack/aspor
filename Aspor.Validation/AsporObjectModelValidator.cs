using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Aspor.Validation
{
    public class AsporObjectModelValidator : IObjectModelValidator
    {
        public void Validate(ActionContext context, ValidationStateDictionary state, string prefix, object model)
        {
            if (context.ModelState.Count != context.ModelState.ErrorCount) context.ModelState.Clear();//Workaround to fix an interesting issue
            ValidationAction? action = GetValidationScope(context.HttpContext.Request.Method);
            if (action == null) return;
            AsporValidator.ValidateModel(context.ModelState, (ValidationAction)action, model);
        }

        private ValidationAction? GetValidationScope(string method)
        {
            if (method.Equals("POST")) return ValidationAction.CREATE;
            else if (method.Equals("PATCH")) return ValidationAction.UPDATE;
            else if (method.Equals("PUT")) return ValidationAction.REPLACE;
            return null;
        }
    }
}
