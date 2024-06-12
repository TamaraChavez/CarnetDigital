using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;


namespace CarnetDigital.Models
{
    public class FotografiaDAO 
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El correo electrónico es requerido")]
        public string Email { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "La foto es requerida")]
        public string FotoBase64 { get; set; } = null!;


    }
}
