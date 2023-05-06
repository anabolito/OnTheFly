namespace Company.Utils
{
    public class DataBaseSettings : IDataBaseSettings
    {
        public string CompanyCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DataBaseName { get; set; }
    }

}
