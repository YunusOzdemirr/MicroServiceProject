namespace CatalogService.API.Core.Domain;

public class CatalogBrand
{
    public CatalogBrand(int id, string brand)
    {
        Id = id;
        Brand = brand;
    }
    public int Id { get; set; }
    public string Brand { get; set; }
}