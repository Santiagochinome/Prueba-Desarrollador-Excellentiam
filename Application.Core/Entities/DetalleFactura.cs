using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Entities
{
    public class DetalleFactura
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Product { get; set; } = string.Empty;

        [Range (1, 1000)]
        public int Amount { get; set; }

        [Range(0.01, 10000)]
        public decimal UnitPrice { get; set; }

        public decimal Subtotal
        {
            get => Amount * UnitPrice;

            private set { }
        }
        public int IdBill { get; set; }
        public Factura Bill { get; set; } = null!;
    }
}
