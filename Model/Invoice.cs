using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyERP.Model
{
    class Invoice
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public double Amount { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int Vat { get; set; }

        public ICollection<InvoicePosition> Positions { get; set; } = new List<InvoicePosition>(); //Referenz auf die Rechnungsposition
    }
}
