using Microsoft.EntityFrameworkCore;
using CarnetDigital.DataAccess.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace CarnetDigital.Users;

public static class UsuarioEndpoints
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
                    .SetProperty(m => m.TipoIdentificacionId, usuario.TipoIdentificacionId)
                    .SetProperty(m => m.Identificacion, usuario.Identificacion)
                    .SetProperty(m => m.NombreCompleto, usuario.NombreCompleto)
                    .SetProperty(m => m.Contrasena, usuario.Contrasena)
                    //.SetProperty(m => m.Estado, usuario.Estado)
                    .SetProperty(m => m.TipoUsuarioId, usuario.TipoUsuarioId)
                    .SetProperty(m => m.Fiotografia, usuario.Fiotografia)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateUsuario")
        .WithOpenApi();

        group.MapPost("/", async (Usuario usuario, CarnetDigitalContext db) =>
        {
            db.Usuario.Add(usuario);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Usuario/{usuario.Email}",usuario);
        })
        .WithName("CreateUsuario")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (string email, CarnetDigitalContext db) =>
        {
            var affected = await db.Usuario
                .Where(model => model.Email == email)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteUsuario")
        .WithOpenApi();
    }
}
