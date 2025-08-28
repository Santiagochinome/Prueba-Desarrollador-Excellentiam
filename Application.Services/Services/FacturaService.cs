using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Application.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.Data;
using Application.Core.DTOs;

namespace Application.Services.Services
{
    public class FacturaService : IFacturaService
    {
        private readonly AppDbContext _context;

        public FacturaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Factura> CreateFacturaAsync(Factura bill)
        {
            if(await _context.Facturas.AnyAsync(f => f.InvoiceNumber == bill.InvoiceNumber))
            {
                throw new InvalidOperationException("El numero de factura ya se encuentra asignado");
            }

            _context.Facturas.Add(bill);
            await _context.SaveChangesAsync();
            return bill;
        }

        public async Task<Factura> GetFacturaByIdAsync(int Id)
        {
            return await _context.Facturas
                .Include(f => f.Detail)
                .FirstOrDefaultAsync(f => f.Id == Id)
                ?? throw new KeyNotFoundException("La Factura no fue encontrada");
        }

        public async Task<IEnumerable<Factura>> GetAllFacturasAsync()
        {
            return await _context.Facturas
                .Include(f => f.Detail)
                .OrderByDescending(f => f.IssueDate)
                .ToListAsync();
        }

        public async Task UpdateFacturaAsync(Factura bill)
        {
            var existing = await GetFacturaByIdAsync(bill.Id);

            _context.Entry(existing).CurrentValues.SetValues(bill);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFacturaAsync(int Id)
        {
            var bill = await GetFacturaByIdAsync(Id);
            _context.Facturas.Remove(bill);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> CalculateTotalAsync(int Idbill)
        {
            var bill = await GetFacturaByIdAsync(Idbill);
            return bill.Detail.Sum(d => d.Subtotal);
        }

        public async Task<Factura> CreateFacturaFromDtoAsync(FacturaCreateDto billDto)
        {
            if (await _context.Facturas.AnyAsync(f => f.InvoiceNumber == billDto.InvoiceNumber))
                throw new InvalidOperationException("El número de factura ya existe");

            var bill = new Factura
            {
                InvoiceNumber = billDto.InvoiceNumber,
                Customer = billDto.Customer,
                IssueDate = billDto.IssueDate,
                State = billDto.State,
                Total = billDto.Total
            };

            foreach (var detailDto in billDto.Detail)
            {
                bill.Detail.Add(new DetalleFactura
                {
                    Product = detailDto.Product,
                    Amount = detailDto.Amount,
                    UnitPrice = detailDto.UnitPrice,
                    IdBill = bill.Id 
                });
            }

            _context.Facturas.Add(bill);
            await _context.SaveChangesAsync();

            bill.Total = bill.Detail.Sum(d => d.Subtotal);
            await _context.SaveChangesAsync();

            return bill;
        }

        public async Task UpdateFacturaFromDtoAsync(int id, UpdateFacturaDto facturaDto)
        {
            var existingFactura = await _context.Facturas
                .Include(f => f.Detail)
                .FirstOrDefaultAsync(f => f.Id == id)
                ?? throw new KeyNotFoundException("Factura no encontrada");

            // Actualizar propiedades principales
            existingFactura.InvoiceNumber = facturaDto.InvoiceNumber;
            existingFactura.Customer = facturaDto.Customer;
            existingFactura.IssueDate = facturaDto.IssueDate;
            existingFactura.State = Enum.Parse<InvoiceStatus>(facturaDto.State);
            existingFactura.Total = facturaDto.Total;

            // Actualizar detalles
            existingFactura.Detail.Clear();
            foreach (var detalleDto in facturaDto.Detail)
            {
                existingFactura.Detail.Add(new DetalleFactura
                {
                    Id = detalleDto.Id,
                    Product = detalleDto.Product,
                    Amount = detalleDto.Amount,
                    UnitPrice = detalleDto.UnitPrice,
                    IdBill = id
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
