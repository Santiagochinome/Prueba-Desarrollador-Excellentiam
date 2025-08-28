using Client.Models;

namespace Client.Services
{
    public interface IFacturaService
    {
        Task<List<Factura>> GetFacturas();
        Task<Factura> GetFactura(int id);
        Task<Factura> CreateFactura(Factura factura);
        Task UpdateFactura(Factura factura);
        Task DeleteFactura(int id);
    }
}
