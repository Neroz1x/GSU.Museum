using GSU.Museum.Web.Interfaces;

namespace GSU.Museum.Web.Models
{
    ///<inheritdoc/>
    public class DatabaseSettings : IDatabaseSettings
    {
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
