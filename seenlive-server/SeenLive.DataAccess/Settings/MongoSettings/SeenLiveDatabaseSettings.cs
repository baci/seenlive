using SeenLive.Core.Abstractions.Settings;
using System;

namespace SeenLive.DataAccess.Settings.MongoSettings
{
    public class SeenLiveDatabaseSettings : ISeenLiveDatabaseSettings
    {
        private readonly string username = Environment.GetEnvironmentVariable("seenlive-db-username");
        private readonly string password = Environment.GetEnvironmentVariable("seenlive-db-password");
        private readonly string server = Environment.GetEnvironmentVariable("seenlive-db-server");

        public string ArtistsCollectionName { get; set; }
        public string DatesCollectionName { get; set; }
        public string DatabaseName { get; set; }
        public string BaseConnectionString { get; set; }

        public string ConnectionString
        {
            get
            {
                return BaseConnectionString
                    .Replace("[username]", username, StringComparison.CurrentCulture)
                    .Replace("[password]", password, StringComparison.CurrentCulture)
                    .Replace("[server]", server, StringComparison.CurrentCulture);
            }
        }
    }
}
