using System;
namespace Collectium.Validation
{
    public sealed class IlKeiValidator : IValidator
    {
        private static readonly Lazy<IlKeiValidator> lazy = new Lazy<IlKeiValidator>(() => new IlKeiValidator());

        public static IlKeiValidator Instance { get { return lazy.Value; } }

        private IlKeiValidator()
        {
        }

        public IValidationContent WithPoCo(object obj)
        {
            var val = new ValidationContent();
            val.TargetObj = obj;
            return val;
        }
    }
}
