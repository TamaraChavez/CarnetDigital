using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;

namespace CarnetDigital.Catalogs

{
    public class EstadoDAO
    {
        [Required( ErrorMessage = "El estado de la persona es requerido")]
        public int EstadoID { get; set; }

        [Required(ErrorMessage = "La descripción es requerido")]
        public string Descripcion { get; set; } = null!;
    }
}
