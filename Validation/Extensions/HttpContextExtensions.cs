using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace Aspor.Validation.Extensions
{

    public static class HttpContextExtensions
    {

        public static ValidationAction GetAction(this HttpContext context)
        {
            ValidationAction? action = GetActionOrNull(context);
            if (action == null) throw new InvalidOperationException("");
            return (ValidationAction)action;
        }

        public static ValidationAction? GetActionOrNull(this HttpContext context)
        {
            string method = context.Request.Method;
            if (method.Equals("POST")) return ValidationAction.CREATE;
            else if (method.Equals("PATCH")) return ValidationAction.UPDATE;
            else if (method.Equals("PUT")) return ValidationAction.REPLACE;
            else if (method.Equals("DELETE")) return ValidationAction.DELETE;
            //else if (method.Equals("GET")) return ValidationAction.READ;
            return null;
        }

        public static bool ValidateRules(this HttpContext context, object instance)
        {
            if (instance == null) return false;
            ModelStateDictionary state = context.Items["modelState"] as ModelStateDictionary;
            AsporValidator.ValidateRules(context.RequestServices, state, instance);
            return true;
        }

        public async static Task<bool> ValidateRulesAsync(this HttpContext context, object instance)
        {
            if (instance == null) return false;
            ModelStateDictionary state = context.Items["modelState"] as ModelStateDictionary;
            await AsporValidator.ValidateRulesAsync(context.RequestServices, state, instance);
            return true;
        }
    }

}
