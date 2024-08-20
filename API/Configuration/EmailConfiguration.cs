namespace API.Configuration
{
    public class EmailConfiguration
    {

        public required string Host { get; set; }

        public required int Port { get; set; }

        public required bool UseSSL { get; set; }

        public required EmailFromDetails From { get; set; }

    }
}
