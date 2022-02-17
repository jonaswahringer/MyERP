using _4_06_EF_ERP.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4_06_EF_ERP.Context
{
    class InvoiceInitializer : DropCreateDatabaseAlways<InvoiceContext>
    {
        protected override void Seed(InvoiceContext context)
        {
            IList<Invoice> defaults = new List<Invoice>();
            IList<Position> defaultPositions = new List<Position>();
            defaultPositions.Add(new Position
            {
                ItemNr = 0,
                Price = 100,
                Qty = 10
            });

            defaults.Add(new Invoice
            {
                CustomerName = "HTL",
                Amount = 100,
                CustomerAddress = "Ybbs",
                InvoiceDate = new DateTime(2020, 01, 25),
                Vat = 20,
                Positions = defaultPositions
            });

            defaults.Add(new Invoice
            {
                CustomerName = "HAK",
                Amount = 200,
                CustomerAddress = "Ybbs",
                InvoiceDate = new DateTime(2020, 01, 15),
                Vat = 20,
                Positions = defaultPositions
            });

            context.Invoices.AddRange(defaults);

            base.Seed(context);
        }
    }
}
