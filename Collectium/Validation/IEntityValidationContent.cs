using System;
using Microsoft.EntityFrameworkCore;
using Collectium.Model;

namespace Collectium.Validation
{
    public interface IEntityValidationContent
    {
        public IEntityValidatorRule Check(string entities);
        public ProcessResult Validate();
        public CollectiumDBContext GetDBContext();
    }
}
