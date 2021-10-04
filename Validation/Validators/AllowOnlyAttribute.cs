namespace Aspor.Validation.Validators
{
    public class AllowOnlyAttribute : ModelValidationAttribute
    {

        private readonly ValidationAction _action;

        public AllowOnlyAttribute(ValidationAction action)
        {
            _action = action;
        }

        public override bool IsValid(ValidationAction action, object input)
        {
            return action == _action;
        }

        public override string FormatErrorMessage(string propertyName)
        {
            return propertyName + " is only allowed for " + _action + " operations";
        }
    }
}
