using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExportController : ControllerBase
    {
        private readonly IExcelExportService _excelExportService;

        public ExportController(IExcelExportService excelExportService)
        {
            _excelExportService = excelExportService;
        }

        [HttpGet("facturas/excel")]
        public async Task<IActionResult> ExportFacturasToExcel()
        {
            try
            {
                var excelData = await _excelExportService.ExportFacturasToExcelAsync();
                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Facturas.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al exportar: {ex.Message}");
            }
        }

        [HttpGet("facturas/{facturaId}/detalles/excel")]
        public async Task<IActionResult> ExportDetallesToExcel(int facturaId)
        {
            try
            {
                var excelData = await _excelExportService.ExportDetallesToExcelAsync(facturaId);
                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Detalles_Factura_{facturaId}.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al exportar: {ex.Message}");
            }
        }
    }
}
