using Microsoft.EntityFrameworkCore;
using S.Data.Entities;

namespace Project.Data
{
    public class ScreeningDbContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Screening> Screenings { get; set; }
        public ScreeningDbContext(DbContextOptions<ScreeningDbContext> options) : base(options)
        {

        }
    }
}
