using GSU.Museum.CommonClassLibrary.Interfaces;

namespace GSU.Museum.CommonClassLibrary.Models
{
    ///<inheritdoc/>
    public class DatabaseSettings : IDatabaseSettings
    {
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string UsersCollectionName { get; set; }
    }
}
