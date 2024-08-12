using API.Configuration;
using MongoDB.Driver;

namespace API.Services
{
    public class DatabaseService
    {

        private MessagingConfiguration Configuration { get; init; }


        public DatabaseService(IConfiguration configuration)
        {
            Configuration = configuration.Get<MessagingConfiguration>()!;
        }

        public IMongoDatabase GetMongoDatabase()
        {
            var clientSettings = MongoClientSettings.FromConnectionString(Configuration.DatabaseDetail.ConnectionString);

            var client = new MongoClient(clientSettings);

            return client.GetDatabase(Configuration.DatabaseDetail.DatabaseName);
        }

    }
}
