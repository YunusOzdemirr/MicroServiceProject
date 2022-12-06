using CatalogService.API.Core.Domain;
using CatalogService.API.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.API.Infrastructure.EntityConfigurations;

public class CatalogTypeEntityTypeConfiguration:IEntityTypeConfiguration<CatalogType>
{
    public void Configure(EntityTypeBuilder<CatalogType> builder)
    {
        builder.ToTable("CatalogType", CatalogContext.DEFAULT_SCHEMA);

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).UseHiLo("catalog_hilo").IsRequired();
        builder.Property(a => a.Type).IsRequired(true).HasMaxLength(100);
    }
}