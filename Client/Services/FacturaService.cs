using System.Net.Http.Json;
using Client.Models;

namespace Client.Services
{
    public class FacturaService : IFacturaService
    {
        private readonly HttpClient _http;

        public FacturaService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Factura>> GetFacturas()
        {
            return await _http.GetFromJsonAsync<List<Factura>>("api/facturas") ?? new List<Factura>();
        }

        public async Task<Factura> GetFactura(int Id)
        {
            return await _http.GetFromJsonAsync<Factura>($"api/facturas/{Id}");
        }

        public async Task<Factura> CreateFactura(Factura bill)
        {
            var response = await _http.PostAsJsonAsync("api/facturas", bill);
            return await response.Content.ReadFromJsonAsync<Factura>();
        }

        public async Task UpdateFactura(Factura bill)
        {
            var existingFactura = await _http.GetFromJsonAsync<Factura>($"api/facturas/{bill.Id}");
            if (existingFactura == null)
                throw new Exception("Factura no encontrada");

            var response = await _http.PutAsJsonAsync($"api/facturas/{bill.Id}", bill);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al actualizar: {error}");
            }
        }

        public async Task DeleteFactura(int id)
        {
            var response = await _http.DeleteAsync($"api/facturas/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al eliminar factura: {errorMessage}");
            }
        }
    }
}
