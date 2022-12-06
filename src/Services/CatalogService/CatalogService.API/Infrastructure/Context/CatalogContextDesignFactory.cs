using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CatalogService.API.Infrastructure.Context;

public class CatalogContextDesignFactory : IDesignTimeDbContextFactory<CatalogContext>
{
    public CatalogContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CatalogContext>()
            .UseSqlServer("Data Source=c_sqlserver;Initial Catalog=catalog;Persist Security Info=True;User ID=sa;Password=Salih123!");

        return new CatalogContext(optionsBuilder.Options);
    }
}