using System;
using System.Collections.Generic;

namespace Collectium.Validation
{
    public class ValidationContent : IValidationContent
    {
        private List<ValidationRule> rules;

        public Object TargetObj { get; set; }

        public ValidationContent()
        {
            rules = new List<ValidationRule>();
        }

        public IValidatorRule Pick(string name)
        {
            var rule = new ValidationRule(this);
            rule.FieldName = name;
            rules.Add(rule);
            return rule;
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
