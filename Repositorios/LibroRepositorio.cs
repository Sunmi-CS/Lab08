using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Biblioteca.Web.Models;

namespace Biblioteca.Web.Repositorios;

public class LibroRepositorio : ILibroRepositorio
{
    private readonly IConfiguration _configuration;

    public LibroRepositorio(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private SqlConnection CrearConexion()
    {
        return new SqlConnection(
            _configuration.GetConnectionString("BibliotecaDB"));
    }

    public async Task<IEnumerable<Libro>> ListarAsync()
    {
        using var connection = CrearConexion();

        return await connection.QueryAsync<Libro>(
            "sp_Libros_Listar",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Libro>> BuscarAsync(string texto)
    {
        using var connection = CrearConexion();

        var parametros = new
        {
            Texto = texto
        };

        return await connection.QueryAsync<Libro>(
            "sp_Libros_Buscar",
            parametros,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Libro?> ObtenerPorIdAsync(int id)
    {
        using var connection = CrearConexion();

        var parametros = new
        {
            LibroId = id
        };

        return await connection.QueryFirstOrDefaultAsync<Libro>(
            "sp_Libros_ObtenerPorId",
            parametros,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Autor>> ListarAutoresAsync()
    {
        using var connection = CrearConexion();

        return await connection.QueryAsync<Autor>(
            "sp_Autores_Listar",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> InsertarAsync(Libro libro)
    {
        using var connection = CrearConexion();

        var parametros = new
        {
            libro.Titulo,
            libro.ISBN,
            libro.AutorId,
            libro.Ejemplares
        };

        return await connection.QuerySingleAsync<int>(
            "sp_Libros_Insertar",
            parametros,
            commandType: CommandType.StoredProcedure);
    }

    public async Task ActualizarAsync(Libro libro)
    {
        using var connection = CrearConexion();

        var parametros = new
        {
            libro.LibroId,
            libro.Titulo,
            libro.ISBN,
            libro.AutorId,
            libro.Ejemplares
        };

        await connection.ExecuteAsync(
            "sp_Libros_Actualizar",
            parametros,
            commandType: CommandType.StoredProcedure);
    }

    public async Task EliminarAsync(int id)
    {
        using var connection = CrearConexion();

        var parametros = new
        {
            LibroId = id
        };

        await connection.ExecuteAsync(
            "sp_Libros_Eliminar",
            parametros,
            commandType: CommandType.StoredProcedure);
    }
}