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

group.MapPost("/", async (TipoIdentificacionDAO tipoIdentificacion, CarnetDigitalDbContext db) =>
{
    // Validar el objeto UsuarioDAO
            //    var validationResults = new List<ValidationResult>();
            //    var validationContext = new ValidationContext(usuarioDAO);
            //    bool isValid = Validator.TryValidateObject(usuarioDAO, validationContext, validationResults, true);

            //    if (!isValid)
            //    {
            //        // Si hay errores de validación, devolverlos en la respuesta
            //        var response = new BusinessLogicResponse
            //        {
            //            StatusCode = 400,
            //            Message = "Errores de validación",
            //            ResponseObject = validationResults
            //        };
            //        return Results.BadRequest(response);
            //    }

            //    if (db.Usuario.Any(u => u.Email == usuarioDAO.Email))
            //    {
            //        var response = new BusinessLogicResponse
            //        {
            //            StatusCode = 409, // Conflict
            //            Message = "El correo electrónico ya está en uso"
            //        };
            //        return Results.Conflict(response);
            //    }

            //    // Crear objeto Usuario a partir de UsuarioDAO después de las validaciones
            //    var usuario = new Usuario
            //    {
            //        Email = usuarioDAO.Email,
            //        TipoIdentificacionId = usuarioDAO.TipoIdentificacionid,
            //        Identificacion = usuarioDAO.Identificacion,
            //        NombreCompleto = usuarioDAO.NombreCompleto,
            //        Contrasena = usuarioDAO.Contrasena,
            //        TipoUsuarioId = GetTipoUsuarioId(usuarioDAO.Email, usuarioDAO.TipoUsuarioid, db),
            //        Estado = 1,
            //        // Si tienes que mapear más propiedades, hazlo aquí
            //    };


            //    // Agregar usuario a la base de datos y guardar cambios
            //    db.Usuario.Add(usuario);
            //    await db.SaveChangesAsync();

            //    var createdResponse = new BusinessLogicResponse
            //    {
            //        StatusCode = 201,
            //        Message = "Usuario creado exitosamente",
            //        ResponseObject = usuario
            //    };

            //    return Results.Created($"/api/Usuario/{usuario.Email}", createdResponse);
            //})
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
    var TIPO = new TipoIdentificacion
    {
        Nombre = tipoIdentificacion.Nombre,
        TipoIdentificacionId = tipoIdentificacion.TipoIdentificacionID,

        // Si tienes que mapear más propiedades, hazlo aquí
    };
    db.TipoIdentificacion.Add(TIPO);
    await db.SaveChangesAsync();

    var createdResponse = new BusinessLogicResponse
    {
        StatusCode = 201,
        Message = "Usuario creado exitosamente",
        Data = tipoIdentificacion
    };

    return TypedResults.Created($"/api/TipoIdentificacion/{TIPO.TipoIdentificacionId}", createdResponse);
    //return TypedResults.Created($"/api/TipoIdentificacion/{tI.TipoIdentificacionId}",tipoIdentificacion);
})
.WithName("CreateTipoIdentificacion")
.WithOpenApi();
        
    }

}
