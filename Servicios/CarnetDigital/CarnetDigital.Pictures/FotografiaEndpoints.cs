using Microsoft.EntityFrameworkCore;
using CarnetDigital.DataAccess.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Authorization;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace CarnetDigital.Pictures
{
  public static class FotografiaEndpoints
  {
    // Este método mapea los endpoints relacionados con las fotografias
    public static void MapFotografiaEndpoints(this IEndpointRouteBuilder routes)
    {
      var group = routes.MapGroup("/usuario").WithTags(nameof(Usuario));

      // Actualizar Foto con validación
      group.MapPatch("/fotografia/{id}", async Task<Results<Ok, NotFound, BadRequest>> (string id, [FromBody] string fotografiaBase64, CarnetDigitalDbContext db) =>
      {
        if (string.IsNullOrEmpty(fotografiaBase64))
        {
          return TypedResults.BadRequest();
        }

        var usuario = await db.Usuario.FindAsync(id);
        if (usuario == null)
        {
          return TypedResults.NotFound();
        }

        usuario.Fotografia = fotografiaBase64;
        await db.SaveChangesAsync();

        return TypedResults.Ok();
      })
      .WithName("ModificarFotografia")
      .WithOpenApi();

      // Eliminar Foto
      group.MapDelete("/fotografia/{id}", async Task<Results<Ok, NotFound>> (string id, CarnetDigitalDbContext db) =>
      {
        var usuario = await db.Usuario.FindAsync(id);
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