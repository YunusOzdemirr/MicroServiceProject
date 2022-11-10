namespace CatalogService.API.Core.Domain;

public class CatalogType
{
    public CatalogType(int ıd, string type)
    {
        Id = ıd;
        Type = type;
    }
    public int Id { get; set; }
    public string Type { get; set; }
}