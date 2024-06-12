using Microsoft.EntityFrameworkCore;
using CarnetDigital.DataAccess.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
//using CarnetDigital.Abstract;
using MiniValidation;
using CarnetDigital.Models;
using System.ComponentModel.DataAnnotations;

namespace CarnetDigital.Users
{
    public static class UsuarioEndpoints
    {
        public static void MapUsuarioEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/Usuario").WithTags(nameof(Usuario));

            //group.MapGet("/", async (CarnetDigitalContext db) =>
            //{
            //    return await db.Usuario.ToListAsync();
            //})
            //.WithName("GetAllUsuarios")
            //.WithOpenApi();



            group.MapGet("/", async (CarnetDigitalContext db) =>
            {
                return await db.Usuario
                    .Select(u => new
                    {
                        u.Email,
                        u.TipoIdentificacionId,
                        u.Identificacion,
                        u.NombreCompleto,
                        u.Contrasena,
                        u.TipoUsuarioId,
                        u.Fiotografia,
                        TelefonoUsuario = u.TelefonoUsuario.Select(t => new
                        {
                            t.Email,
                            t.Telefono
                        }),
                        TipoIdentificacion = new
                        {
                            u.TipoIdentificacion.TipoIdentificacionID,
                            u.TipoIdentificacion.Nombre
                        },
                        TipoUsuario = new
                        {
                            u.TipoUsuario.TipoUsuarioId,
                            u.TipoUsuario.Nombre
                        },
                        Area = u.Area.Select(a => new
                        {
                            a.AreaId,
                            a.Nombre
                        }),
                        Carrera = u.Carrera.Select(c => new
                        {
                            c.CarreraId,
                            c.NombreCarrera
                        })
                    })
                    .ToListAsync();
            })
            .WithName("GetAllUsuarios")
            .WithOpenApi();


            //group.MapGet("/{email}", async Task<Results<Ok<Usuario>, NotFound>> (string email, CarnetDigitalContext db) =>
            //{
            //    var usuario = await db.Usuario.AsNoTracking()
            //        //.Include(u => u.RefreshToken)
            //        .Include(u => u.TelefonoUsuario)
            //        .Include(u => u.TipoIdentificacion)
            //        .Include(u => u.TipoUsuario)
            //        .Include(u => u.Area)
            //        .Include(u => u.Carrera)
            //        .FirstOrDefaultAsync(u => u.Email == email);

            //    return usuario is not null
            //        ? TypedResults.Ok(usuario)
            //        : TypedResults.NotFound();
            //})
            //.WithName("GetUsuarioById")
            //.WithOpenApi();

            group.MapPost("/", async (UsuarioDAO usuarioDAO, CarnetDigitalContext db) =>
            {
                // Validar el objeto UsuarioDAO
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(usuarioDAO);
                bool isValid = Validator.TryValidateObject(usuarioDAO, validationContext, validationResults, true);



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

                if (db.Usuario.Any(u => u.Email == usuarioDAO.Email))
                {
                    var response = new BusinessLogicResponse
                    {
                        StatusCode = 409, // Conflict
                        Message = "El correo electrónico ya está en uso"
                    };
                    return Results.Conflict(response);
                }

                // Crear objeto Usuario a partir de UsuarioDAO después de las validaciones
                var usuario = new Usuario
                {
                    Email = usuarioDAO.Email,
                    TipoIdentificacionId = usuarioDAO.TipoIdentificacionid,
                    Identificacion = usuarioDAO.Identificacion,
                    NombreCompleto = usuarioDAO.NombreCompleto,
                    Contrasena = usuarioDAO.Contrasena,
                    TipoUsuarioId = GetTipoUsuarioId(usuarioDAO.Email, usuarioDAO.TipoUsuarioid, db),
                    Estado = 1,
                    // Si tienes que mapear más propiedades, hazlo aquí
                };


                // Agregar usuario a la base de datos y guardar cambios
                db.Usuario.Add(usuario);
                await db.SaveChangesAsync();

                var createdResponse = new BusinessLogicResponse
                {
                    StatusCode = 201,
                    Message = "Usuario creado exitosamente",
                    ResponseObject = usuario
                };

                return Results.Created($"/api/Usuario/{usuario.Email}", createdResponse);
            })
            .WithName("CreateUsuario")
            .WithOpenApi();

            group.MapPut("/{email}", async (string email, UsuarioDAO usuarioDAO, CarnetDigitalContext db) =>
            {
                // Validar el objeto UsuarioDAO
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(usuarioDAO);
                bool isValid = Validator.TryValidateObject(usuarioDAO, validationContext, validationResults, true);

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

                // Actualizar las propiedades del usuario existente con los valores del objeto UsuarioDAO
                existingUser.TipoIdentificacionId = usuarioDAO.TipoIdentificacionid;
                existingUser.Identificacion = usuarioDAO.Identificacion;
                existingUser.NombreCompleto = usuarioDAO.NombreCompleto;
                existingUser.Contrasena = usuarioDAO.Contrasena;
                existingUser.TipoUsuarioId = GetTipoUsuarioId(usuarioDAO.Email, usuarioDAO.TipoUsuarioid, db);
                // Actualizar más propiedades si es necesario

                // Guardar los cambios en la base de datos
                db.Usuario.Update(existingUser);
                await db.SaveChangesAsync();

                var updatedResponse = new BusinessLogicResponse
                {
                    StatusCode = 200,
                    Message = "Usuario actualizado exitosamente",
                    ResponseObject = existingUser
                };

                return Results.Ok(updatedResponse);
            })
            .WithName("UpdateUsuario")
            .WithOpenApi();


            //group.MapPut("/{email}", async (string email, Usuario usuario, CarnetDigitalContext db) =>
            //{
            //    // Verificar si el usuario existe
            //    var existingUser = await db.Usuario
            //        .Include(u => u.Area)
            //        .Include(u => u.Carrera)
            //        .Include(u => u.TelefonoUsuario)
            //        .FirstOrDefaultAsync(u => u.Email == email);

            //    if (existingUser == null)
            //    {
            //        // Si el usuario no existe, devolver un resultado NotFound
            //        return TypedResults.NotFound();
            //    }

            //    // Validar el objeto UsuarioDTO
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

            //    // Actualizar los datos del usuario existente
            //    existingUser.TipoIdentificacionId = usuario.TipoIdentificacionId;
            //    existingUser.Identificacion = usuario.Identificacion;
            //    existingUser.NombreCompleto = usuario.NombreCompleto;
            //    existingUser.Contrasena = usuario.Contrasena;
            //    existingUser.Fiotografia = usuario.Fiotografia;
            //    existingUser.TipoUsuarioId = usuario.TipoUsuarioId;

            //    // Actualizar las áreas asociadas
            //    existingUser.Area.Clear();
            //    foreach (var area in usuario.Area)
            //    {
            //        var areaToAdd = await db.Area.FindAsync(area.AreaId);
            //        if (areaToAdd != null)
            //        {
            //            existingUser.Area.Add(areaToAdd);
            //        }
            //    }

            //    // Actualizar las carreras asociadas
            //    existingUser.Carrera.Clear();
            //    foreach (var carrera in usuario.Carrera)
            //    {
            //        var carreraToAdd = await db.Carrera.FindAsync(carrera.CarreraId);
            //        if (carreraToAdd != null)
            //        {
            //            existingUser.Carrera.Add(carreraToAdd);
            //        }
            //    }

            //    // Actualizar los números de teléfono asociados
            //    existingUser.TelefonoUsuario.Clear();
            //    foreach (var telefono in usuario.TelefonoUsuario)
            //    {
            //        existingUser.TelefonoUsuario.Add(new TelefonoUsuario
            //        {
            //            Email = telefono.Email,
            //            Telefono = telefono.Telefono
            //        });
            //    }

            //    // Guardar los cambios en la base de datos
            //    await db.SaveChangesAsync();

            //    // Proyección de los datos actualizados en la estructura deseada
            //    var updatedUser = await db.Usuario
            //        .Where(u => u.Email == email)
            //        .Select(u => new
            //        {
            //            u.Email,
            //            u.TipoIdentificacionId,
            //            u.Identificacion,
            //            u.NombreCompleto,
            //            u.Contrasena,
            //            u.TipoUsuarioId,
            //            u.Fiotografia,
            //            TelefonoUsuario = u.TelefonoUsuario.Select(t => new
            //            {
            //                t.Email,
            //                t.Telefono
            //            }),
            //            TipoIdentificacion = new
            //            {
            //                u.TipoIdentificacion.TipoIdentificacionID,
            //                u.TipoIdentificacion.Nombre
            //            },
            //            TipoUsuario = new
            //            {
            //                u.TipoUsuario.TipoUsuarioId,
            //                u.TipoUsuario.Nombre
            //            },
            //            Area = u.Area.Select(a => new
            //            {
            //                a.AreaId,
            //                a.Nombre
            //            }),
            //            Carrera = u.Carrera.Select(c => new
            //            {
            //                c.CarreraId,
            //                c.NombreCarrera
            //            })
            //        }).FirstOrDefaultAsync();

            //    // Devolver el usuario actualizado en la estructura deseada
            //    return Results.Ok(updatedUser);
            //})
            //.WithName("UpdateUsuario")
            //.WithOpenApi();



            //group.MapPost("/", async (Usuario usuario, CarnetDigitalContext db) =>
            //{
            //    usuario.Contrasena = HashPassword(usuario.Contrasena);
            //    usuario.TipoUsuarioId = GetTipoUsuarioId(usuario.Email, usuario.TipoUsuarioId, db);

            //    db.Usuario.Add(usuario);
            //    await db.SaveChangesAsync();
            //    return TypedResults.Created($"/api/Usuario/{usuario.Email}", usuario);
            //})
            //.WithName("CreateUsuariouser")
            //.WithOpenApi();


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