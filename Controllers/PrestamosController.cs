using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Web.Controllers;

public class PrestamosController : Controller
{
    private readonly IPrestamoRepositorio _repositorio;

    public PrestamosController(IPrestamoRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    [HttpGet]
    public async Task<IActionResult> Reporte(
        DateTime? desde,
        DateTime? hasta)
    {
        ViewData["Title"] = "Reporte de préstamos";

        var fechaDesde = desde ?? DateTime.Today.AddMonths(-1);
        var fechaHasta = hasta ?? DateTime.Today;

        if (fechaDesde > fechaHasta)
        {
            ModelState.AddModelError(
                string.Empty,
                "La fecha inicial no puede ser posterior a la fecha final.");

            ViewData["Desde"] = fechaDesde.ToString("yyyy-MM-dd");
            ViewData["Hasta"] = fechaHasta.ToString("yyyy-MM-dd");

            return View(Enumerable.Empty<Models.PrestamoReporte>());
        }

        var reporte = await _repositorio.ReporteAsync(
            fechaDesde,
            fechaHasta);

        ViewData["Desde"] = fechaDesde.ToString("yyyy-MM-dd");
        ViewData["Hasta"] = fechaHasta.ToString("yyyy-MM-dd");

        return View(reporte);
    }
}