using System;
using System.Collections.Generic;

namespace CarnetDigital.DataAccess.Models;

public partial class TipoUsuario
{
    public byte TipoUsuarioId { get; set; }

    public string Nombre { get; set; } = null!;

<<<<<<< HEAD
    public virtual ICollection<Usuario> Usuario { get; set; } = new List<Usuario>();
=======
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
>>>>>>> d95f8b2937232dd174c02909fb7b342e713fcf73
}
