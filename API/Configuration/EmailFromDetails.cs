namespace API.Configuration
{
    public class EmailFromDetails
    {

        public required string EmailAddress { get; set; }

        public string? DisplayName { get; set; }

        public required string Username { get; set; }

        public required string Password { get; set; }   

    }
}
