namespace TvMazeScraper.Presentation.Domain.Configurations
{
    public class StoreConfiguration : DAL.IDatabaseFactoryConfiguration
    {
        public string ConnectionString { get; set; }
    }
}