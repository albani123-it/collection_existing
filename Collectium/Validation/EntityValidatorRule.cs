using System;

namespace Collectium.Validation
{
    public class EntityValidatorRule : IEntityValidatorRule
    {

        private IEntityValidationContent Content;
        private int? Id;
        private string dbSet;

        public EntityValidatorRule(IEntityValidationContent content, string dbSet)
        {
            this.Content = content;
            this.dbSet = dbSet;
        }

        public IEntityValidationContent Pack()
        {
            return this.Content;
        }

        public IEntityValidatorRule WithId(int? Id)
        {
            this.Id = Id;
            return this;
        }

        public ProcessResult validate()
        {
            string ctxName = dbSet;
            var ctx = Content.GetDBContext();

            ProcessResult pr = new ProcessResult();
            pr.Result = false;

            //TODO ADD ENTITY VALIDATION

            pr.Result = true;
            return pr;
        }
    }
}
