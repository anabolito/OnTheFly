namespace CompanyAPI.Utils
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string CompanyCollectionName { get; set; }
        public string RestrictedCompaniesCollectionName { get; set; }
        public string ReleasedCompaniesCollectionName { get; set; }
        public string DeletedCompaniesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DataBaseName { get; set; }
    }

}
