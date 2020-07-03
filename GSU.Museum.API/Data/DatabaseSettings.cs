using GSU.Museum.API.Interfaces;

namespace GSU.Museum.API.Data
{
    ///<inheritdoc/>
    public class DatabaseSettings : IDatabaseSettings
    {
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
