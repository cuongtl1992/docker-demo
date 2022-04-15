using DockerDemo.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DockerDemo.API.Infras.Persistence
{
    public class DockerDemoDbContext :  DbContext
    {
        public DbSet<SugarBaby> SugarBabies { get; set; }
        
        public DockerDemoDbContext(DbContextOptions<DockerDemoDbContext> options) 
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SugarBaby>()
                .ToTable(nameof(SugarBabies))
                .HasKey(s => s.Id)
                .IsClustered();

            modelBuilder.Entity<SugarBaby>()
                .HasIndex(s => s.Name)
                .IncludeProperties(s => s.Age)
                .IsUnique();
            
            modelBuilder.Entity<SugarBaby>()
                .Property(s => s.Name)
                .IsRequired();

            modelBuilder.Entity<SugarBaby>()
                .Property(s => s.Age)
                .IsRequired();
            
            base.OnModelCreating(modelBuilder);
        }
    }
}