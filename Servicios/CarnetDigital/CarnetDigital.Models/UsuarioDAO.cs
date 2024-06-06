using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarnetDigital.Models
{
    public class UsuarioDAO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El correo electronico de la persona es requerido")]//NO PERMITE QUE EL STRING VENGA VACIO
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "El tipo de identificacion es requerido")]
        [Range(1, 3, ErrorMessage = "El tipo de identificacion debe ser 1, 2 o 3")] // No se cuantos tipos de identificacion hay
        public byte TipoIdentificacion { get; set; }//

        [Required(AllowEmptyStrings = false, ErrorMessage = "La identificacion es requerida")]
        public string Identificacion { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre completo es requerido")]
        public string NombreCompleto { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "La contraseña es requerida")]
        public string Contrasena { get; set; } = null!;


        [Required(ErrorMessage = "El tipo de identificacion es requerido")]
        [Range(1, 3, ErrorMessage = "El tipo de identificacion debe ser 1, 2 o 3")] // No se cuantos tipos de identificacion hay

        public string? Telefono { get; set; } = null!;//?OPCIONAL

        [Required(AllowEmptyStrings = false, ErrorMessage = "El rol de la persona es requerido")]
        public string Rol { get; set; } = null!;
    }
}
