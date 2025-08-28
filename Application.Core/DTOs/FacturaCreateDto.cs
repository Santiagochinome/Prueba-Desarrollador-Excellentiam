using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core.Entities;

namespace Application.Core.DTOs
{
    public class FacturaCreateDto
    {
        [Required, StringLength(100)]
        public string InvoiceNumber { get; set; } = string.Empty;

        [Required, StringLength(200)]
        public string Customer { get; set; } = string.Empty;

        public DateTime IssueDate { get; set; } = DateTime.Now;

        [Required]
        public InvoiceStatus State { get; set; } = InvoiceStatus.Pending;

        public decimal Total { get; set; }

        public List<DetalleFacturaCreateDto> Detail { get; set; } = new List<DetalleFacturaCreateDto>();
    }
}
