using System;
using System.Collections.Generic;

namespace CarnetDigital.DataAccess.Models;

public partial class TipoIdentificacion
{
<<<<<<< HEAD
    public byte TipoIdentificacionID { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Usuario> Usuario { get; set; } = new List<Usuario>();
=======
    public byte Email { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
>>>>>>> d95f8b2937232dd174c02909fb7b342e713fcf73
}
