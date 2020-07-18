namespace GSU.Museum.Web.Interfaces
{
    /// <summary>
    /// Contains database settings
    /// </summary>
    public interface IDatabaseSettings
    {
        /// <summary>
        /// Name of the collection from appseting.json in DatabaseSettings section
        /// </summary>
        string CollectionName { get; set; }

        /// <summary>
        /// Connection string from appseting.json in DatabaseSettings section
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// Database name from appseting.json in DatabaseSettings section
        /// </summary>
        string DatabaseName { get; set; }
    }
}
