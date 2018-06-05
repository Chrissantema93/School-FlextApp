using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace Flext.Models
{
    public class ApplicatieDbContext : DbContext
    {
        public ApplicatieDbContext(DbContextOptions<ApplicatieDbContext> options)
        : base(options) { }

        public DbSet<ImageDescription> Detecties { get; set; }
    }
}
