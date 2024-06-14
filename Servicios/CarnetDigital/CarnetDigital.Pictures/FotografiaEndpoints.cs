using Microsoft.EntityFrameworkCore;
using CarnetDigital.DataAccess.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using CarnetDigital.BusinessLogic;
using CarnetDigital.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
namespace CarnetDigital.Pictures;

public static class FotografiaEndpoints
{
    public static void MapUsuarioEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Usuario").WithTags(nameof(Usuario));


        // Actualizar Foto con validación
        group.MapPatch("/{email}", async Task<BusinessLogicResponse> (
     string email,
     [FromBody] FotografiaDAO fotografiaDAO,
     CarnetDigitalDbContext db) =>
        {
            // Validar el modelo FotografiaDAO
            if (!ValidationHelper.Validate(fotografiaDAO, out var validationResults))
            {
                return new BusinessLogicResponse(400, string.Join("; ", validationResults.Select(r => r.ErrorMessage)));
            }

            // Verificar si el usuario existe
            var usuario = await db.Usuario.FirstOrDefaultAsync(u => u.Email == fotografiaDAO.Email);
            if (usuario == null)
            {
                return new BusinessLogicResponse(404, "Usuario no encontrado.");
            }

            // Actualizar la fotografía del usuario
            usuario.Fotografia = fotografiaDAO.Fotografia;
            await db.SaveChangesAsync();

            return new BusinessLogicResponse(200, "Fotografía actualizada exitosamente.");
        })
 .WithName("ModificarFotografia")
 .WithOpenApi();


        // Eliminar Foto
        group.MapDelete("/{email}", async Task<BusinessLogicResponse> (
            string email,
            CarnetDigitalDbContext db) =>
        {
            var usuario = await db.Usuario.FirstOrDefaultAsync(u => u.Email == email);
            if (usuario == null)
            {
                return new BusinessLogicResponse(404, "Usuario no encontrado.");
            }

            usuario.Fotografia = null;
            await db.SaveChangesAsync();

            return new BusinessLogicResponse(200, "Fotografía eliminada exitosamente.");
        })
        .WithName("BorrarFotografia")
        .WithOpenApi();

        // Obtener Foto
        group.MapGet("/{email}", async Task<BusinessLogicResponse> (
            string email,
            CarnetDigitalDbContext db) =>
        {
            var usuario = await db.Usuario.FirstOrDefaultAsync(u => u.Email == email);
            if (usuario == null)
            {
                return new BusinessLogicResponse(404, "Usuario no encontrado.");
            }

            return new BusinessLogicResponse(200, "Fotografía obtenida exitosamente.", usuario.Fotografia);
        })
        .WithName("GetUsuarioFotografia")
        .WithOpenApi();

        /*group.MapPost("/", async Task<BusinessLogicResponse> (
         [FromBody] FotografiaDAO fotografiaDAO,
         CarnetDigitalDbContext db) =>
        {
            if (!ValidationHelper.Validate(fotografiaDAO, out var validationResults))
            {
                return new BusinessLogicResponse(400, string.Join("; ", validationResults.Select(r => r.ErrorMessage)));
            }

            var usuario = await db.Usuario.FirstOrDefaultAsync(u => u.Email == fotografiaDAO.Email);
            if (usuario == null)
            {
                return new BusinessLogicResponse(404, "Usuario no encontrado.");
            }

            if (!string.IsNullOrEmpty(usuario.Fotografia))
            {
                return new BusinessLogicResponse(400, "El usuario ya tiene una fotografía.");
            }

            usuario.Fotografia = fotografiaDAO.Fotografia;
            await db.SaveChangesAsync();

            return new BusinessLogicResponse(201, "Fotografía agregada exitosamente.");
        })
       .WithName("AgregarFotografia")
       .WithOpenApi();
        */




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
