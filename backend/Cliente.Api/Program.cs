using Clientes.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseOracle(builder.Configuration.GetConnectionString("Oracle")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Endpoints CRUD + búsqueda q (RUC o Razón Social)
app.MapGet("/api/clientes", async (string? q, AppDbContext db) =>
{
    var query = db.Clientes.AsQueryable();
    if (!string.IsNullOrWhiteSpace(q))
        query = query.Where(c => c.Ruc.Contains(q) || c.RazonSocial.ToLower().Contains(q.ToLower()));

    var items = await query.OrderBy(c => c.RazonSocial).Take(100).ToListAsync();
    return Results.Ok(items);
});

app.MapGet("/api/clientes/{id:int}", async (int id, AppDbContext db) =>
    await db.Clientes.FindAsync(id) is { } c ? Results.Ok(c) : Results.NotFound());

app.MapPost("/api/clientes", async (Cliente c, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(c.Ruc) || c.Ruc.Length != 11 || !c.Ruc.All(char.IsDigit))
        return Results.BadRequest("RUC inválido (11 dígitos).");

    db.Clientes.Add(c);
    await db.SaveChangesAsync();
    return Results.Created($"/api/clientes/{c.Id}", c);
});

app.MapPut("/api/clientes/{id:int}", async (int id, Cliente dto, AppDbContext db) =>
{
    var c = await db.Clientes.FindAsync(id);
    if (c is null) return Results.NotFound();

    c.Ruc = dto.Ruc; c.RazonSocial = dto.RazonSocial;
    c.Telefono = dto.Telefono; c.Correo = dto.Correo; c.Direccion = dto.Direccion;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/api/clientes/{id:int}", async (int id, AppDbContext db) =>
{
    var c = await db.Clientes.FindAsync(id);
    if (c is null) return Results.NotFound();
    db.Clientes.Remove(c);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();

namespace Clientes.Api.Data
{
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }
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

    public class Cliente
    {
        public int Id { get; set; }
        public string Ruc { get; set; } = null!;
        public string RazonSocial { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? Direccion { get; set; }
    }
}
