using System;
namespace Collectium.Validation
{
    public interface IValidationContent
    {

        public IValidatorRule Pick(String name);
        public ProcessResult Validate();
    }
}
