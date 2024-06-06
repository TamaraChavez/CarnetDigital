using System;
using System.Collections.Generic;

namespace CarnetDigital.Login.Models;

public partial class RefreshToken
{
    public long RefreshTokenId { get; set; }

    public string RefreshTokenValue { get; set; } = null!;

    public bool Burned { get; set; }

    public string Email { get; set; } = null!;

    public DateTime ExpirationDate { get; set; }

    public virtual Usuario EmailNavigation { get; set; } = null!;
}
