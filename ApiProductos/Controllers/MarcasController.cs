using ApiProductos.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiProductos.Controllers;

[ApiController]
[Route("api/marcas")]
public class MarcasController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct) =>
        Ok(await db.Marcas.AsNoTracking().OrderBy(m => m.Nombre)
            .Select(m => new { idMarca = m.IdMarca, marca = m.Nombre }).ToListAsync(ct));
}
