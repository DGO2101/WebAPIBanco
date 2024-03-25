using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIBanco;
using WebAPIBanco.Modelos;

namespace WebAPIBanco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarjetasController : ControllerBase
    {
        private readonly WebAppBancoContextContext _context;

        public TarjetasController(WebAppBancoContextContext context)
        {
            _context = context;
        }

        // GET: api/Tarjetas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tarjeta>>> GetTarjetas()
        {
            return await _context.Tarjetas.ToListAsync();
        }

        // GET: api/Tarjetas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tarjeta>> GetTarjeta(int id)
        {
            var tarjeta = await _context.Tarjetas.FindAsync(id);

            if (tarjeta == null)
            {
                return NotFound();
            }

            return tarjeta;
        }

        // PUT: api/Tarjetas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTarjeta(int id, Tarjeta tarjeta)
        {
            if (id != tarjeta.Id)
            {
                return BadRequest();
            }

            _context.Entry(tarjeta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TarjetaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tarjetas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tarjeta>> PostTarjeta(Tarjeta tarjeta)
        {
            _context.Tarjetas.Add(tarjeta);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTarjeta", new { id = tarjeta.Id }, tarjeta);
        }

        // DELETE: api/Tarjetas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarjeta(int id)
        {
            var tarjeta = await _context.Tarjetas.FindAsync(id);
            if (tarjeta == null)
            {
                return NotFound();
            }

            _context.Tarjetas.Remove(tarjeta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("tarjeta/{id}/pagos")]
        public IActionResult GetPagosByTarjetaId(int id)
        {
            var pagos = _context.Pagos.Where(p => p.TarjetaId == id).ToList();
            return Ok(pagos);
        }

        [HttpGet("tarjeta/{id}/compras")]
        public IActionResult GetComprasByTarjetaId(int id)
        {
            var compras = _context.Compras.Where(c => c.TarjetaId == id).ToList();
            return Ok(compras);
        }
        [HttpGet("tarjeta/{id}/pagos/mes-actual")]
        public IActionResult GetPagosMesActualByTarjetaId(int id)
        {
            var fechaInicioMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var fechaFinMes = fechaInicioMes.AddMonths(1).AddDays(-1);

            var pagos = _context.Pagos.Where(p => p.TarjetaId == id && p.Fecha >= fechaInicioMes && p.Fecha <= fechaFinMes).ToList();
            return Ok(pagos);
        }

        [HttpGet("tarjeta/{id}/compras/mes-actual")]
        public IActionResult GetComprasMesActualByTarjetaId(int id)
        {
            var fechaInicioMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var fechaFinMes = fechaInicioMes.AddMonths(1).AddDays(-1);

            var compras = _context.Compras.Where(c => c.TarjetaId == id && c.Fecha >= fechaInicioMes && c.Fecha <= fechaFinMes).ToList();
            return Ok(compras);
        }



        private bool TarjetaExists(int id)
        {
            return _context.Tarjetas.Any(e => e.Id == id);
        }


    }
}
