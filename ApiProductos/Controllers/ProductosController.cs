using ApiProductos.Data;
using ApiProductos.DTOs;
using ApiProductos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiProductos.Controllers;

[ApiController]
[Route("api/productos")]
public class ProductosController(AppDbContext db) : ControllerBase
{
    private IQueryable<ProductoDto> Consulta() => db.Productos.AsNoTracking()
        .Select(p => new ProductoDto
        {
            IdProducto = p.IdProducto, Producto = p.Nombre, IdMarca = p.IdMarca,
            Marca = p.Marca!.Nombre, Descripcion = p.Descripcion,
            PrecioCosto = p.PrecioCosto, PrecioVenta = p.PrecioVenta, Existencia = p.Existencia
        });

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct) =>
        Ok(await Consulta().OrderBy(p => p.IdProducto).ToListAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var producto = await Consulta().SingleOrDefaultAsync(p => p.IdProducto == id, ct);
        return producto is null ? NotFound() : Ok(producto);
    }

    [HttpPost]
    public async Task<IActionResult> Post(GuardarProductoDto dto, CancellationToken ct)
    {
        if (!await db.Marcas.AnyAsync(m => m.IdMarca == dto.IdMarca, ct))
            return MarcaInvalida();
        var producto = new Producto();
        Asignar(producto, dto);
        db.Productos.Add(producto);
        await db.SaveChangesAsync(ct);
        var resultado = await Consulta().SingleAsync(p => p.IdProducto == producto.IdProducto, ct);
        return CreatedAtAction(nameof(GetById), new { id = producto.IdProducto }, resultado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, GuardarProductoDto dto, CancellationToken ct)
    {
        var producto = await db.Productos.FindAsync([id], ct);
        if (producto is null) return NotFound();
        if (!await db.Marcas.AnyAsync(m => m.IdMarca == dto.IdMarca, ct))
            return MarcaInvalida();
        Asignar(producto, dto);
        try { await db.SaveChangesAsync(ct); }
        catch (DbUpdateConcurrencyException) { return NotFound(); }
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var producto = await db.Productos.FindAsync([id], ct);
        if (producto is null) return NotFound();
        db.Productos.Remove(producto);
        try { await db.SaveChangesAsync(ct); }
        catch (DbUpdateConcurrencyException) { return NotFound(); }
        return NoContent();
    }

    private IActionResult MarcaInvalida()
    {
        ModelState.AddModelError(nameof(GuardarProductoDto.IdMarca), "La marca seleccionada no existe.");
        return ValidationProblem(ModelState);
    }

    private static void Asignar(Producto p, GuardarProductoDto dto)
    {
        p.Nombre = dto.Producto;
        p.IdMarca = dto.IdMarca!.Value;
        p.Descripcion = dto.Descripcion;
        p.PrecioCosto = dto.PrecioCosto!.Value;
        p.PrecioVenta = dto.PrecioVenta!.Value;
        p.Existencia = dto.Existencia!.Value;
    }
}
