namespace Collectium.Model.Bean
{
    public class GenericResponseSum<T, U>
    {
        public GenericResponseSum()
        {
            this.Data = new List<T>();

        }

        public Boolean? Status { set; get; }
        public string? Message { set; get; }
        public string? Page { set; get; }

        public int? MaxPage { set; get; }

        public int? DataCount { set; get; }

        public ICollection<T> Data { set; get; }

        public U? Summary { set; get; }

        public void AddData(T t)
        {
            if (this.Data == null)
            {
                this.Data = new List<T>();
            }

            this.Data.Add(t);
        }
    }
}
