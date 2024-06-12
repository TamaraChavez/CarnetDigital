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
      var group = routes.MapGroup("/usuario/fotografia").WithTags(nameof(Usuario));

      // Actualizar Foto con validación
      group.MapPatch("/{email}", async Task<Results<Ok, NotFound, BadRequest>> (string email, [FromBody] string fotoBase64, CarnetDigitalDbContext db) =>
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


      


            // Eliminar Foto
            group.MapDelete("/{email}", async Task<Results<Ok, NotFound>> (string email, CarnetDigitalDbContext db) =>
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
      group.MapGet("/{email}", async Task<Results<Ok<string>, NotFound>> (string email, CarnetDigitalDbContext db) =>
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