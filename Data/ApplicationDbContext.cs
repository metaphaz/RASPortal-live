using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RASPortal.Models;

namespace RASPortal.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Event> Events { get; set; }
        public DbSet<Project> Projects { get; set; }
        // Add additional DbSet properties here if you have other entities
    }
}
