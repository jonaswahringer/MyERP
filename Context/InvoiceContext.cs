using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using _4_06_EF_ERP.Model;

namespace _4_06_EF_ERP.Context
{
    class InvoiceContext: DbContext
    {
        public DbSet<Invoice> Invoices { get; set; }
    }
}
