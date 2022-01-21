using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyERP.Model
{
    class Invoice
    {
        int Id { get; set; }
        string CustomerName { get; set; }
        string CustomerAddress { get; set; }
        double Amount { get; set; }
        DateTime InvoiceDate { get; set; }
        int Vat { get; set; }
    }
}
