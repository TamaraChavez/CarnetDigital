using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CarnetDigital.Models
{
    public enum TipoUsuario
    {
        Funcionario = 1,
        Estudiante = 2,
        Administrador = 3
    }

    // Atributo personalizado para validar que un string no esté vacío ni contenga solo espacios en blanco
    public class NoEmptyOrWhiteSpaceAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string stringValue)
            {
                if (string.IsNullOrWhiteSpace(stringValue))
                {
                    return new ValidationResult("El campo no puede estar vacío ni contener solo espacios en blanco.");
                }
            }
            return ValidationResult.Success;
        }
    }

    public class UsuarioDAO : IValidatableObject
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El correo electrónico de la persona es requerido")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "El tipo de identificación es requerido")]
        [Range(1, 3, ErrorMessage = "El tipo de identificación debe ser 1, 2 o 3")] // Ajusta el rango según sea necesario
        public byte TipoIdentificacion { get; set; } 

        [Required(AllowEmptyStrings = false, ErrorMessage = "La identificación es requerida")]
        public string Identificacion { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre completo es requerido")]
        [NoEmptyOrWhiteSpace(ErrorMessage = "El nombre completo no puede estar vacío ni contener solo espacios en blanco.")]
        public string NombreCompleto { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "La contraseña es requerida")]
        public string Contrasena { get; set; } = null!;

        //[Required(ErrorMessage = "El estado es requerido")]
        //[Range(1, 3, ErrorMessage = "El tipo de estado debe ser 1, 2 o 3")] // No se cuantos tipos de estado hay
        //public string Estado { get; set; } = null!;

        [Required(ErrorMessage = "El tipo de usuario es requerido")]
        [EnumDataType(typeof(TipoUsuario), ErrorMessage = "El tipo de usuario debe ser un valor válido")]
        public TipoUsuario TipoUsuario { get; set; }

        public string? Telefono { get; set; } = null!;//?OPCIONAL

        public List<string>? Carrera { get; set; } = new List<string>();

        public List<string>? Area { get; set; } = new List<string>();


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
            if (TipoUsuario == TipoUsuario.Estudiante && (Carrera == null || !Carrera.Any()))
            {
                results.Add(new ValidationResult("Los estudiantes deben tener al menos una carrera asociada.", new[] { nameof(Carrera) }));
            }
            if (TipoUsuario == TipoUsuario.Funcionario && (Area == null || !Area.Any()))
            {
                results.Add(new ValidationResult("Los funcionarios deben tener al menos un área asociada.", new[] { nameof(Area) }));
            }

            return results;
        }
    }
}
