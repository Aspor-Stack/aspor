using Aspor.Validation.Rules;
using Aspor.Validation.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace Aspor.Validation.Definition
{
    public class ValidationDefinitionBuilder
    {
        public static ValidationDefinition Build(Type type)
        {
            IList<ValidationPropertyDefinition> validations = new List<ValidationPropertyDefinition>();
            IList<object> ruleValidations = new List<object>();

            foreach (Attribute attribute in type.GetCustomAttributes())
            {
                if (attribute is IAsyncRuleValidation) ruleValidations.Add((IAsyncRuleValidation)attribute);
                else if (attribute is IRuleValidation) ruleValidations.Add((IRuleValidation)attribute);
            }

            foreach (PropertyInfo property in type.GetProperties().Where(p => p.GetIndexParameters().Length == 0))
            {
                List<object> validators = new List<object>();
                foreach (Attribute attribute in property.GetCustomAttributes())
                {
                    if (attribute is ModelValidationAttribute)
                    {
                        validators.Add(attribute);
                    }
                    else if (attribute is ValidationAttribute)
                    {
                        validators.Add(attribute);
                    }
                    else if (attribute is DatabaseGeneratedAttribute)
                    {
                        validators.Add(new OnlyServerAttribute());
                    }
                }
                validations.Add(new ValidationPropertyDefinition(property.Name, property.GetGetMethod(), validators));
            }
            return new ValidationDefinition(validations, ruleValidations);
        }
    }
}
