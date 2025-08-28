using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core.DTOs;
using Application.Core.Entities;

namespace Application.Services.Interfaces
{
    public interface IFacturaService
    {
        Task<Factura> CreateFacturaAsync(Factura Factura);
        Task<Factura> GetFacturaByIdAsync(int Id);
        Task<IEnumerable<Factura>> GetAllFacturasAsync();
        Task UpdateFacturaAsync(Factura Factura);
        Task DeleteFacturaAsync(int Id);
        Task<decimal> CalculateTotalAsync(int IdFactura);
        Task<Factura> CreateFacturaFromDtoAsync(FacturaCreateDto facturaDto);
        Task UpdateFacturaFromDtoAsync(int id, UpdateFacturaDto facturaDto);
    }
}
