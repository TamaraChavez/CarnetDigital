using System;
using System.Collections.Generic;

namespace CarnetDigital.DataAccess.Models;

public partial class Estados
{
    public byte EstadoId { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Usuario> Usuario { get; set; } = new List<Usuario>();
}
