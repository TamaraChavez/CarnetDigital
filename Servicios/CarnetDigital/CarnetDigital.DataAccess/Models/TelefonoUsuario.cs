using System;
using System.Collections.Generic;

namespace CarnetDigital.Login.Models;

public partial class TelefonoUsuario
{
    public string Email { get; set; } = null!;

    public int Telefono { get; set; }

    public virtual Usuario EmailNavigation { get; set; } = null!;
}
