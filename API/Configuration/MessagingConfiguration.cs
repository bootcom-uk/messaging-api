namespace API.Configuration
{
    public class MessagingConfiguration
    {

        public required MongoDatabaseDetail DatabaseDetail { get; set; }

        public required MessageTypes Messaging {  get; set; }

    }
}
