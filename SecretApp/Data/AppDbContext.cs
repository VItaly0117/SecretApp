using Microsoft.EntityFrameworkCore;
using SecretApp.Models;

namespace SecretApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<SecretMessage> Messages { get; set; }
}