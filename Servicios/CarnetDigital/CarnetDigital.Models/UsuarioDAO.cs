using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarnetDigital.Models
{
    public enum TipoUsuario
     {
            Funcionario = 1,
            Estudiante = 2,
            Administrador = 3 
      }

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

        [Required(ErrorMessage = "El estado es requerido")]
        [Range(1, 3, ErrorMessage = "El tipo de estado debe ser 1, 2 o 3")] // No se cuantos tipos de estado hay
        public string Estado { get; set; } = null!;

        [Required(ErrorMessage = "El tipo de usuario es requerido")]
        [EnumDataType(typeof(TipoUsuario), ErrorMessage = "El tipo de usuario debe ser un valor válido")]
        public TipoUsuario TipoUsuario { get; set; }

        public string? Telefono { get; set; } = null!;//?OPCIONAL

        public List<string>? Carreras { get; set; } = new List<string>();

        public List<string>? Areas { get; set; } = new List<string>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            // Validación del dominio del correo electrónico y tipo de usuario
            var emailDomain = Email.Split('@').Last();
            if (emailDomain == "cuc.cr" && TipoUsuario != TipoUsuario.Estudiante)
            {
                results.Add(new ValidationResult("Si el dominio es cuc.cr, el tipo de usuario debe ser estudiante.", new[] { nameof(Email), nameof(TipoUsuario) }));
            }
            else if (emailDomain == "cuc.ac.cr" && (TipoUsuario != TipoUsuario.Funcionario && TipoUsuario != TipoUsuario.Administrador))
            {
                results.Add(new ValidationResult("Si el dominio es cuc.ac.cr, el tipo de usuario debe ser funcionario o administrador.", new[] { nameof(Email), nameof(TipoUsuario) }));
            }

            // Validación de carreras y áreas asociadas
            if (TipoUsuario == TipoUsuario.Estudiante && (Carreras == null || !Carreras.Any()))
            {
                results.Add(new ValidationResult("Los estudiantes deben tener al menos una carrera asociada.", new[] { nameof(Carreras) }));
            }
            if (TipoUsuario == TipoUsuario.Funcionario && (Areas == null || !Areas.Any()))
            {
                results.Add(new ValidationResult("Los funcionarios deben tener al menos un área asociada.", new[] { nameof(Areas) }));
            }

            return results;
        }
    }
}
