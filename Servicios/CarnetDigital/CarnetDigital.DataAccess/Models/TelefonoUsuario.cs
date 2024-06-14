using System;
using System.Collections.Generic;

namespace CarnetDigital.DataAccess.Models;

<<<<<<< HEAD
public class TelefonoUsuario
{
    public string Email { get; set; } = null!;
    public int? Telefono { get; set; }

    // Propiedad de navegación
=======
public partial class TelefonoUsuario
{
    public string Email { get; set; } = null!;

    public int Telefono { get; set; }

>>>>>>> d95f8b2937232dd174c02909fb7b342e713fcf73
    public virtual Usuario EmailNavigation { get; set; } = null!;
}
