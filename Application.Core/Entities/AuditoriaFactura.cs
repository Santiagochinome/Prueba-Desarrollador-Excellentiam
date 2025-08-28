using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Entities
{
    public class AuditoriaFactura
    {
        public int Id { get; set; }
        public int IdBill { get; set; }
        public string Action { get; set; } = string.Empty; 
        public DateTime Date { get; set; } = DateTime.Now;
        public string User { get; set; } = string.Empty;
        public string PreviousData { get; set; } = string.Empty;
        public string NewData { get; set; } = string.Empty;
    }
}
