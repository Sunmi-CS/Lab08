using Biblioteca.Web.Models;

namespace Biblioteca.Web.Repositorios;

public interface IPrestamoRepositorio
{
    Task<IEnumerable<PrestamoReporte>> ReporteAsync(
        DateTime fechaInicio,
        DateTime fechaFin);
}