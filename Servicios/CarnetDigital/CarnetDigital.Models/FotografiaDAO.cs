using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;


namespace CarnetDigital.Models
{
    public class FotografiaDAO : IValidatableObject
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El correo electrónico es requerido")]
        public string Email { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "La foto es requerida")]
        public string FotoBase64 { get; set; } = null!;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            // Validación de la foto
            if (!ValidarFoto(FotoBase64, out var validationMessage))
            {
                results.Add(new ValidationResult(validationMessage, new[] { nameof(FotoBase64) }));
            }

            return results;
        }

        public static bool ValidarFoto(string base64Image, out string validationMessage)
        {
            validationMessage = string.Empty;

            try
            {
                // Convertir la cadena Base64 a un arreglo de bytes
                byte[] imageBytes = Convert.FromBase64String(base64Image);

                // Cargar el arreglo de bytes en una memoria
                using (var ms = new MemoryStream(imageBytes))
                {
                    // Cargar la memoria en una imagen
                    using (var image = Image.FromStream(ms))
                    {
                        // Checar el formato de la imagen
                        if (image.RawFormat.Equals(ImageFormat.Jpeg) || image.RawFormat.Equals(ImageFormat.Png))
                        {
                            // Checar tamaño
                            const int maxSizeInBytes = 1 * 1024 * 1024; // 1 MB
                            if (imageBytes.Length <= maxSizeInBytes)
                            {
                                return true; // Todas las validaciones aprobadas
                            }
                            else
                            {
                                validationMessage = "El tamaño de la imagen no debe superar los 1MB.";
                                return false;
                            }
                        }
                        else
                        {
                            validationMessage = "La imagen debe estar en formato JPEG o PNG.";
                            return false;
                        }
                    }
                }
            }
            catch (FormatException)
            {
                validationMessage = "La cadena Base64 no es válida.";
                return false;
            }
            catch (Exception ex)
            {
                validationMessage = $"La imagen no es válida o está dañada: {ex.Message}";
                return false;
            }
        }
    }
}
