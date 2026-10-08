using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Biblioteca.Web.Models;

namespace Biblioteca.Web.Repositorios;

public class PrestamoRepositorio : IPrestamoRepositorio
{
    private readonly IConfiguration _configuration;

    public PrestamoRepositorio(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private SqlConnection CrearConexion()
    {
        return new SqlConnection(
            _configuration.GetConnectionString("BibliotecaDB"));
    }

    public async Task<IEnumerable<PrestamoReporte>> ReporteAsync(
        DateTime fechaInicio,
        DateTime fechaFin)
    {
        using var connection = CrearConexion();

        var parametros = new
        {
            FechaInicio = fechaInicio.Date,
            FechaFin = fechaFin.Date
        };

        return await connection.QueryAsync<PrestamoReporte>(
            "sp_Prestamos_ReportePorFechas",
            parametros,
            commandType: CommandType.StoredProcedure);
    }
}