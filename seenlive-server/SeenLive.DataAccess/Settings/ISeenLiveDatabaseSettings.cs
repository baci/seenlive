namespace SeenLive.DataAccess.Settings
{
    public interface ISeenLiveDatabaseSettings
    {
        string ArtistsCollectionName { get; set; }
        string DatesCollectionName { get; set; }
        string DatabaseName { get; set; }
        string BaseConnectionString { get; set; }
        string ConnectionString { get; }
    }
}
