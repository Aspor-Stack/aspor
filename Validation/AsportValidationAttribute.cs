using Aspor.Validation.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Aspor.Validation
{
    public class AsporValidationAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ValidationAction? action = context.HttpContext.GetActionOrNull();
            if (action == null) return;

            foreach (ParameterDescriptor descriptor in context.ActionDescriptor.Parameters)
            {
                if (descriptor.BindingInfo.BindingSource.DisplayName.Equals("Body"))
                {
                    object instance;
                    if (context.ActionArguments.TryGetValue(descriptor.Name, out instance))
                    {
                        AsporValidator.ValidateModel(context.ModelState, (ValidationAction)action, instance);
                    }

                }
            }
            if (!context.ModelState.IsValid) context.Result = new BadRequestObjectResult(context.ModelState);
        }

    }

}
