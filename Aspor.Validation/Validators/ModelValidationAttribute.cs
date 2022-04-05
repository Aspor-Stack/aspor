using System;

namespace Aspor.Validation.Validators
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ModelValidationAttribute : Attribute
    {

        public abstract bool IsValid(ValidationAction action, object input);

        public abstract string FormatErrorMessage(string propertyName);
    }
}
