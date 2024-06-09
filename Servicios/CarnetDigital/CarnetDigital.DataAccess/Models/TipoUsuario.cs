using System;
using System.Collections.Generic;

namespace CarnetDigital.DataAccess.Models;

public partial class TipoUsuario
{
    public byte TipoUsuarioId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
