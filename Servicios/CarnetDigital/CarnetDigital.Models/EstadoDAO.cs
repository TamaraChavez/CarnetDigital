using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;

namespace CarnetDigital.Catalogs

{
    public class EstadoDAO
    {
        [NoEmptyOrWhiteSpaceAttributeEmail( ErrorMessage = "")]
        public int Email { get; set; }

        [NoEmptyOrWhiteSpaceAttribute(ErrorMessage = "")]
        public byte EstadoID { get; set; } 

        public class NoEmptyOrWhiteSpaceAttributeEmail : ValidationAttribute
        {
            protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
            {
                if (value is string stringValue)
                {
                    if (string.IsNullOrWhiteSpace(stringValue))
                    {
                        return new ValidationResult("El email no puede estar vacío ni contener solo espacios en blanco.");
                    }
                }
                return ValidationResult.Success;
            }
        }

        public class NoEmptyOrWhiteSpaceAttribute : ValidationAttribute
        {
            protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
            {
                if (value is string stringValue)
                {
                    if (string.IsNullOrWhiteSpace(stringValue))
                    {
                        return new ValidationResult("El código del estado no puede estar vacío ni contener solo espacios en blanco.");
                    }
                }
                return ValidationResult.Success;
            }
        }

    }
}
