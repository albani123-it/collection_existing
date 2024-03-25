using System;
using System.Collections;
using System.Globalization;
using System.Linq;

namespace Collectium.Validation
{
    public class ValidationRule : IValidatorRule
    {

        private ValidationContent Content;
        public string FieldName { get; set; }
        private string FieldLabel = "";
        private bool Mandatory = false;
        private bool IsString = false;
        private bool IsInteger = false;
        private bool IsCollection = false;
        private bool IsDate = false;
        private bool IsDouble = false;
        private int? Min = null;
        private int? Max = null;
        private String DateFormat = "dd/MM/yyyy";
        private String Lang = "id";

        public ValidationRule(ValidationContent Content)
        {
            this.Content = Content;
        }

        public IValidatorRule AsDate()
        {
            this.IsDate = true;
            return this;
        }

        public IValidatorRule AsDouble()
        {
            this.IsDouble = true;
            return this;
        }

        public IValidatorRule AsInteger()
        {
            this.IsInteger = true;
            return this;
        }

        public IValidatorRule AsCollection()
        {
            this.IsCollection = true;
            return this;
        }

        public IValidatorRule AsString()
        {
            this.IsString = true;
            return this;
        }

        public IValidatorRule IsMandatory()
        {
            this.Mandatory = true;
            return this;
        }

        public IValidatorRule WithDateFormat(string format)
        {
            this.DateFormat = format;
            return this;
        }

        public IValidatorRule WithMaxLen(int? len)
        {
            this.Max = len;
            return this;
        }

        public IValidatorRule WithMinLen(int? len)
        {
            this.Min = len;
            return this;
        }

        public ProcessResult validate()
        {
            ProcessResult pr = new ProcessResult();
            pr.Result = true;
            Object obj = this.Content.TargetObj;
            String field = null;
            Object stmp = null;
            try
            {
                stmp = obj.GetType().GetProperty(FieldName).GetValue(obj, null);
            }
            catch (Exception e)
            {
                // TODO: handle exception
                Console.WriteLine(e.Message);
            }

            if (stmp != null)
            {
                field = stmp.ToString();
            }

            return check(pr, field, stmp);
        }

        private ProcessResult check(ProcessResult pr, string field, Object stmp)
        {
            if (this.Mandatory)
            {
                if (this.IsCollection)
                {
                    if (stmp == null)
                    {
                        pr.Result = false;
                        pr.Message = FieldName + " is mandatory";
                        return pr;
                    }
                } else
                {
                    if (field == null || field.Equals("") || field.Length < 1)
                    {
                        pr.Result = false;
                        pr.Message = FieldName + " is mandatory";
                        return pr;
                    }
                }

            }

            if (this.IsString)
            {
                if (this.Min != null)
                {
                    if (field.Length < this.Min)
                    {
                        pr.Result = false;
                        pr.Message = FieldName + " length is min " + this.Min;
                        return pr;
                    }
                }
                if (this.Max != null)
                {
                    if (field.Length > this.Max)
                    {
                        pr.Result = false;
                        pr.Message = FieldName + " length is max " + this.Max;
                        return pr;
                    }
                }
            } else if (this.IsInteger)
            {
                try
                {
                    int? result = Int32.Parse(field);
                    if (this.Min != null)
                    {
                        if (result < this.Min)
                        {
                            pr.Result = false;
                            pr.Message = FieldName + " value is min " + this.Min;
                            return pr;
                        }
                    }

                    if (this.Max != null)
                    {
                        if (result > this.Max)
                        {
                            pr.Result = false;
                            pr.Message = FieldName + " value is max " + this.Max;
                            return pr;
                        }
                    }
                }
                catch (FormatException)
                {
                    pr.Result = false;
                    pr.Message = FieldName + " is not numeric";
                    return pr;
                }
            } else if (this.IsDate)
            {
                if (field.Length > 10)
                {
                    field = field.Substring(0, 10);
                }
                CultureInfo provider = CultureInfo.InvariantCulture;
                // It throws Argument null exception
                DateTime? dt = DateTime.ParseExact(field, "yyyy-MM-dd", provider);
                if (dt == null)
                {
                    pr.Result = false;
                    pr.Message = FieldName + " is mandatory";
                    return pr;
                }
            } else if (this.IsCollection)
            {
                var t = ((IEnumerable)stmp).Cast<object>().ToList();
                if (t.Count() < 1)
                {
                    pr.Result = false;
                    pr.Message = FieldName + " is not date";
                    return pr;
                }
            }

            return pr;
        }

        public IValidationContent Pack()
        {
            return this.Content;
        }
    }
}
