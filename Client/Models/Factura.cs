using System.Text.Json.Serialization;

namespace Client.Models
{
    public class Factura
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public string Customer { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; } = DateTime.Now;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public InvoiceStatus State { get; set; } = InvoiceStatus.Pending;
        public decimal Total { get; set; }
        public List<DetalleFactura> Detail { get; set; } = new List<DetalleFactura>();
    }

    public class DetalleFactura
    {
        public int Id { get; set; }
        public string Product { get; set; } = string.Empty;
        public int Amount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal => Amount * UnitPrice;
        public int IdBill { get; set; }
    }

    public enum InvoiceStatus
    {
        Pending,
        Paid,
        Cancelled
    }
}
