using Microsoft.EntityFrameworkCore;

namespace Clientes.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Cliente> Clientes => Set<Cliente>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(e =>
            {
                e.ToTable("CLIENTE");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("ID");
                e.Property(x => x.Ruc).HasMaxLength(11).HasColumnName("RUC").IsRequired();
                e.Property(x => x.RazonSocial).HasMaxLength(200).HasColumnName("RAZON_SOCIAL").IsRequired();
                e.Property(x => x.Telefono).HasMaxLength(20).HasColumnName("TELEFONO");
                e.Property(x => x.Correo).HasMaxLength(200).HasColumnName("CORREO");
                e.Property(x => x.Direccion).HasMaxLength(300).HasColumnName("DIRECCION");
            });
        }

    }

}

    

    

    
