using CatalogService.API.Core.Domain;
using CatalogService.API.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.API.Infrastructure.Context;

public class CatalogContext:DbContext
{
    public const string DEFAULT_SCHEMA = "catalog";

    public CatalogContext(DbContextOptions<CatalogContext> options):base(options)
    {
    }

    public DbSet<CatalogItem> CatalogItems { get; set; }
    public DbSet<CatalogBrand> CatalogBrands { get; set; }
    public DbSet<CatalogType> CatalogTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CatalogBrandEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration());
    }
}