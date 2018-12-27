namespace TvMazeScraper.Integration.Domain.Configurations
{
    public class DatabaseFactoryConfiguration : DAL.IDatabaseFactoryConfiguration
    {
        public string ConnectionString { get; set; }
    }
}