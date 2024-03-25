using System;
namespace Collectium.Validation
{
    public interface IValidatorRule
    {

		public IValidatorRule IsMandatory();
		public IValidatorRule AsString();
		public IValidatorRule AsDate();
		public IValidatorRule AsInteger();
		public IValidatorRule AsCollection();
		public IValidatorRule AsDouble();
		public IValidatorRule WithMinLen(int? len);
		public IValidatorRule WithMaxLen(int? len);
		public IValidatorRule WithDateFormat(string format);

		public IValidationContent Pack();
	}

}
