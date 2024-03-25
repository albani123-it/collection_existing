using System;
using Collectium.Model;

namespace Collectium.Validation
{
    public interface IEntityValidator
    {
        public IEntityValidationContent WithPoCo(CollectiumDBContext context);
    }
}
