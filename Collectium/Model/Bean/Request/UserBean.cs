namespace Collectium.Model.Bean.Request
{
    public class UserActivateRequest
    {
        public int? UserId { get; set; }
    }

    public class UserChangePasswordRequest
    {
        public int? UserId { get; set; }

        public string? Password { get; set; }
    }

    public class UserSelfPasswordRequest
    {
        public string? Password1 { get; set; }

        public string? Password { get; set; }
    }

    public class UserChangeTelRequest
    {
        public int? UserId { get; set; }

        public string? TelCode { get; set; }

        public string? TelDevice { get; set; }

        public string? PassDevice { get; set; }
    }

    public class UserDistribution
    {
        public int UserId { get; set; }

        public int Count { get; set; }

    }
}
