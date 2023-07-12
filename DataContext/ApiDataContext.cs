using API_Eventos.Models;
using API_Eventos.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace API_Eventos.DataContext
{
    public class ApiDataContext : DbContext
    {
        public ApiDataContext(DbContextOptions<ApiDataContext> options) : base(options)
        {

        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Events> Events { get; set; }
        public DbSet<Registrations> Registrations { get; set; }
        public DbSet<Reviews> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersMap());
            modelBuilder.ApplyConfiguration(new CategoriesMap());
            modelBuilder.ApplyConfiguration(new EventsMap());
            modelBuilder.ApplyConfiguration(new RegistrationsMap());
            modelBuilder.ApplyConfiguration(new ReviewsMap());
        }
    }
}