using Application.Core.DTOs;
using Application.Core.Entities;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacturasController : ControllerBase
    {
        private readonly IFacturaService _facturaService;

        public FacturasController(IFacturaService facturaService)
        {
            _facturaService = facturaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Factura>>> GetFacturas()
        {
            try
            {
                var bill = await _facturaService.GetAllFacturasAsync();
                return Ok(bill);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<Factura>> GetFactura(int Id)
        {
            try
            {
                var bill = await _facturaService.GetFacturaByIdAsync(Id);
                return Ok(bill);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }

        }

        [HttpPost]
        public async Task<ActionResult<Factura>> CreateFactura([FromBody] FacturaCreateDto billDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var newInvoice = await _facturaService.CreateFacturaFromDtoAsync(billDto);
                return CreatedAtAction(nameof(GetFactura), new { id = newInvoice.Id }, newInvoice);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateFactura(int id, [FromBody] UpdateFacturaDto facturaDto)
        {
            try
            {
                if (id != facturaDto.Id)
                    return BadRequest("ID mismatch");

                await _facturaService.UpdateFacturaFromDtoAsync(id, facturaDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteFactura(int Id)
        {
            try
            {
                await _facturaService.DeleteFacturaAsync(Id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet("{Id}/total")]
        public async Task<ActionResult<decimal>> GetTotalFactura(int Id)
        {
            try
            {
                var total = await _facturaService.CalculateTotalAsync(Id);
                return Ok(total);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}
