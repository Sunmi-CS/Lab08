using Biblioteca.Web.Models;
using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Web.Controllers;

public class SociosController : Controller
{
    private readonly ISocioRepositorio _repositorio;

    public SociosController(ISocioRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Socios";

        var socios = await _repositorio.ListarAsync();

        return View(socios);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewData["Title"] = "Nuevo socio";

        return View(new Socio());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Socio socio)
    {
        ViewData["Title"] = "Nuevo socio";

        if (!ModelState.IsValid)
        {
            return View(socio);
        }

        try
        {
            await _repositorio.InsertarAsync(socio);

            TempData["Mensaje"] = "Socio registrado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("DNI"))
            {
                ModelState.AddModelError(
                    nameof(Socio.DNI),
                    "El DNI ya está registrado.");
            }
            else
            {
                ModelState.AddModelError(
                    string.Empty,
                    "No se pudo registrar el socio.");
            }

            return View(socio);
        }
    }
}