namespace CompanyAPI.Utils
{
    public interface IDatabaseSettings
    {
        string CompanyCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DataBaseName { get; set; }
    }
}