namespace Collectium.Model.Bean.Request
{

    public class TelephonyRequest
    {
        public int? Id { get; set; }
    }

    public class AdmLoginRequest
    {
        public string? Cmd { get; set; }
    }

    public class AdmLoginDataResponse
    {
        public string? Ticket { get; set; }

        public string? Ip { get; set; }

        public string? Ts { get; set; }
    }

    public class AdmLoginResponse
    {
        public string? Status { get; set; }

        public IList<AdmLoginDataResponse>? Data { set; get; }
    }


    public class AgentLoginRequest
    {
        public string? Cmd { get; set; }

        public string? Event { get; set; }

        public string? Queue { get; set; }

        public string? Agent { get; set; }

        public string? Device { get; set; }
    }


    public class AgentLoginDataResponse
    {
        public string? Datetime { get; set; }

        public string? Event { get; set; }

        public string? Queue { get; set; }

        public string? Device { get; set; }

        public string? Priority { get; set; }

        public string? Status { get; set; }
    }

    public class AgentLoginResponse
    {
        public string? Status { get; set; }

        public IList<AgentLoginDataResponse>? Data { set; get; }
    }


    public class AgentCallRequest
    {
        public string? Cmd { get; set; }

        public string? Dialto { get; set; }

        public string? Timeout { get; set; }

        public string? Cid { get; set; }

        public string? Dialfrom { get; set; }

        public string? V { get; set; }
    }

    public class AgentCallDataResponse
    {
        public string? Datetime { get; set; }

        public string? Channel { get; set; }

        public string? Extension { get; set; }

        public string? Uniqueid { get; set; }

        public string? Linkedid { get; set; }

        public string? V { get; set; }
    }

    public class AgentCallResponse
    {
        public string? Status { get; set; }

        public IList<AgentCallDataResponse>? Data { set; get; }
    }

    public class CallBackBean
    {
        public string? CallId { get; set; }

        public string? Ctc { get; set; }

        public IFormFile? File { get; set; }

    }

    public class CallBackResponseBean
    {

        public int? Id { get; set; }

        public string? AccNo { get; set; }

        public string? PhoneNo { get; set; }

        public DateTime? CallDate{ get; set; }

        public string? Result { get; set; }


    }

    public class CallBackRequest : PagedRequestBean
    {
        public int? Id { get; set; }
    }
}
