namespace CompanyAPI.Utils
{
    public interface IDataBaseSettings
    {
        string CompanyCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DataBaseName { get; set; }
    }
}