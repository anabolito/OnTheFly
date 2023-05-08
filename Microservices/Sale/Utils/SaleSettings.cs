namespace SaleAPI.Utils
{
    public class SaleSettings : ISaleSettings
    {
        public string SalesCollectionName { get; set; }
        public string ReservedSalesCollectionName { get; set; }
        public string DeletedSalesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DataBaseName { get; set; }
    }
}
