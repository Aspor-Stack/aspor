using System;

namespace Aspor.Validation.Validators
{
    public class OnlyServerAttribute : ModelValidationAttribute
    {

        public override bool IsValid(ValidationAction action, object input)
        {
            if (action == ValidationAction.CREATE)
            {
                return input == null
                    || (input is int && ((int)input) == default(int))
                    || (input is DateTime && ((DateTime)input) == default(DateTime))
                    || (input is Guid && ((Guid)input) == default(Guid));
            }
            else return input == null;
        }

        public override string FormatErrorMessage(string propertyName)
        {
            return propertyName + " is generated automatically and can only be modified by the server";
        }
    }
}
