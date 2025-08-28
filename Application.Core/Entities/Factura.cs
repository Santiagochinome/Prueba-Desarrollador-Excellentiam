using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Entities
{
    public class Factura
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string InvoiceNumber { get; set; } = string.Empty;

        [Required, StringLength(200)]
        public string Customer { get; set; } = string.Empty;

        public DateTime IssueDate { get; set; } = DateTime.Now;

        [Required]
        public InvoiceStatus State { get; set; } = InvoiceStatus.Pending;

        public decimal Total { get; set; }

        public ICollection<DetalleFactura> Detail { get; set; } = new List<DetalleFactura>();
    }


    public enum InvoiceStatus
    {
        Pending,
        Paid,
        Cancelled
    }
}
