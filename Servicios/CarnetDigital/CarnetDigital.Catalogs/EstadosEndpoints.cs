using Microsoft.EntityFrameworkCore;
using CarnetDigital.DataAccess.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace CarnetDigital.Catalogs;

public static class EstadosEndpoints
{
    public static void MapEstadosEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Estados").WithTags(nameof(Estados));

        //group.MapGet("/", async (CarnetDigitalContext db) =>
        //{
        //    return await db.Estados.ToListAsync();
        //})
        //.WithName("GetAllEstados")
        //.WithOpenApi();

        //group.MapGet("/{id}", async Task<Results<Ok<Estados>, NotFound>> (byte estadoid, CarnetDigitalContext db) =>
        //{
        //    return await db.Estados.AsNoTracking()
        //        .FirstOrDefaultAsync(model => model.EstadoId == estadoid)
        //        is Estados model
        //            ? TypedResults.Ok(model)
        //            : TypedResults.NotFound();
        //})
        //.WithName("GetEstadosById")
        //.WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (byte estadoid, Estados estados, CarnetDigitalContext db) =>
        {
            var affected = await db.Estados
                .Where(model => model.EstadoId == estadoid)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.EstadoId, estados.EstadoId)
                    .SetProperty(m => m.Descripcion, estados.Descripcion)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateEstados")
        .WithOpenApi();

        //group.MapPost("/", async (Estados estados, CarnetDigitalContext db) =>
        //{
        //    db.Estados.Add(estados);
        //    await db.SaveChangesAsync();
        //    return TypedResults.Created($"/api/Estados/{estados.EstadoId}",estados);
        //})
        //.WithName("CreateEstados")
        //.WithOpenApi();

        //group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (byte estadoid, CarnetDigitalContext db) =>
        //{
        //    var affected = await db.Estados
        //        .Where(model => model.EstadoId == estadoid)
        //        .ExecuteDeleteAsync();
        //    return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        //})
        //.WithName("DeleteEstados")
        //.WithOpenApi();
    }
}
