namespace Collectium.Model.Bean
{
    public class UserWrapper
    {
        public UserWrapper()
        {
        }

        public int? Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Lastname { get; set; }

        public string Password { get; set; }

        public int? RoleId { get; set; }

    }
}
