using Microsoft.EntityFrameworkCore;
using CarnetDigital.DataAccess.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Authorization;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using CarnetDigital.BusinessLogic;
using CarnetDigital.Models;
using System.ComponentModel.DataAnnotations;

namespace CarnetDigital.Pictures
{
    public static class FotografiaEndpoints
    {
        // Este método mapea los endpoints relacionados con las fotografias
        public static void MapFotografiaEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/usuario/fotografia").WithTags(nameof(Usuario));

            // Actualizar Foto con validación
            group.MapPatch("/{email}", async Task<BusinessLogicResponse> (
                string email,
                [FromBody] FotografiaDAO fotografiaDAO,
                CarnetDigitalDbContext db) =>
            {
                if (!ValidationHelper.Validate(fotografiaDAO, out var validationResults))
                {
                    return new BusinessLogicResponse(400, string.Join("; ", validationResults.Select(r => r.ErrorMessage)));
                }

                var usuario = await db.Usuario.FirstOrDefaultAsync(u => u.Email == email);
                if (usuario == null)
                {
                    return new BusinessLogicResponse(404, "Usuario no encontrado.");
                }

                usuario.Fotografia = fotografiaDAO.FotoBase64;
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
}