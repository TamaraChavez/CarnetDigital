using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarnetDigital.Models
{
    public class TipoIdentificacionDAO 
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre del tipo de identificación es requerido")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres")]
        public string Nombre { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "El id del tipo de identificacion es requerido")]
        [Range(1, 255, ErrorMessage = "El ID del tipo de identificación debe ser un valor numérico.")]
        public byte TipoIdentificacionID { get; set; }
    }
}
