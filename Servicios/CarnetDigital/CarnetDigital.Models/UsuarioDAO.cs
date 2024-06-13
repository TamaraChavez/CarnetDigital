using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;

namespace CarnetDigital.Models
{
    public enum TipoUsuarioff
    {
        Funcionario = 1,
        Estudiante = 2,
        Administrador = 3
    }

    public class UsuarioDAO
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "El correo electrónico de la persona es requerido")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        [CustomEmailDomainValidator(new string[] { "cuc.cr", "cuc.ac.cr" }, ErrorMessage = "El dominio del correo electrónico debe ser cuc.cr o cuc.ac.cr")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "El tipo de identificación es requerido")]
        [Range(1, 3, ErrorMessage = "El tipo de identificación debe ser 1, 2 o 3")]
        public byte TipoIdentificacionid { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "La identificación es requerida")]
        public string Identificacion { get; set; } = null!;

        [NoEmptyOrWhiteSpace(ErrorMessage = "El nombre completo no puede estar vacío ni contener solo espacios en blanco.")]
        public string NombreCompleto { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "La contraseña es requerida")]
        [MinLength(8, ErrorMessage = "La contraseña no puede tener menos de 8 caracteres")]
        public string Contrasena { get; set; } = null!;

        [Required(ErrorMessage = "El tipo de usuario es requerido")]
        [Range(1, 3, ErrorMessage = "El tipo de usuario debe ser 1 (Funcionario), 2 (Estudiante) o 3 (Administrador)")]
        public byte TipoUsuarioid { get; set; }

        public int TelefonoUsuario { get; set; }

        public string? Fiotografia { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "La carrera es requerida")]
        public List<CarreraDAO> Carrera { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "El área es requerida")]
        public List<AreaDAO> Area { get; set; } = null!;

        public class NoEmptyOrWhiteSpaceAttribute : ValidationAttribute
        {
            protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
            {
                if (value is string stringValue)
                {
                    if (string.IsNullOrWhiteSpace(stringValue))
                    {
                        return new ValidationResult("El nombre completo no puede estar vacío ni contener solo espacios en blanco.");
                    }
                }
                return ValidationResult.Success;
            }
        }

        public class CustomEmailDomainValidator : ValidationAttribute
        {
            private readonly string[] _domains;

            public CustomEmailDomainValidator(string[] domains)
            {
                _domains = domains;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var email = value as string;
                if (email != null)
                {
                    var domain = email.Split('@').Last();
                    if (!_domains.Contains(domain))
                    {
                        return new ValidationResult(ErrorMessage);
                    }
                }
                return ValidationResult.Success;
            }
        }



        //[Required(AllowEmptyStrings = false, ErrorMessage = "El correo electrónico de la persona es requerido")]
        //public string Email { get; set; } = null!;

        //[Required(ErrorMessage = "El tipo de identificación es requerido")]
        //[Range(1, 3, ErrorMessage = "El tipo de identificación debe ser 1, 2 o 3")]
        //public byte TipoIdentificacionid { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "La identificación es requerida")]
        //public string Identificacion { get; set; } = null!;

        ////[Required(AllowEmptyStrings = false, ErrorMessage = "El nombre completo es requerido")]
        //[NoEmptyOrWhiteSpace(ErrorMessage = "")]
        //public string NombreCompleto { get; set; } = null!;

        //[Required(AllowEmptyStrings = false, ErrorMessage = "La contraseña es requerida")]
        //[MinLength(8, ErrorMessage = "La contraseña no puede tener menos de 8 caracteres")]
        //public string Contrasena { get; set; } = null!;

        //[Required(ErrorMessage = "El tipo de usuario es requerido")]
        //[Range(1, 3, ErrorMessage = "El tipo de usuario debe ser 1 (Funcionario), 2 (Estudiante) o 3 (Administrador)")]
        //public byte TipoUsuarioid { get; set; }

        //public int Telefono { get; set; }

        //public string? Fiotografia { get; set; } = null!;

        //[Required(AllowEmptyStrings = false, ErrorMessage = "La carrera es requerida")]
        //public List<CarreraDAO> Carrera { get; set; } = null!;

        //[Required(AllowEmptyStrings = false, ErrorMessage = "El área es requerida")]
        //public List<AreaDAO> Area { get; set; } = null!;


        //public class NoEmptyOrWhiteSpaceAttribute : ValidationAttribute
        //{
        //    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        //    {
        //        if (value is string stringValue)
        //        {
        //            if (string.IsNullOrWhiteSpace(stringValue))
        //            {
        //                return new ValidationResult("El nombre completo no puede estar vacío ni contener solo espacios en blanco.");
        //            }
        //        }
        //        return ValidationResult.Success;
        //    }
        //}
    }

    public class CarreraDAO
    {
        public string CarreraId { get; set; } = null!;
        public string NombreCarrera { get; set; } = null!;
    }

    public class AreaDAO
    {
        public int AreaId { get; set; }
        public string Nombre { get; set; } = null!;
    }

}
        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    var results = new List<ValidationResult>();
        //    var emailDomain = Email.Split('@').Last();

        //    if (emailDomain == "cuc.cr" && TipoUsuarioid != 2)
        //    {
        //        results.Add(new ValidationResult("Si el dominio es cuc.cr, el tipo de usuario debe ser estudiante.", new[] { nameof(Email), nameof(TipoUsuarioid) }));
        //    }
        //    else if (emailDomain == "cuc.ac.cr" && (TipoUsuarioid != 1 && TipoUsuarioid != 3))
        //    {
        //        results.Add(new ValidationResult("Si el dominio es cuc.ac.cr, el tipo de usuario debe ser funcionario o administrador.", new[] { nameof(Email), nameof(TipoUsuarioid) }));
        //    }

        //    if (TipoUsuarioid == 2 && (Carrera == null || !Carrera.Any()))
        //    {
        //        results.Add(new ValidationResult("Los estudiantes deben tener al menos una carrera asociada.", new[] { nameof(Carrera) }));
        //    }
        //    if (TipoUsuarioid == 1 && (Area == null || !Area.Any()))
        //    {
        //        results.Add(new ValidationResult("Los funcionarios deben tener al menos un área asociada.", new[] { nameof(Area) }));
        //    }

        //    return results;
        //}

