using Biblioteca.Web.Models;
using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biblioteca.Web.Controllers;

public class LibrosController : Controller
{
    private readonly ILibroRepositorio _repositorio;

    public LibrosController(ILibroRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IActionResult> Index(string? buscar)
    {
        ViewData["Title"] = "Libros";

        IEnumerable<Libro> libros;

        if (string.IsNullOrWhiteSpace(buscar))
        {
            libros = await _repositorio.ListarAsync();
        }
        else
        {
            libros = await _repositorio.BuscarAsync(buscar);
        }

        ViewData["Buscar"] = buscar;

        return View(libros);
    }

    public async Task<IActionResult> Details(int id)
    {
        ViewData["Title"] = "Detalle del libro";

        var libro = await _repositorio.ObtenerPorIdAsync(id);

        if (libro == null)
        {
            return NotFound();
        }

        return View(libro);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewData["Title"] = "Nuevo libro";

        await CargarAutoresAsync();

        return View(new Libro());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Libro libro)
    {
        ViewData["Title"] = "Nuevo libro";

        if (!ModelState.IsValid)
        {
            await CargarAutoresAsync(libro.AutorId);
            return View(libro);
        }

        try
        {
            await _repositorio.InsertarAsync(libro);

            TempData["Mensaje"] = "Libro registrado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ObtenerMensajeError(ex));

            await CargarAutoresAsync(libro.AutorId);

            return View(libro);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Editar libro";

        var libro = await _repositorio.ObtenerPorIdAsync(id);

        if (libro == null)
        {
            return NotFound();
        }

        await CargarAutoresAsync(libro.AutorId);

        return View(libro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Libro libro)
    {
        ViewData["Title"] = "Editar libro";

        if (!ModelState.IsValid)
        {
            await CargarAutoresAsync(libro.AutorId);
            return View(libro);
        }

        try
        {
            await _repositorio.ActualizarAsync(libro);

            TempData["Mensaje"] = "Libro actualizado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ObtenerMensajeError(ex));

            await CargarAutoresAsync(libro.AutorId);

            return View(libro);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        ViewData["Title"] = "Eliminar libro";

        var libro = await _repositorio.ObtenerPorIdAsync(id);

        if (libro == null)
        {
            return NotFound();
        }

        return View(libro);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _repositorio.EliminarAsync(id);

            TempData["Mensaje"] = "Libro eliminado correctamente.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ObtenerMensajeError(ex);
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task CargarAutoresAsync(int? seleccionado = null)
    {
        var autores = await _repositorio.ListarAutoresAsync();

        ViewBag.Autores = new SelectList(
            autores,
            "AutorId",
            "Nombre",
            seleccionado);
    }

    private static string ObtenerMensajeError(Exception ex)
    {
        return ex.Message.Contains("ISBN")
            ? ex.Message
            : "No se pudo completar la operación.";
    }
}