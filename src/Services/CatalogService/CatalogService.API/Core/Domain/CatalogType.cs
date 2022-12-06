namespace CatalogService.API.Core.Domain;

public class CatalogType
{
    public CatalogType(int id, string type)
    {
        Id = id;
        Type = type;
    }

    public CatalogType()
    {
        
    }
    public int Id { get; set; }
    public string Type { get; set; }
}