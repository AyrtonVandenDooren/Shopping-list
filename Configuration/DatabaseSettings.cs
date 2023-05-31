namespace Shops.Configuration;

public class DatabaseSettings
{
    public string? ConnectionString {get;set;}
    public string? DatabaseName { get; set; }
    public string? ShopsCollection { get; set; }
    public string? ProductsCollection { get; set; }
    public string? CategorysCollection { get; set; }
    public string? BasketCollection {get; set;}
}