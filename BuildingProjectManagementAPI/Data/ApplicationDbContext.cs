using BuildingProjectManagementAPI.Model.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BuildingProjectManagementAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<UserEntity> Usuarios { get; set; }
        public DbSet<ContactEntity> Contactos { get; set; }
        public DbSet<ProjectEntity> Proyectos { get; set; }
        public DbSet<ProjectContactEntity> ProyectosContactos { get; set; }
    }
}
