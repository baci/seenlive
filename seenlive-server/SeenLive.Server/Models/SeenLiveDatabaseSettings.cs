using System;

namespace SeenLive.Server.Models
{
    public class SeenLiveDatabaseSettings : ISeenLiveDatabaseSettings
    {
        private readonly string username = Environment.GetEnvironmentVariable("seenlive-db-username", EnvironmentVariableTarget.User);
        private readonly string password = Environment.GetEnvironmentVariable("seenlive-db-password", EnvironmentVariableTarget.User);
        private readonly string server = Environment.GetEnvironmentVariable("seenlive-db-server", EnvironmentVariableTarget.User);

        public string ArtistsCollectionName { get; set; }
        public string DatesCollectionName { get; set; }        
        public string DatabaseName { get; set; }
        public string BaseConnectionString { get; set; }

        public string ConnectionString { 
            get {                
                return BaseConnectionString
                    .Replace("[username]", username, StringComparison.CurrentCulture)
                    .Replace("[password]", password, StringComparison.CurrentCulture)
                    .Replace("[server]", server, StringComparison.CurrentCulture);
            } 
        }
    }

    public interface ISeenLiveDatabaseSettings
    {
        string ArtistsCollectionName { get; set; }
        string DatesCollectionName { get; set; }        
        string DatabaseName { get; set; }
        string BaseConnectionString { get; set; }
        string ConnectionString { get; }
    }
}
