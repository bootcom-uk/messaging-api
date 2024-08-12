using API.Configuration;
using API.Models;
using MongoDB.Driver;

namespace API.Services
{
    public class APIKeyService
    {

        private readonly MessagingConfiguration Configuration;

        private readonly DatabaseService DatabaseService;

        public APIKeyService(IConfiguration configuration, DatabaseService databaseService) {
            Configuration = configuration.Get<MessagingConfiguration>()!;
            DatabaseService = databaseService;
        }

        public async Task<APIModel> CollectAPIDetail(string apiKey)
        {
            var database = DatabaseService.GetMongoDatabase();
            var collection = database.GetCollection<APIModel>("APIKeys");
            return await collection.Find(record => record.APIKey == apiKey).FirstOrDefaultAsync();
        }

    }
}
