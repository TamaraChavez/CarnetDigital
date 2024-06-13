using System;
using System.Collections.Generic;

namespace CarnetDigital.DataAccess.Models;

public partial class Estado
{
    public byte EstadoId { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
