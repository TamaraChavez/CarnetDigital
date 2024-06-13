using CarnetDigital.Abstract;
using CarnetDigital.BusinessLogic;
using CarnetDigital.DataAccess;
using CarnetDigital.DataAccess.Models;
using CarnetDigital.Models;
using Microsoft.EntityFrameworkCore;
using MiniValidation;
using Microsoft.AspNetCore.Mvc;
using Azure;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Tiusr24plContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Registrar dependencias de los servicios
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

//Endpoint de login
app.MapPost("/login", async (
    [FromHeader(Name = "Email")] string email,
    [FromHeader(Name = "Password")] string password,
    [FromHeader(Name = "TipoUsuarioId")] string tipoUsuarioId,
    IAuthenticationService authService,
    Tiusr24plContext db) =>
{
   
        User user = new User()
        {
            Email = email,
            Password = password,
            TipoUsuarioId = int.Parse(tipoUsuarioId),
        };

    //Validar que los datos
    if (!MiniValidator.TryValidate(user, out var errors))
    {
        return Results.BadRequest(
            new
            {
                codigo = -2,
                mensaje = "Errores de validación",
                errores = errors
            }
        );
    }

    //Autenticar al usuario
    var tokenResponse = await authService.AuthenticateAsync(email, password, int.Parse(tipoUsuarioId));
    return tokenResponse.StatusCode switch
    {
        201 => Results.Ok(tokenResponse.ObjectResponse),
        _ => Results.Json(new {mensaje = "Usuario y/o contraseña incorrectos" },
                          statusCode: 401)

    };

});

//Endpoint de validacion de token
app.MapGet("/validate", async (ITokenService tokenService, [FromQuery] string token) =>
{
    if (string.IsNullOrEmpty(token))
    {
        return Results.Unauthorized();
    }
    //Validar el token
    var isValid = await tokenService.ValidateTokenAsync(token);
        
    return isValid.StatusCode switch
    {
        200 => Results.Json(new { valido = isValid.Message }),
        _ => Results.Unauthorized()
    };
    
});

//Endpoint de validar/refrescar token
app.MapPost("/refresh", async (IAuthenticationService authService, [FromQuery] string refreshToken) =>
{
    if (string.IsNullOrEmpty(refreshToken))
    {
        return Results.Unauthorized();
    }

    //Validar y refrescar el token
    var newTokens = await authService.ValidateRefeshTokenAsync(refreshToken.Replace(" ", "+"));
    
    return newTokens.StatusCode switch
    {
        201 => Results.Ok(newTokens.ObjectResponse),
        _ => Results.Json(new { mensaje = "No autorizado" },
                          statusCode: 401)
    };
});




app.Run();


