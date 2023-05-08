namespace SaleAPI.Utils
{
    public interface ISaleSettings
    {
        string SalesCollectionName { get; set; }
        string ReservedSalesCollectionName { get; set; }
        string DeletedSalesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DataBaseName { get; set; }
    }
}
