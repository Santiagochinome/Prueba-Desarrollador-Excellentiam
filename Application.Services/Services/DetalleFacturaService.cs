using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.Data;
using Application.Core.Entities;
using Application.Core.DTOs;

namespace Application.Services.Services
{
    public class DetalleFacturaService : IDetalleFacturaService
    {
        private readonly AppDbContext _context;
        public DetalleFacturaService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<DetalleFactura> AddDetalleToFacturaAsync(int IdBill, DetalleFactura Detail)
        {
            var bill = await _context.Facturas.FindAsync(IdBill)
                ?? throw new KeyNotFoundException("La Factura no fue encontrada");

            Detail.IdBill = IdBill;
            Detail.Bill = null;
            _context.DetallesFactura.Add(Detail);
            await _context.SaveChangesAsync();
            await RecalculateFacturaTotalAsync(IdBill);

            return Detail;
        }
        public async Task RemoveDetalleFromFacturaAsync(int IdDetail)
        {
            var detail = await GetDetalleByIdAsync(IdDetail);
            var Idbill = detail.IdBill;
            _context.DetallesFactura.Remove(detail);
            await _context.SaveChangesAsync();
            await RecalculateFacturaTotalAsync(Idbill);
        }
        public async Task<DetalleFactura> GetDetalleByIdAsync(int Id)
        {
            return await _context.DetallesFactura
                .Include(d => d.Bill)
                .FirstOrDefaultAsync(d => d.Id == Id)
                ?? throw new KeyNotFoundException("Detalle no encontrado");
        }
        public async Task<IEnumerable<DetalleFactura>> GetDetallesByFacturaIdAsync(int IdBill)
        {
            return await _context.DetallesFactura
                .Where(d => d.IdBill == IdBill)
                .ToListAsync();
        }

        public async Task UpdateDetalleAsync(DetalleFactura Detail)
        {
            var existing = await GetDetalleByIdAsync(Detail.Id);

            _context.Entry(existing).CurrentValues.SetValues(Detail);
            await _context.SaveChangesAsync();

            // Recalcular el total de la factura
            await RecalculateFacturaTotalAsync(existing.IdBill);
        }

        private async Task RecalculateFacturaTotalAsync(int IdBill)
        {
            var total = await _context.DetallesFactura
                .Where(d => d.IdBill == IdBill)
                .SumAsync(d => d.Subtotal);

            var bill = await _context.Facturas.FindAsync(IdBill);
            bill.Total = total;
            await _context.SaveChangesAsync();
        }

        public async Task<DetalleFactura> AddDetalleFromDtoAsync(int Idbill, DetalleFacturaCreateDto detailDto)
        {
            var factura = await _context.Facturas.FindAsync(Idbill)
                ?? throw new KeyNotFoundException("Factura no encontrada");

            var detalle = new DetalleFactura
            {
                Product = detailDto.Product,
                Amount = detailDto.Amount,
                UnitPrice = detailDto.UnitPrice,
                IdBill = Idbill
            };

            _context.DetallesFactura.Add(detalle);
            await _context.SaveChangesAsync();

            await RecalculateFacturaTotalAsync(Idbill);

            return detalle;
        }
    }
}
