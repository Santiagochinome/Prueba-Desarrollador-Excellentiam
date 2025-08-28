using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Infrastructure.Data.Data;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;

namespace Application.Services.Services
{
    public class ExcelExportService : IExcelExportService
    {
        private readonly AppDbContext _context;

        public ExcelExportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<byte[]> ExportFacturasToExcelAsync()
        {
            var facturas = await _context.Facturas.ToListAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Facturas");

            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Número Factura";
            worksheet.Cell(1, 3).Value = "Cliente";
            worksheet.Cell(1, 4).Value = "Fecha Emisión";
            worksheet.Cell(1, 5).Value = "Estado";
            worksheet.Cell(1, 6).Value = "Total";

            for (int i = 0; i < facturas.Count; i++)
            {
                var factura = facturas[i];
                worksheet.Cell(i + 2, 1).Value = factura.Id;
                worksheet.Cell(i + 2, 2).Value = factura.InvoiceNumber;
                worksheet.Cell(i + 2, 3).Value = factura.Customer;
                worksheet.Cell(i + 2, 4).Value = factura.IssueDate;
                worksheet.Cell(i + 2, 5).Value = factura.State.ToString();
                worksheet.Cell(i + 2, 6).Value = factura.Total;
            }

            worksheet.Columns().AdjustToContents();
            worksheet.Range(1, 1, 1, 6).Style.Font.Bold = true;

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public async Task<byte[]> ExportDetallesToExcelAsync(int facturaId)
        {
            var detalles = await _context.DetallesFactura
                .Where(d => d.IdBill == facturaId)
                .Include(d => d.Bill)
                .ToListAsync();

            var factura = await _context.Facturas.FindAsync(facturaId);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add($"Detalles Factura {facturaId}");

            worksheet.Cell(1, 1).Value = "Factura:";
            worksheet.Cell(1, 2).Value = factura?.InvoiceNumber;
            worksheet.Cell(2, 1).Value = "Cliente:";
            worksheet.Cell(2, 2).Value = factura?.Customer;
            worksheet.Cell(3, 1).Value = "Fecha:";
            worksheet.Cell(3, 2).Value = factura?.IssueDate;
            worksheet.Cell(4, 1).Value = "Total:";
            worksheet.Cell(4, 2).Value = factura?.Total;

            worksheet.Cell(6, 1).Value = "ID";
            worksheet.Cell(6, 2).Value = "Producto";
            worksheet.Cell(6, 3).Value = "Cantidad";
            worksheet.Cell(6, 4).Value = "Precio Unitario";
            worksheet.Cell(6, 5).Value = "Subtotal";

            for (int i = 0; i < detalles.Count; i++)
            {
                var detalle = detalles[i];
                worksheet.Cell(i + 7, 1).Value = detalle.Id;
                worksheet.Cell(i + 7, 2).Value = detalle.Product;
                worksheet.Cell(i + 7, 3).Value = detalle.Amount;
                worksheet.Cell(i + 7, 4).Value = detalle.UnitPrice;
                worksheet.Cell(i + 7, 5).Value = detalle.Subtotal;
            }

            worksheet.Columns().AdjustToContents();
            worksheet.Range(1, 1, 6, 5).Style.Font.Bold = true;

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
