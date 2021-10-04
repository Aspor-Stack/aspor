namespace Aspor.Validation.Validators
{
    public class DisallowModificationAttribute : AllowOnlyAttribute
    {
        public DisallowModificationAttribute() : base(ValidationAction.CREATE) { }

        public override string FormatErrorMessage(string propertyName)
        {
            return propertyName + " can not be modified";
        }

    }
}
