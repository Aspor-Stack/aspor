using Aspor.Validation.Validators;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Aspor.Validation.Definition
{
    public class ValidationPropertyDefinition
    {

        public ValidationPropertyDefinition(string name, MethodInfo? instanceGetMethod, List<object> validators)
        {
            Name = name;
            InstanceGetMethod = instanceGetMethod;
            Validators = validators;
        }

        public string Name { get; }

        public MethodInfo InstanceGetMethod { get; }

        public List<object> Validators { get; }

        public void Validate(ModelStateDictionary state, ValidationAction action, object? value)
        {
            foreach (object validator in Validators)
            {
                ModelValidationAttribute modelValidation = validator as ModelValidationAttribute;
                if (modelValidation != null)
                {
                    if (!modelValidation.IsValid(action, value))
                    {
                        state.AddModelError(Name, modelValidation.FormatErrorMessage(Name));
                    }
                }
                else
                {
                    ValidationAttribute validation = validator as ValidationAttribute;
                    if (validation != null)
                    {
                        if (!validation.IsValid(value))
                        {
                            state.AddModelError(Name, validation.FormatErrorMessage(Name));
                        }
                    }
                }
            }
        }
    }
}
