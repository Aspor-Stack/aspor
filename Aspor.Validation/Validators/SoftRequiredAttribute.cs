using Aspor.Validation;
using Aspor.Validation.Validators;

namespace Validation.Validators
{
    public class SoftRequiredAttribute : ModelValidationAttribute
    {
        public override string FormatErrorMessage(string propertyName)
        {
            return propertyName + " is required";
        }

        public override bool IsValid(ValidationAction action, object input)
        {
            return input != null;
        }
    }
}
