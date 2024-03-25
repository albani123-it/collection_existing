using System;
using Collectium.Model;

namespace Collectium.Validation
{
    public sealed class IlKeiEntityValidator : IEntityValidator
    {
        private static readonly Lazy<IlKeiEntityValidator> lazy = new Lazy<IlKeiEntityValidator>(() => new IlKeiEntityValidator());

        public static IlKeiEntityValidator Instance { get { return lazy.Value; } }

        private IlKeiEntityValidator()
        {
        }

        public IEntityValidationContent WithPoCo(CollectiumDBContext context)
        {
            var val = new EntityValidationContent(context);
            return val;
        }
    }
}
