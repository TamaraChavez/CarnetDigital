using System;
using System.Collections.Generic;

namespace CarnetDigital.DataAccess.Models;

public class TelefonoUsuario
{
    public string Email { get; set; } = null!;
    public int? Telefono { get; set; }

    // Propiedad de navegación
    public virtual Usuario EmailNavigation { get; set; } = null!;
}
