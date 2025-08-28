using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core.DTOs;
using Application.Core.Entities;

namespace Application.Services.Interfaces
{
    public interface IDetalleFacturaService
    {
        Task<DetalleFactura> AddDetalleToFacturaAsync(int IdBill, DetalleFactura Detail);
        Task RemoveDetalleFromFacturaAsync(int IdDetail);
        Task<DetalleFactura> GetDetalleByIdAsync(int Id);
        Task<IEnumerable<DetalleFactura>> GetDetallesByFacturaIdAsync(int IdBill);
        Task UpdateDetalleAsync(DetalleFactura Detail);
        Task<DetalleFactura> AddDetalleFromDtoAsync(int facturaId, DetalleFacturaCreateDto detalleDto);
    }
}
