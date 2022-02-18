using MyERP.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyERP.Context
{
    class InvoiceInitializer: DropCreateDatabaseAlways<InvoiceContext>
    {
        protected override void Seed(InvoiceContext context)
        {
            seedInvoices(context);
            seedInvoicePositions(context);
            base.Seed(context);
        }

        protected void seedInvoices(InvoiceContext context)
        {
            IList<Invoice> seederInvoices = new List<Invoice>();
            seederInvoices.Add(new Invoice() { CustomerName = "Andi", CustomerAddress = "Arbeit", Amount = 100, Vat = 1, InvoiceDate = DateTime.Now });
            seederInvoices.Add(new Invoice() { CustomerName = "Sue", CustomerAddress = "Permarkt", Amount = 200, Vat = 2, InvoiceDate = DateTime.Now });
            seederInvoices.Add(new Invoice() { CustomerName = "Harry", CustomerAddress = "Gersack", Amount = 300, Vat = 3, InvoiceDate = DateTime.Now });

            context.Invoices.AddRange(seederInvoices);
        }

        protected void seedInvoicePositions(InvoiceContext context)
        {
            IList<InvoicePosition> seederInvoicePositions = new List<InvoicePosition>();
            seederInvoicePositions.Add(new InvoicePosition
            {
                InvoiceId = 1,
                ItemNr = 1,
                Qty = 100,
                Price = 1000
            });
            seederInvoicePositions.Add(new InvoicePosition
            {
                InvoiceId = 1,
                ItemNr = 2,
                Qty = 200,
                Price = 2000
            });
            seederInvoicePositions.Add(new InvoicePosition
            {
                InvoiceId = 2,
                ItemNr = 3,
                Qty = 300,
                Price = 3000
            });
            seederInvoicePositions.Add(new InvoicePosition
            {
                InvoiceId = 3,
                ItemNr = 4,
                Qty = 400,
                Price = 4000
            });
            seederInvoicePositions.Add(new InvoicePosition
            {
                InvoiceId = 3,
                ItemNr = 5,
                Qty = 500,
                Price = 5000
            });

            context.InvoicePositions.AddRange(seederInvoicePositions);
        }
    }
}
