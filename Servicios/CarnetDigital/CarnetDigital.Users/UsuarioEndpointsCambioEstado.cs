
using Microsoft.EntityFrameworkCore;
using CarnetDigital.DataAccess.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using CarnetDigital.Models;
using System.ComponentModel.DataAnnotations;
using CarnetDigital.Catalogs;


namespace CarnetDigital.Users;

public static class UsuarioEndpointsCambioEstado
{
    public static void MapUsuarioEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Usuario").WithTags(nameof(Usuario));
        group.MapPatch("/estado", async Task<IResult> (EstadoDAO usuario, CarnetDigitalContext db) =>

        {
            // Validar el objeto EstadoDAO
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(usuario);
            bool isValid = Validator.TryValidateObject(usuario, validationContext, validationResults, true);

            if (!isValid)
            {
                // Si hay errores de validación, devolverlos en la respuesta
                var response = new BusinessLogicResponse
                {
                    StatusCode = 400,
                    Message = "Errores de validación",
                    ResponseObject = validationResults
                };
                return Results.BadRequest(response);
            }

            // Obtener el usuario existente por correo electrónico
            var existingUser = await db.Usuario.FirstOrDefaultAsync(u => u.Email == usuario.Identificador);
            if (existingUser == null)
            {
                // Si el usuario no existe, devolver un error 404 (Not Found)
                var notFoundResponse = new BusinessLogicResponse
                {
                    StatusCode = 404,
                    Message = "Usuario no encontrado"
                };
                return Results.NotFound(notFoundResponse);
            }

            // Modificar el estado del usuario
            existingUser.Estado = usuario.Estado;

            // Guardar los cambios en la base de datos
            await db.SaveChangesAsync();

            var updatedResponse = new BusinessLogicResponse
            {
                StatusCode = 200,
                Message = "Usuario actualizado exitosamente",
                ResponseObject = existingUser
            };

            return Results.Ok(updatedResponse);
        })
         .WithName("UpdateUsuarioEstado")
         .WithOpenApi();

        //group.MapPatch("/estado", async (string Email, EstadoDAO usuario, CarnetDigitalContext db) =>
        //{
        //    // Validar el objeto UsuarioDAO
        //    var validationResults = new List<ValidationResult>();
        //    var validationContext = new ValidationContext(usuario);
        //    bool isValid = Validator.TryValidateObject(usuario, validationContext, validationResults, true);


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

        //    // Obtener el usuario existente por correo electrónico
        //    var existingUser = await db.Usuario.FirstOrDefaultAsync(u => u.Email == Email);
        //    if (existingUser == null)
        //    {
        //        // Si el usuario no existe, devolver un error 404 (Not Found)
        //        var notFoundResponse = new BusinessLogicResponse
        //        {
        //            StatusCode = 404,
        //            Message = "Usuario no encontrado"
        //        };
        //        return Results.NotFound(notFoundResponse);
        //    }

        //    // Modificar el estado del usuario
        //    existingUser.Estado = usuario.Estado;

        //    // Guardar los cambios en la base de datos
        //    await db.SaveChangesAsync();

        //    var updatedResponse = new BusinessLogicResponse
        //    {
        //        StatusCode = 200,
        //        Message = "Usuario actualizado exitosamente",
        //        ResponseObject = existingUser
        //    };

        //    return Results.Ok(updatedResponse);
        //})
        //    .WithName("UpdateUsuarioEstado")
        //    .WithOpenApi();

    }
}
