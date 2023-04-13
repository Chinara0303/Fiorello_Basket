using Microsoft.EntityFrameworkCore;
using Practice.Models;
using System.Linq.Expressions;

namespace Practice.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SliderInfo> SliderInfos { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<ExpertHeader> ExpertHeaders { get; set; }
        public DbSet<Expert> Experts { get; set; }
        public DbSet<ExpertPosition> ExpertPositions { get; set; }
        public DbSet<ExpertExpertPosition> ExpertExpertPositions { get; set; }
        public DbSet<Subscribe> Subscribes { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogHeader> BlogHeaders { get; set; }
        public DbSet<Say> Says { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Instagram> Instagrams { get; set; }
        public bool Deleted { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasQueryFilter(p => !p.SoftDelete);
            modelBuilder.Entity<Blog>().HasQueryFilter(p => !p.SoftDelete);
            modelBuilder.Entity<Blog>().HasQueryFilter(p => !p.SoftDelete);
            modelBuilder.Entity<BlogHeader>().HasQueryFilter(p => !p.SoftDelete);
            modelBuilder.Entity<Category>().HasQueryFilter(p => !p.SoftDelete);
            modelBuilder.Entity<ExpertExpertPosition>().HasQueryFilter(p => !p.SoftDelete);
            modelBuilder.Entity<ExpertHeader>().HasQueryFilter(p => !p.SoftDelete);
            modelBuilder.Entity<Instagram>().HasQueryFilter(p => !p.SoftDelete);
            modelBuilder.Entity<Slider>().HasQueryFilter(p => !p.SoftDelete);
            modelBuilder.Entity<Author>().HasQueryFilter(p => !p.SoftDelete);
            modelBuilder.Entity<BlogHeader>().HasQueryFilter(p => !p.SoftDelete);
            modelBuilder.Entity<Subscribe>().HasQueryFilter(p => !p.SoftDelete);
        }

    

    }
}
