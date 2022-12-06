using CatalogService.API.Core.Domain;
using CatalogService.API.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.API.Infrastructure.EntityConfigurations;

public class CatalogItemEntityTypeConfiguration:IEntityTypeConfiguration<CatalogItem>
{
    public void Configure(EntityTypeBuilder<CatalogItem> builder)
    {
        builder.ToTable("Catalog", CatalogContext.DEFAULT_SCHEMA);

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).UseHiLo("catalog_hilo").IsRequired();

        builder.Property(a => a.Name).IsRequired(true).HasMaxLength(50);
        builder.Property(a => a.Price).IsRequired(true);
        builder.Property(a => a.PictureFileName).IsRequired(true);
        builder.Ignore(a => a.PictureUri);
        builder.HasOne(a => a.CatalogBrand).WithMany().HasForeignKey(a => a.CatalogBrandId);
        builder.HasOne(a => a.CatalogType).WithMany().HasForeignKey(a => a.CatalogTypeId);


    }
}