using System;
using System.Collections.Generic;

namespace CarnetDigital.DataAccess.Models;

<<<<<<< HEAD
public class Usuario
{
    public string Email { get; set; } = null!;
    public byte TipoIdentificacionId { get; set; }
    public string Identificacion { get; set; } = null!;
    public string NombreCompleto { get; set; } = null!;
    public string Contrasena { get; set; } = null!;
    public byte Estado { get; set; }
    public byte TipoUsuarioId { get; set; }
    public string? Fiotografia { get; set; }

    //public virtual Estados EstadoNavigation { get; set; } = null!;
    public virtual ICollection<RefreshToken> RefreshToken { get; set; } = new List<RefreshToken>();
    public virtual ICollection<TelefonoUsuario> TelefonoUsuario { get; set; } = new List<TelefonoUsuario>();
    public virtual TipoIdentificacion TipoIdentificacion { get; set; } = null!;
    public virtual TipoUsuario TipoUsuario { get; set; } = null!;
    public virtual ICollection<Area> Area { get; set; } = new List<Area>();
    public virtual ICollection<Carrera> Carrera { get; set; } = new List<Carrera>();

}
    //public string Email { get; set; } = null!;

    //public byte TipoIdentificacionId { get; set; }

    //public string Identificacion { get; set; } = null!;

    //public string NombreCompleto { get; set; } = null!;

    //public string Contrasena { get; set; } = null!;

    ////

    //public byte TipoUsuarioId { get; set; }

    //public string? Fiotografia { get; set; }


    //public virtual ICollection<RefreshToken> RefreshToken { get; set; } = new List<RefreshToken>();

    //public virtual ICollection<TelefonoUsuario> TelefonoUsuario { get; set; } = new List<TelefonoUsuario>();

    //public virtual TipoIdentificacion TipoIdentificacion { get; set; } = null!;

    //public virtual TipoUsuario TipoUsuario { get; set; } = null!;

    //public virtual ICollection<Area> Area { get; set; } = new List<Area>();

    //public virtual ICollection<Carrera> Carrera { get; set; } = new List<Carrera>();
=======
public partial class Usuario
{
    public string Email { get; set; } = null!;

    public byte TipoIdentificacionId { get; set; }

    public string Identificacion { get; set; } = null!;

    public string NombreCompleto { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public byte Estado { get; set; }

    public byte TipoUsuarioId { get; set; }

    public string? Fiotografia { get; set; }

    public virtual Estado EstadoNavigation { get; set; } = null!;

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual ICollection<TelefonoUsuario> TelefonoUsuarios { get; set; } = new List<TelefonoUsuario>();

    public virtual TipoIdentificacion TipoIdentificacion { get; set; } = null!;

    public virtual TipoUsuario TipoUsuario { get; set; } = null!;

    public virtual ICollection<Area> Areas { get; set; } = new List<Area>();

    public virtual ICollection<Carrera> Carreras { get; set; } = new List<Carrera>();
}
>>>>>>> d95f8b2937232dd174c02909fb7b342e713fcf73
