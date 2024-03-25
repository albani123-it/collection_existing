using Collectium.Model.Bean.Response;

namespace Collectium.Model.Bean.Request
{
    public class ReportAktivitasCabangRequest
    {
        public IList<SpvMonResponseBean>? Data { get; set; }

        public string? Cabang { get; set; }

        public string? Start { get; set; }

        public string? End { get; set; }
    }
}
