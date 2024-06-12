using Microsoft.EntityFrameworkCore;
using CarnetDigital.DataAccess.Models;
using CarnetDigital.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace CarnetDigital.Catalogs;

public static class TipoIdentificacionEndpoints
{
    public static void MapTipoIdentificacionEndpoints(this IEndpointRouteBuilder routes)
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

 group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (byte tipoidentificacionid, CarnetDigitalDbContext db) =>
         {
             var affected = await db.TipoIdentificacion
                 .Where(model => model.TipoIdentificacionId == tipoidentificacionid)
                 .ExecuteDeleteAsync();
             return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
         })
         .WithName("DeleteTipoIdentificacion")
         .WithOpenApi();
group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (byte tipoidentificacionid, TipoIdentificacion tipoIdentificacion, CarnetDigitalDbContext db) =>
        {
            var affected = await db.TipoIdentificacion
                .Where(model => model.TipoIdentificacionId == tipoidentificacionid)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Nombre, tipoIdentificacion.Nombre)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateTipoIdentificacion")
        .WithOpenApi();

group.MapPost("/", async (TipoIdentificacion tI, TipoIdentificacionDAO tipoIdentificacion, CarnetDigitalDbContext db) =>
{
    var validationResults = new List<ValidationResult>();
    var validationContext = new ValidationContext(tipoIdentificacion);
    bool isValid = Validator.TryValidateObject(tipoIdentificacion, validationContext, validationResults, true);

    if (!isValid)
    {
        // Si hay errores de validación, devolverlos en la respuesta
        var response = new BusinessLogicResponse
        {
            StatusCode = 400,
            Message = "Errores de validación",
            Data = validationResults
        };
        return Results.BadRequest(response);
    }
    db.TipoIdentificacion.Add(tI);
    await db.SaveChangesAsync();
    return TypedResults.Created($"/api/TipoIdentificacion/{tI.TipoIdentificacionId}",tipoIdentificacion);
})
.WithName("CreateTipoIdentificacion")
.WithOpenApi();
        
    }

}
