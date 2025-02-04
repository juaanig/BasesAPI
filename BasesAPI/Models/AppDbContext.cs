using BasesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BasesAPI.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Definir índice único para el username de los usuarios
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Configurar la relación entre User y Contact
            modelBuilder.Entity<Contact>()
                .HasOne(c => c.User) // Un contacto pertenece a un usuario
                .WithMany(u => u.Contacts) // Un usuario tiene muchos contactos
                .HasForeignKey(c => c.UserId) // Clave foránea en Contact
                .OnDelete(DeleteBehavior.Cascade); // Elimina contactos al eliminar el usuario

            // Otras configuraciones adicionales (si es necesario)
        }
    }
}
