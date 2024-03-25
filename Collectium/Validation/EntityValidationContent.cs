using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Collectium.Model;

namespace Collectium.Validation
{
    public class EntityValidationContent : IEntityValidationContent
    {
        private List<EntityValidatorRule> rules;
        private CollectiumDBContext context;

        public EntityValidationContent(CollectiumDBContext context)
        {
            this.rules = new List<EntityValidatorRule>();
            this.context = context;
        }

        public IEntityValidatorRule Check(string name)
        {
            var rule = new EntityValidatorRule(this, name);
            this.rules.Add(rule);
            return rule;
        }

        public CollectiumDBContext GetDBContext()
        {
            return this.context;
        }

        public ProcessResult Validate()
        {
            foreach (var validationRule in rules)
            {
                var px = validationRule.validate();
                if (px.Result == false)
                {
                    return px;
                }
            }
            ProcessResult pr = new ProcessResult();
            pr.Result = true;
            pr.Message = "";
            return pr;
        }
    }
}
