using Microsoft.EntityFrameworkCore;

namespace NetCore.RabbitMQ.Watermark.Web.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
}