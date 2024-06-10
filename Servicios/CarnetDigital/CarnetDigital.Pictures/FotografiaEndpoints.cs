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
      var group = routes.MapGroup("/api/Usuario").WithTags(nameof(Usuario));

      // Actualizar Foto con validación
      group.MapPut("/fotografia/{id}", [Authorize] async Task<Results<Ok, NotFound, BadRequest>> (string id, [FromBody] string fotografiaBase64, CarnetDigitalDbContext db) =>
      {
        if (string.IsNullOrEmpty(fotografiaBase64))
        {
          return TypedResults.BadRequest();
        }

        // Validar el formato y tamaño de la foto
        if (!TryValidatePhoto(fotografiaBase64, out var validationMessage))
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
      .WithName("UpdateFotografia")
      .WithOpenApi();

      // Eliminar Foto
      group.MapDelete("/fotografia/{id}", [Authorize] async Task<Results<Ok, NotFound>> (string id, CarnetDigitalDbContext db) =>
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
      .WithName("DeleteFotografia")
      .WithOpenApi();

      // Obtener Foto
      group.MapGet("/fotografia/{id}", [Authorize] async Task<Results<Ok<string>, NotFound>> (string id, CarnetDigitalDbContext db) =>
      {
        var usuario = await db.Usuario.FindAsync(id);
        if (usuario == null)
        {
          return TypedResults.NotFound();
        }

        return TypedResults.Ok(usuario.Fotografia);
      })
      .WithName("GetFotografia")
      .WithOpenApi();
    }

    // Este método intenta validar la foto recibida
    private static bool TryValidatePhoto(string base64Image, out string validationMessage)
    {
      validationMessage = string.Empty;

      try
      {
        // Convertir la cadena Base64 en un array de bytes
        byte[] imageBytes = Convert.FromBase64String(base64Image);

        // Cargar el array de bytes en un MemoryStream
        using (var ms = new MemoryStream(imageBytes))
        {
          // Cargar la imagen desde el MemoryStream
          using (var image = Image.FromStream(ms))
          {
            // Verificar el formato de la imagen
            if (image.RawFormat != ImageFormat.Jpeg && image.RawFormat != ImageFormat.Png)
            {
              validationMessage = "La imagen debe estar en formato JPEG o PNG.";
              return false;
            }

            // Verificar las dimensiones de la imagen (relación de aspecto 4:3)
            if (image.Width * 3 != image.Height * 4)
            {
              validationMessage = "La imagen debe tener una proporción de aspecto 4:3.";
              return false;
            }

            // Verificar el tamaño de la imagen (por ejemplo, máximo 2MB)
            const int maxSizeInBytes = 2 * 1024 * 1024;
            if (imageBytes.Length > maxSizeInBytes)
            {
              validationMessage = "El tamaño de la imagen no debe superar los 2MB.";
              return false;
            }
          }
        }
      }
      catch (Exception)
      {
        validationMessage = "La imagen no es válida o está dañada.";
        return false;
      }

      return true;
    }
  }
}