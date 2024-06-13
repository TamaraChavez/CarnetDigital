using System;
using System.Collections.Generic;

namespace CarnetDigital.DataAccess.Models;

public partial class Carrera
{
    public string CarreraId { get; set; } = null!;

    public string NombreCarrera { get; set; } = null!;

    public string DirectorCarrera { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int Telefono { get; set; }

<<<<<<< HEAD
    public virtual ICollection<Usuario> EmailNavigation { get; set; } = new List<Usuario>();
=======
    public virtual ICollection<Usuario> Emails { get; set; } = new List<Usuario>();
>>>>>>> d95f8b2937232dd174c02909fb7b342e713fcf73
}
