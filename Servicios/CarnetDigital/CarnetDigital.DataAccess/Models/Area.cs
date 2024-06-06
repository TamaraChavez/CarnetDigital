using System;
using System.Collections.Generic;

namespace CarnetDigital.Login.Models;

public partial class Area
{
    public byte AreaId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Usuario> Email { get; set; } = new List<Usuario>();
}
