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
        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre del tipo de identificacion es requerido")]
        [Range(1, 3, ErrorMessage = "El tipo de identificacion debe ser Cédula de Identidad, Pasaporte, Visa o Licencia de Conducir")]
        public byte TipoIdentificacion { get; set; }

    }
}
