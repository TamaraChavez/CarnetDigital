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

namespace CarnetDigital.Pictures
{
  public static class FotografiaEndpoints
  {
    // Este método mapea los endpoints relacionados con las fotografias
    public static void MapFotografiaEndpoints(this IEndpointRouteBuilder routes)
    {
      var group = routes.MapGroup("/usuario").WithTags(nameof(Usuario));

      // Actualizar Foto con validación
      group.MapPatch("/fotografia/{email}", async Task<Results<Ok, NotFound, BadRequest>> (string email, [FromBody] string fotoBase64, CarnetDigitalDbContext db) =>
      {
        if (string.IsNullOrEmpty(fotoBase64))
        {
          return TypedResults.BadRequest();
        }

        var usuario = await db.Usuario.FirstOrDefaultAsync(u => u.Email == email);
        if (usuario == null)
        {
          return TypedResults.NotFound();
        }

        usuario.Fotografia = fotoBase64;
        await db.SaveChangesAsync();

        return TypedResults.Ok();
      })
      .WithName("ModificarFotografia")
      .WithOpenApi();


      group.MapPatch("/fotografia{email}", async Task<BusinessLogicResponse> (string email, [FromBody] fotoBase64 FotografiaDAO, CarnetDigitalDbContext db) =>
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


            // Eliminar Foto
            group.MapDelete("/fotografia/{email}", async Task<Results<Ok, NotFound>> (string email, CarnetDigitalDbContext db) =>
      {
        var usuario = await db.Usuario.FirstOrDefaultAsync(u => u.Email == email);
          if (usuario == null)
        {
          return TypedResults.NotFound();
        }

        usuario.Fotografia = null;
        await db.SaveChangesAsync();

        return TypedResults.Ok();
      })
      .WithName("BorrarFotografia")
      .WithOpenApi();

      // Obtener Foto
      group.MapGet("/fotografia/{email}", async Task<Results<Ok<string>, NotFound>> (string email, CarnetDigitalDbContext db) =>
      {
        var usuario = await db.Usuario.FirstOrDefaultAsync(u => u.Email == email);
        if (usuario == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(usuario.Fotografia);
      })
      .WithName("GetUsuarioFotografia")
      .WithOpenApi();
    }
  }
}