namespace GSU.Museum.CommonClassLibrary.Interfaces
{
    /// <summary>
    /// Contains database settings
    /// </summary>
    public interface IDatabaseSettings
    {
        /// <summary>
        /// Name of the collection of the content from appseting.json in DatabaseSettings section
        /// </summary>
        string CollectionName { get; set; }
        
        /// <summary>
        /// Name of the collection pf users from appseting.json in DatabaseSettings section
        /// </summary>
        string UsersCollectionName { get; set; }

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
