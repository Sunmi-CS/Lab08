using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Biblioteca.Web.Models;

namespace Biblioteca.Web.Repositorios;

public class SocioRepositorio : ISocioRepositorio
{
    private readonly IConfiguration _configuration;

    public SocioRepositorio(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private SqlConnection CrearConexion()
    {
        return new SqlConnection(
            _configuration.GetConnectionString("BibliotecaDB"));
    }

    public async Task<IEnumerable<Socio>> ListarAsync()
    {
        using var connection = CrearConexion();

        return await connection.QueryAsync<Socio>(
            "sp_Socios_Listar",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> InsertarAsync(Socio socio)
    {
        using var connection = CrearConexion();

        var parametros = new
        {
            socio.DNI,
            socio.Nombre,
            socio.Email
        };

        return await connection.QuerySingleAsync<int>(
            "sp_Socios_Insertar",
            parametros,
            commandType: CommandType.StoredProcedure);
    }
}