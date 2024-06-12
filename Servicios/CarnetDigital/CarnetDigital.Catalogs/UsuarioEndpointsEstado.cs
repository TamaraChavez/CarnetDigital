using Microsoft.EntityFrameworkCore;
using CarnetDigital.DataAccess.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
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


    }
}
