namespace Collectium.Validation
{
    public sealed class IlKeiCopyObject : IIlKeiCopyObject
    {
        private static readonly Lazy<IlKeiCopyObject> lazy = new Lazy<IlKeiCopyObject>(() => new IlKeiCopyObject());

        private object Src;
        private object Dest;
        private Dictionary<string, string> Exc;
        private Dictionary<string, string> Inc;

        public static IlKeiCopyObject Instance
        {
            get { return lazy.Value; }
        }

        private IlKeiCopyObject()
        {
            this.Exc = new Dictionary<string, string>();
            this.Inc = new Dictionary<string, string>();
        }

        public IIlKeiCopyObject WithSource(object obj)
        {
            this.Src = obj;
            return this;
        }

        public IIlKeiCopyObject WithDestination(object obj)
        {
            this.Dest = obj;
            return this;
        }

        public IIlKeiCopyObject Exclude(string obj)
        {
            if (this.Exc.ContainsKey(obj) == false)
            {
                this.Exc.Add(obj, obj);
            }

            return this;
        }

        public IIlKeiCopyObject Include(string obj)
        {
            if (this.Inc.ContainsKey(obj) == false)
            {
                this.Inc.Add(obj, obj);
            }

            return this;
        }

        public void Execute()
        {
            if (this.Src == null || this.Dest == null)
            {
                return;
            }

            var sprop = this.Src.GetType().GetProperties();
            
            var sdest = this.Dest.GetType().GetProperties();
            var ddest = new Dictionary<string, string>();
            foreach (var p in sdest)
            {
                var sn = p.Name;
                ddest.Add(sn, sn);
            }

            foreach (var p in sprop)
            {
                var pn = p.Name;

                if (this.Inc.Keys.Count() > 0)
                {
                    if (this.Inc.ContainsKey(pn) == false)
                    {
                        continue;
                    }
                } else
                {
                    if (this.Exc.ContainsKey(pn))
                    {
                        continue;
                    }
                }

                if (ddest.ContainsKey(pn) == false)
                {
                    continue;
                }

                var pv = p.GetValue(this.Src);

                var dprop = this.Dest.GetType().GetProperty(pn);
                if (dprop == null)
                {
                    continue;
                }

                if (dprop.SetMethod != null && dprop.SetMethod.IsPrivate)
                {
                    continue;
                }


                if (pv == null)
                {
                    dprop.SetValue(this.Dest, null, null);
                }
                else
                {
                    var pt = dprop.PropertyType;
                    var targetType = IsNullableType(pt) ? Nullable.GetUnderlyingType(pt) : pt;
                    var idWithRightType = Convert.ChangeType(pv, targetType!);
                    dprop.SetValue(this.Dest, idWithRightType);
                }

            }
        }

        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }
    }
}
