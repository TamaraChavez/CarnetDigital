using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using CarnetDigital.DataAccess.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.CodeAnalysis.Scripting;

namespace CarnetDigital.Users
{
    public static class UsuarioEndpoints
    {
        public static void MapUsuarioEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/Usuario").WithTags(nameof(Usuario));

            group.MapGet("/", async (CarnetDigitalContext db) =>
            {
                return await db.Usuario.Include(u => u.Area).Include(u => u.Carrera).Include(u => u.TelefonoUsuario).ToListAsync();
            })
            .WithName("GetAllUsuarios")
            .WithOpenApi();

            group.MapGet("/{email}", async Task<Results<Ok<Usuario>, NotFound>> (string email, CarnetDigitalContext db) =>
            {
                var user = await db.Usuario.Include(u => u.Area).Include(u => u.Carrera).Include(u => u.TelefonoUsuario)
                    .FirstOrDefaultAsync(model => model.Email == email);

                return user is not null ? TypedResults.Ok(user) : TypedResults.NotFound();
            })
            .WithName("GetUsuarioById")
            .WithOpenApi();

            group.MapPut("/{email}", async Task<Results<Ok, NotFound>> (string email, Usuario usuario, CarnetDigitalContext db) =>
            {
                var existingUser = await db.Usuario
                    .Include(u => u.Area)
                    .Include(u => u.Carrera)
                    .Include(u => u.TelefonoUsuario)
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (existingUser == null)
                {
                    return TypedResults.NotFound();
                }

                existingUser.TipoIdentificacionId = usuario.TipoIdentificacionId;
                existingUser.Identificacion = usuario.Identificacion;
                existingUser.NombreCompleto = usuario.NombreCompleto;
                existingUser.Contrasena = HashPassword(usuario.Contrasena);
                existingUser.Estado = usuario.Estado;
                existingUser.TipoUsuarioId = GetTipoUsuarioId(usuario.Email, usuario.TipoUsuarioId, db);

                // Update associated areas
                existingUser.Area.Clear();
                foreach (var area in usuario.Area)
                {
                    var areaToAdd = await db.Area.FindAsync(area.AreaId);
                    if (areaToAdd != null)
                    {
                        existingUser.Area.Add(areaToAdd);
                    }
                }

                // Update associated careers
                existingUser.Carrera.Clear();
                foreach (var carrera in usuario.Carrera)
                {
                    var carreraToAdd = await db.Carrera.FindAsync(carrera.CarreraId);
                    if (carreraToAdd != null)
                    {
                        existingUser.Carrera.Add(carreraToAdd);
                    }
                }

                // Update associated phone numbers
                existingUser.TelefonoUsuario.Clear();
                foreach (var telefono in usuario.TelefonoUsuario)
                {
                    existingUser.TelefonoUsuario.Add(new TelefonoUsuario
                    {
                        Email = usuario.Email,
                        Telefono = telefono.Telefono
                    });
                }

                await db.SaveChangesAsync();
                return TypedResults.Ok();
            })
            .WithName("UpdateUsuario")
            .WithOpenApi();

            group.MapPost("/", async (Usuario usuario, CarnetDigitalContext db) =>
            {
                usuario.Contrasena = HashPassword(usuario.Contrasena);
                usuario.TipoUsuarioId = GetTipoUsuarioId(usuario.Email, usuario.TipoUsuarioId, db);

                db.Usuario.Add(usuario);
                await db.SaveChangesAsync();
                return TypedResults.Created($"/api/Usuario/{usuario.Email}", usuario);
            })
            .WithName("CreateUsuario")
            .WithOpenApi();

            group.MapDelete("/{email}", async Task<Results<Ok, NotFound>> (string email, CarnetDigitalContext db) =>
            {
                var affected = await db.Usuario
                    .Where(model => model.Email == email)
                    .ExecuteDeleteAsync();
                return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
            })
            .WithName("DeleteUsuario")
            .WithOpenApi();
        }

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        private static byte GetTipoUsuarioId(string email, byte requestedTipoUsuarioId, CarnetDigitalContext db)
        {
            if (email.EndsWith("@cuc.ac.cr"))
            {
                var tipoUsuario = db.TipoUsuario.FirstOrDefault(tu => tu.Nombre == "funcionario" || tu.Nombre == "administrador");
                if (tipoUsuario != null)
                {
                    return tipoUsuario.TipoUsuarioId;
                }
            }
            return requestedTipoUsuarioId;
        }
    }
}
