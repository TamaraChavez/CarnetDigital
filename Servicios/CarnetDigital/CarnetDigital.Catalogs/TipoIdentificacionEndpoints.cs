using Microsoft.EntityFrameworkCore;
using CarnetDigital.DataAccess.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace CarnetDigital.Catalogs;

public static class TipoIdentificacionEndpoints
{
    public static void MapTipoIdentificacionEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/TipoIdentificacion").WithTags(nameof(TipoIdentificacion));

        group.MapGet("/", async (CarnetDigitalDbContext db) =>
        {
            return await db.TipoIdentificacion.ToListAsync();
        })
        .WithName("GetAllTipoIdentificacions")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<TipoIdentificacion>, NotFound>> (byte tipoidentificacionid, CarnetDigitalDbContext db) =>
        {
            return await db.TipoIdentificacion.AsNoTracking()
                .FirstOrDefaultAsync(model => model.TipoIdentificacionId == tipoidentificacionid)
                is TipoIdentificacion model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetTipoIdentificacionById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (byte tipoidentificacionid, TipoIdentificacion tipoIdentificacion, CarnetDigitalDbContext db) =>
        {
            var affected = await db.TipoIdentificacion
                .Where(model => model.TipoIdentificacionId == tipoidentificacionid)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.TipoIdentificacionId, tipoIdentificacion.TipoIdentificacionId)
                    .SetProperty(m => m.Nombre, tipoIdentificacion.Nombre)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateTipoIdentificacion")
        .WithOpenApi();

        group.MapPost("/", async (TipoIdentificacion tipoIdentificacion, CarnetDigitalDbContext db) =>
        {
            db.TipoIdentificacion.Add(tipoIdentificacion);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/TipoIdentificacion/{tipoIdentificacion.TipoIdentificacionId}",tipoIdentificacion);
        })
        .WithName("CreateTipoIdentificacion")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (byte tipoidentificacionid, CarnetDigitalDbContext db) =>
        {
            var affected = await db.TipoIdentificacion
                .Where(model => model.TipoIdentificacionId == tipoidentificacionid)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteTipoIdentificacion")
        .WithOpenApi();
    }
}
