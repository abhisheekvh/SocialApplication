using Microsoft.EntityFrameworkCore;
using SocialApplication.Entities;



public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<AppUser> Users { get; set; }

}

