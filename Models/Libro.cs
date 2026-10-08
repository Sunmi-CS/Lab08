using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Models;

public class Libro
{
    public int LibroId { get; set; }

    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(150, ErrorMessage = "El título no puede superar los 150 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El ISBN es obligatorio.")]
    [StringLength(20, ErrorMessage = "El ISBN no puede superar los 20 caracteres.")]
    public string ISBN { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe seleccionar un autor.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un autor válido.")]
    public int AutorId { get; set; }

    public string NombreAutor { get; set; } = string.Empty;

    [Range(0, 100000, ErrorMessage = "Los ejemplares deben ser mayores o iguales a 0.")]
    public int Ejemplares { get; set; }

    public bool Activo { get; set; }
}