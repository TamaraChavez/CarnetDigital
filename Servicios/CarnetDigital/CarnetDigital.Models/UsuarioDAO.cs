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
        [Required(AllowEmptyStrings = false,ErrorMessage = "Email de la persona requrido")]
        public string Email { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "tipo de ID de la persona requrido")]
        public byte TipoIdentificacionId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "ID de la perosna requrido")]
        public string Identificacion { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Nombre completo de la persona requrido")]
        public string NombreCompleto { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Contrase\u00F1a de la persona requerida")]
        public string Contrasena { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Estado de la persona requrido")]
        public byte Estado { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor indique tipo de usarios")]
        public byte TipoUsuarioId { get; set; }


       
    }

}
