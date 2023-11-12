using SeenLive.Core.Abstractions.Settings;
using System;

namespace SeenLive.DataAccess.Settings.MongoSettings
{
    public class SeenLiveDatabaseSettings : ISeenLiveDatabaseSettings
    {
        private readonly string username = Environment.GetEnvironmentVariable("seenlive-db-username")!;
        private readonly string password = Environment.GetEnvironmentVariable("seenlive-db-password")!;
        private readonly string server = Environment.GetEnvironmentVariable("seenlive-db-server")!;

        public required string ArtistsCollectionName { get; set; }
        public required string DatesCollectionName { get; set; }
        public required string UsersCollectionName { get; set; }
        public required string DatabaseName { get; set; }
        public required string BaseConnectionString { get; set; }

        public string ConnectionString =>
            BaseConnectionString
                .Replace("[username]", username, StringComparison.CurrentCulture)
                .Replace("[password]", password, StringComparison.CurrentCulture)
                .Replace("[server]", server, StringComparison.CurrentCulture);
    }
}
