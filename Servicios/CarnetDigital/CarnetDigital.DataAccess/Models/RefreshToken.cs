using System;
using System.Collections.Generic;

namespace CarnetDigital.DataAccess.Models;

public class RefreshToken
{
    public int RefreshTokenId { get; set; }
    public string RefreshTokenValue { get; set; } = null!;
    public bool Burned { get; set; }
    public string Email { get; set; } = null!;
    public DateTime ExpirationDate { get; set; }

    // Propiedad de navegación
    public virtual Usuario EmailNavigation { get; set; } = null!;
}
