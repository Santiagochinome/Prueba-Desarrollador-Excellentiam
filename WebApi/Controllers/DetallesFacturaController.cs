using Application.Core.DTOs;
using Application.Core.Entities;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/facturas/{Idbill}/[controller]")]
    public class DetallesFacturaController : ControllerBase
    {
        private readonly IDetalleFacturaService _detailService;

        public DetallesFacturaController(IDetalleFacturaService detailService)
        {
            _detailService = detailService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetalleFactura>>> GetDetallesByFactura(int Idbill)
        {
            try
            {
                var detail = await _detailService.GetDetallesByFacturaIdAsync(Idbill);
                return Ok(detail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<DetalleFactura>> GetDetalle(int Id)
        {
            try
            {
                var detail = await _detailService.GetDetalleByIdAsync(Id);
                return Ok(detail);
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
        public async Task<ActionResult<DetalleFactura>> AddDetalleToFactura(int Idbill, [FromBody] DetalleFacturaCreateDto detailDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var newdetail = await _detailService.AddDetalleFromDtoAsync(Idbill, detailDto);
                return CreatedAtAction(nameof(GetDetalle), new { Idbill, id = newdetail.Id }, newdetail);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateDetalle(int Id, DetalleFactura detail)
        {
            try
            {
                if (Id != detail.Id)
                    return BadRequest("ID equivocado");

                await _detailService.UpdateDetalleAsync(detail);
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
        public async Task<IActionResult> RemoveDetalle(int Id)
        {
            try
            {
                await _detailService.RemoveDetalleFromFacturaAsync(Id);
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
    }
}
