using Clientes.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);


// EF Core Oracle
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseOracle(builder.Configuration.GetConnectionString("Oracle")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Endpoints CRUD + búsqueda q (RUC o Razón Social)
app.MapGet("/api/clientes", async (string? q, [FromServices] AppDbContext db) =>
{
    var query = db.Clientes.AsQueryable();
    if (!string.IsNullOrWhiteSpace(q))
        query = query.Where(c => c.Ruc.Contains(q) || c.RazonSocial.ToLower().Contains(q.ToLower()));

    var items = await query.OrderBy(c => c.RazonSocial).Take(100).ToListAsync();
    return Results.Ok(items);
});

app.MapGet("/api/clientes/{id:int}", async (int id, [FromServices] AppDbContext db) =>
    await db.Clientes.FindAsync(id) is { } c ? Results.Ok(c) : Results.NotFound());

app.MapPost("/api/clientes", async (Cliente c, [FromServices] AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(c.Ruc) || c.Ruc.Length != 11 || !c.Ruc.All(char.IsDigit))
        return Results.BadRequest("RUC inválido (11 dígitos).");

    db.Clientes.Add(c);
    await db.SaveChangesAsync();
    return Results.Created($"/api/clientes/{c.Id}", c);
});

app.MapPut("/api/clientes/{id:int}", async (int id, Cliente dto, [FromServices] AppDbContext db) =>
{
    var c = await db.Clientes.FindAsync(id);
    if (c is null) return Results.NotFound();

    c.Ruc = dto.Ruc; c.RazonSocial = dto.RazonSocial;
    c.Telefono = dto.Telefono; c.Correo = dto.Correo; c.Direccion = dto.Direccion;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/api/clientes/{id:int}", async (int id, [FromServices] AppDbContext db) =>
{
    var c = await db.Clientes.FindAsync(id);
    if (c is null) return Results.NotFound();
    db.Clientes.Remove(c);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
