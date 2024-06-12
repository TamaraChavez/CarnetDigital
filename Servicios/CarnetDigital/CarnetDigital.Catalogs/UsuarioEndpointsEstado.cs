using Microsoft.EntityFrameworkCore;
using CarnetDigital.DataAccess.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using CarnetDigital.Models;
namespace CarnetDigital.Catalogs;

public static class UsuarioEndpointsEstado
{
    public static void MapUsuarioEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Usuario").WithTags(nameof(Usuario));

        group.MapGet("/", async (CarnetDigitalContext db) =>
        {
            return await db.Usuario.ToListAsync();
        })
        .WithName("GetAllUsuarios")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Usuario>, NotFound>> (string email, CarnetDigitalContext db) =>
        {
            return await db.Usuario.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Email == email)
                is Usuario model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetUsuarioById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (string email, Usuario usuario, CarnetDigitalContext db) =>
        {
            var affected = await db.Usuario
                .Where(model => model.Email == email)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Email, usuario.Email)
                    .SetProperty(m => m.Estado, usuario.Estado)

                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateUsuario")
        .WithOpenApi();

        group.MapPatch("/usuarios/estado", async Task<Results<Ok, NotFound, BadRequest>> (string email, Usuario usuario, CarnetDigitalContext db) =>
        {
            // Aquí deberías agregar tu lógica de validación y asignar el resultado a isValid y validationResults
            bool isValid = true; // Cambia esto a tu lógica de validación
            var validationResults = new List<string>(); // Lista de errores de validación

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
            var existingUser = await db.Usuario.FirstOrDefaultAsync(u => u.Email == email);
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


    }
}
