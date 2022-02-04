using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MyERP.Model;

namespace MyERP.Context
{
    class InvoiceContext: DbContext
    {

        public InvoiceContext()
        {
            Database.SetInitializer(new InvoiceInitializer());
        }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoicePosition> InvoicePositions { get; set; }

    }
}
