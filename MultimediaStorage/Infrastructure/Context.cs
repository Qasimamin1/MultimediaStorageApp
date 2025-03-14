
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> contextOptions) 
            : base(contextOptions)
        {

        }

        // Code - Approach
        public virtual DbSet<MultiMedia> Multimedias { get; set; }
    }
}
