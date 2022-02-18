using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyERP.Model
{
    class InvoicePosition //n-Seite
    {
        public int Id { get; set; } // Surrogate Key / Stellvertreter Key

        public int ItemNr { get; set; }
        public int Qty { get; set; }
        public double Price { get; set; }

        public int InvoiceId { get; set; }             // FK der Rechung
        public InvoicePosition Invoice { get; set; }     // Referenz auf die Rechnung

    }
}
