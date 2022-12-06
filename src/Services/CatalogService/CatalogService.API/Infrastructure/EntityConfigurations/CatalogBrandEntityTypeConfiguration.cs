using CatalogService.API.Core.Domain;
using CatalogService.API.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.API.Infrastructure.EntityConfigurations;

public class CatalogBrandEntityTypeConfiguration:IEntityTypeConfiguration<CatalogBrand>
{
    public void Configure(EntityTypeBuilder<CatalogBrand> builder)
    {
        builder.ToTable("CatalogBrand", CatalogContext.DEFAULT_SCHEMA);

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).UseHiLo("catalog_brand_hilo").IsRequired();
        builder.Property(a => a.Brand).IsRequired().HasMaxLength(100);

    }
}