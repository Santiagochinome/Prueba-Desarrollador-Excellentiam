using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.DTOs
{
    public class UpdateFacturaDto
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string InvoiceNumber { get; set; } = string.Empty;

        [Required, StringLength(200)]
        public string Customer { get; set; } = string.Empty;

        public DateTime IssueDate { get; set; }

        [Required]
        public string State { get; set; } = "Pending";

        public decimal Total { get; set; }

        public List<UpdateDetalleDto> Detail { get; set; } = new List<UpdateDetalleDto>();
    }

    public class UpdateDetalleDto
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Product { get; set; } = string.Empty;

        [Range(1, 1000)]
        public int Amount { get; set; }

        [Range(0.01, 10000)]
        public decimal UnitPrice { get; set; }

        public int IdBill { get; set; }
    }
}
