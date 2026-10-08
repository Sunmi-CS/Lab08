using Biblioteca.Web.Models;

namespace Biblioteca.Web.Repositorios;

public interface ILibroRepositorio
{
    Task<IEnumerable<Libro>> ListarAsync();

    Task<IEnumerable<Libro>> BuscarAsync(string texto);

    Task<Libro?> ObtenerPorIdAsync(int id);

    Task<IEnumerable<Autor>> ListarAutoresAsync();

    Task<int> InsertarAsync(Libro libro);

    Task ActualizarAsync(Libro libro);

    Task EliminarAsync(int id);
}