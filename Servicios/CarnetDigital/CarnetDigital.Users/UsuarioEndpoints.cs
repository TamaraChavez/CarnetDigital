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

            group.MapPost("/", async (UsuarioDAO usuarioDAO, CarnetDigitalContext db) =>
            {
                // Validar el objeto UsuarioDAO
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(usuarioDAO);
                bool isValid = Validator.TryValidateObject(usuarioDAO, validationContext, validationResults, true);

                // Validar dominios y tipos de usuario
                isValid = isValid && ValidarDominioYTipoUsuario(usuarioDAO, validationResults);

                // Validar asociaciones según tipo de usuario
                isValid = isValid && ValidarAsociaciones(usuarioDAO, validationResults);

                if (!isValid)
                {
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
                    Contrasena = HashPassword(usuarioDAO.Contrasena),
                    TipoUsuarioId = GetTipoUsuarioId(usuarioDAO.Email, usuarioDAO.TipoUsuarioid, db),
                    Estado = 1
                };

                // Agregar Teléfonos
                foreach (var telefono in usuarioDAO.TelefonoUsuario)
                {
                    usuario.TelefonoUsuario.Add(new TelefonoUsuario
                    {
                        Email = telefono.Email,
                        Telefono = telefono.Telefono
                    });
                }

                // Agregar Carreras
                if (usuarioDAO.TipoUsuarioid == 2)
                {
                    foreach (var carrera in usuarioDAO.Carrera)
                    {
                        var carreraEntity = await db.Carrera.FindAsync(carrera.CarreraId);
                        if (carreraEntity != null)
                        {
                            usuario.Carrera.Add(carreraEntity);
                        }
                    }
                }

                // Agregar Áreas
                if (usuarioDAO.TipoUsuarioid == 1)
                {
                    foreach (var area in usuarioDAO.Area)
                    {
                        var areaEntity = await db.Area.FindAsync(area.AreaId);
                        if (areaEntity != null)
                        {
                            usuario.Area.Add(areaEntity);
                        }
                    }
                }

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


            //group.MapPost("/", async (UsuarioDAO usuarioDAO, CarnetDigitalContext db) =>
            //{
            //    // Validar el objeto UsuarioDAO
            //    var validationResults = new List<ValidationResult>();
            //    var validationContext = new ValidationContext(usuarioDAO);
            //    bool isValid = Validator.TryValidateObject(usuarioDAO, validationContext, validationResults, true);

            //    // Validar dominios y tipos de usuario
            //    isValid = isValid && ValidarDominioYTipoUsuario(usuarioDAO, validationResults);

            //    // Validar asociaciones según tipo de usuario
            //    isValid = isValid && ValidarAsociaciones(usuarioDAO, validationResults);

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

            //    if (db.Usuario.Any(u => u.Email == usuarioDAO.Email))
            //    {
            //        var response = new BusinessLogicResponse
            //        {
            //            StatusCode = 409, // Conflict
            //            Message = "El correo electrónico ya está en uso"
            //        };
            //        return Results.Conflict(response);
            //    }

            //    // Crear objeto Usuario a partir de UsuarioDAO después de las validaciones
            //    var usuario = new Usuario
            //    {
            //        Email = usuarioDAO.Email,
            //        TipoIdentificacionId = usuarioDAO.TipoIdentificacionid,
            //        Identificacion = usuarioDAO.Identificacion,
            //        NombreCompleto = usuarioDAO.NombreCompleto,
            //        Contrasena = HashPassword(usuarioDAO.Contrasena),
            //        TipoUsuarioId = GetTipoUsuarioId(usuarioDAO.Email, usuarioDAO.TipoUsuarioid, db),
            //        Estado = 1,
            //        // Si tienes que mapear más propiedades, hazlo aquí
            //    };

            //    // Agregar usuario a la base de datos y guardar cambios
            //    db.Usuario.Add(usuario);
            //    await db.SaveChangesAsync();

            //    var createdResponse = new BusinessLogicResponse
            //    {
            //        StatusCode = 201,
            //        Message = "Usuario creado exitosamente",
            //        ResponseObject = usuario
            //    };

            //    return Results.Created($"/api/Usuario/{usuario.Email}", createdResponse);
            //})
            //    .WithName("CreateUsuario")
            //    .WithOpenApi();

            group.MapPut("/{email}", async (string email, UsuarioDAO usuarioDAO, CarnetDigitalContext db) =>
            {
                // Validar el objeto UsuarioDAO
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(usuarioDAO);
                bool isValid = Validator.TryValidateObject(usuarioDAO, validationContext, validationResults, true);

                // Validar dominios y tipos de usuario
                isValid = isValid && ValidarDominioYTipoUsuario(usuarioDAO, validationResults);

                // Validar asociaciones según tipo de usuario
                isValid = isValid && ValidarAsociaciones(usuarioDAO, validationResults);

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
                existingUser.Contrasena = HashPassword(usuarioDAO.Contrasena);
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


        }

        private static bool ValidarDominioYTipoUsuario(UsuarioDAO usuarioDAO, List<ValidationResult> validationResults)
         {
            var email = usuarioDAO.Email;
            var dominio = email.Split('@').Last();
            var tipoUsuario = usuarioDAO.TipoUsuarioid;

            if (dominio == "cuc.cr" && tipoUsuario != 2)
            {
                validationResults.Add(new ValidationResult("El dominio cuc.cr solo permite el tipo de usuario Estudiante (2)."));
                return false;
            }
            if (dominio == "cuc.ac.cr" && (tipoUsuario != 1 && tipoUsuario != 3))
            {
                validationResults.Add(new ValidationResult("El dominio cuc.ac.cr solo permite el tipo de usuario Funcionario (1) o Administrador (3)."));
                return false;
            }
            return true;
        }

        private static bool ValidarAsociaciones(UsuarioDAO usuarioDAO, List<ValidationResult> validationResults)
        {
            if (usuarioDAO.TipoUsuarioid == 2 && (usuarioDAO.Carrera == null || !usuarioDAO.Carrera.Any()))
            {
                validationResults.Add(new ValidationResult("Los estudiantes deben tener al menos una carrera asociada."));
                return false;
            }
            if (usuarioDAO.TipoUsuarioid == 1 && (usuarioDAO.Area == null || !usuarioDAO.Area.Any()))
            {
                validationResults.Add(new ValidationResult("Los funcionarios deben tener al menos una area asociada."));
                return false;
            }
            return true;
        }

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private static bool VerificarPassword(string password, string hashedPassword)
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

            //group.MapPost("/", async (UsuarioDAO usuarioDAO, CarnetDigitalContext db) =>
            //{
            //    // Validar el objeto UsuarioDAO
            //    var validationResults = new List<ValidationResult>();
            //    var validationContext = new ValidationContext(usuarioDAO);
            //    bool isValid = Validator.TryValidateObject(usuarioDAO, validationContext, validationResults, true);

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

            //    if (db.Usuario.Any(u => u.Email == usuarioDAO.Email))
            //    {
            //        var response = new BusinessLogicResponse
            //        {
            //            StatusCode = 409, // Conflict
            //            Message = "El correo electrónico ya está en uso"
            //        };
            //        return Results.Conflict(response);
            //    }

            //    // Crear objeto Usuario a partir de UsuarioDAO después de las validaciones
            //    var usuario = new Usuario
            //    {
            //        Email = usuarioDAO.Email,
            //        TipoIdentificacionId = usuarioDAO.TipoIdentificacionid,
            //        Identificacion = usuarioDAO.Identificacion,
            //        NombreCompleto = usuarioDAO.NombreCompleto,
            //        Contrasena = HashPassword(usuarioDAO.Contrasena),
            //        TipoUsuarioId = GetTipoUsuarioId(usuarioDAO.Email, usuarioDAO.TipoUsuarioid, db),
            //        Estado = 1,
            //        // Si tienes que mapear más propiedades, hazlo aquí
            //    };


            //    // Agregar usuario a la base de datos y guardar cambios
            //    db.Usuario.Add(usuario);
            //    await db.SaveChangesAsync();

            //    var createdResponse = new BusinessLogicResponse
            //    {
            //        StatusCode = 201,
            //        Message = "Usuario creado exitosamente",
            //        ResponseObject = usuario
            //    };

            //    return Results.Created($"/api/Usuario/{usuario.Email}", createdResponse);
            //})
            //.WithName("CreateUsuario")
            //.WithOpenApi();

            //group.MapPut("/{email}", async (string email, UsuarioDAO usuarioDAO, CarnetDigitalContext db) =>
            //{
            //    // Validar el objeto UsuarioDAO
            //    var validationResults = new List<ValidationResult>();
            //    var validationContext = new ValidationContext(usuarioDAO);
            //    bool isValid = Validator.TryValidateObject(usuarioDAO, validationContext, validationResults, true);

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
            //    var existingUser = await db.Usuario.FirstOrDefaultAsync(u => u.Email == email);
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

            //    // Actualizar las propiedades del usuario existente con los valores del objeto UsuarioDAO
            //    existingUser.TipoIdentificacionId = usuarioDAO.TipoIdentificacionid;
            //    existingUser.Identificacion = usuarioDAO.Identificacion;
            //    existingUser.NombreCompleto = usuarioDAO.NombreCompleto;
            //    existingUser.Contrasena = HashPassword(usuarioDAO.Contrasena);
            //    existingUser.TipoUsuarioId = GetTipoUsuarioId(usuarioDAO.Email, usuarioDAO.TipoUsuarioid, db);
            //    // Actualizar más propiedades si es necesario

            //    // Guardar los cambios en la base de datos
            //    db.Usuario.Update(existingUser);
            //    await db.SaveChangesAsync();

            //    var updatedResponse = new BusinessLogicResponse
            //    {
            //        StatusCode = 200,
            //        Message = "Usuario actualizado exitosamente",
            //        ResponseObject = existingUser
            //    };

            //    return Results.Ok(updatedResponse);
            //})
            //.WithName("UpdateUsuario")
            //.WithOpenApi();



            //group.MapDelete("/{email}", async Task<Results<Ok, NotFound>> (string email, CarnetDigitalContext db) =>
            //{
            //    var affected = await db.Usuario
            //        .Where(model => model.Email == email)
            //        .ExecuteDeleteAsync();
            //    return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
            //})
            //.WithName("DeleteUsuario")
            //.WithOpenApi();
