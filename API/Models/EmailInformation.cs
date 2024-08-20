using MongoDB.Bson;

namespace API.Models
{
    public class EmailInformation
    {

        public required List<KeyValuePair<string, string>> PrimaryRecipients { get; set; }

        public List<KeyValuePair<string, string>>? CarbonCopyRecipients { get; set; }

        public List<KeyValuePair<string, string>>? HiddenRecipients { get; set; }

        public required string EmailBodyId {  get; set; }

        public required Dictionary<string, string> Data { get; set; }    

    }
}
