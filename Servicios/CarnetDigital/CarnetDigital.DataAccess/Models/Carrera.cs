using System;
using System.Collections.Generic;

namespace CarnetDigital.Login.Models;

public partial class Carrera
{
    public string CarreraId { get; set; } = null!;

    public string NombreCarrera { get; set; } = null!;

    public string DirectorCarrera { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int Telefono { get; set; }

    public virtual ICollection<Usuario> EmailNavigation { get; set; } = new List<Usuario>();
}
