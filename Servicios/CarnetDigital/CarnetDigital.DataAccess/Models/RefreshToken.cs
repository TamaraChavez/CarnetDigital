using System;
using System.Collections.Generic;

namespace CarnetDigital.DataAccess.Models;

<<<<<<< HEAD
public class RefreshToken
{
    public int RefreshTokenId { get; set; }
    public string RefreshTokenValue { get; set; } = null!;
    public bool Burned { get; set; }
    public string Email { get; set; } = null!;
    public DateTime ExpirationDate { get; set; }

    // Propiedad de navegación
=======
public partial class RefreshToken
{
    public long RefreshTokenId { get; set; }

    public string RefreshTokenValue { get; set; } = null!;

    public bool Burned { get; set; }

    public string Email { get; set; } = null!;

    public DateTime ExpirationDate { get; set; }

>>>>>>> d95f8b2937232dd174c02909fb7b342e713fcf73
    public virtual Usuario EmailNavigation { get; set; } = null!;
}
