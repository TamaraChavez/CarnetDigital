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
        group.MapPatch("/{id}", async Task<BusinessLogicResponse> (
            byte tipoidentificacionid,
            [FromBody] TipoIdentificacionDAO tipoIdentificacionDAO,
            CarnetDigitalDbContext db) =>
        {
            var tipoIdentificacion = await db.TipoIdentificacion.FindAsync(tipoidentificacionid);
            if (tipoIdentificacion == null)
            {
                return new BusinessLogicResponse(404, "Tipo de identificación no encontrado.");
            }

            // Validar el objeto TipoIdentificacionDAO
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(tipoIdentificacionDAO, new ValidationContext(tipoIdentificacionDAO), validationResults, true))
            {
                return new BusinessLogicResponse(400, string.Join("; ", validationResults.Select(r => r.ErrorMessage)));
            }

            tipoIdentificacion.Nombre = tipoIdentificacionDAO.Nombre;

            await db.SaveChangesAsync();

            return new BusinessLogicResponse(200, "Tipo de identificación actualizado exitosamente.");
        })
        .WithName("PartialUpdateTipoIdentificacion")
        .WithOpenApi();



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

      
        group.MapPost("/", async Task<BusinessLogicResponse> ([FromBody] TipoIdentificacionDAO tipoIdentificacionDAO, CarnetDigitalDbContext db) =>
        {
            if (!ValidationHelper.Validate(tipoIdentificacionDAO, out var validationResults))
            {
                return new BusinessLogicResponse(400, string.Join("; ", validationResults.Select(r => r.ErrorMessage)));
            }

            var existingTipoIdentificacion = await db.TipoIdentificacion
                .FirstOrDefaultAsync(model => model.TipoIdentificacionId == tipoIdentificacionDAO.TipoIdentificacionID);

            if (existingTipoIdentificacion != null)
            {
                return new BusinessLogicResponse(400, "El ID del tipo de identificación ya existe.");
            }

            var tipoIdentificacion = new TipoIdentificacion
            {
                TipoIdentificacionId = tipoIdentificacionDAO.TipoIdentificacionID,
                Nombre = tipoIdentificacionDAO.Nombre
            };

            db.TipoIdentificacion.Add(tipoIdentificacion);
            var affected = await db.SaveChangesAsync();

            return affected == 1
                ? new BusinessLogicResponse(201, "TipoIdentificacion creado Exitosamente.", tipoIdentificacion)
                : new BusinessLogicResponse(400, "Falló al crear: TipoIdentificacion.");
        })
       .WithName("CreateTipoIdentificacion")
       .WithOpenApi();
    }
    public static class ValidationHelper
    {
        public static bool Validate<T>(T model, out List<ValidationResult> results)
        {
            var context = new ValidationContext(model, serviceProvider: null, items: null);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(model, context, results, true);
        }
    }

}

