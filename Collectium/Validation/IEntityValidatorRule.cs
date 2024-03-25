using System;
namespace Collectium.Validation
{
    public interface IEntityValidatorRule
    {
        public IEntityValidatorRule WithId(int? Id);
        public IEntityValidationContent Pack();
    }
}
