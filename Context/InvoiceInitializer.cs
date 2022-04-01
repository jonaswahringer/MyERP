using _4_06_EF_ERP.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4_06_EF_ERP.Context
{
    class InvoiceInitializer : DropCreateDatabaseIfModelChanges<InvoiceContext>
    {
        protected override void Seed(InvoiceContext context)
        {
            IList<Invoice> defaults = new List<Invoice>();
            IList<Position> defaultPositions = new List<Position>();
            defaultPositions.Add(new Position
            {
                ItemNr = 0,
                Price = 200,
                Qty = 20
            });

            defaultPositions.Add(new Position
            {
                ItemNr = 1,
                Price = 100,
                Qty = 10
            });

            defaultPositions.Add(new Position
            {
                ItemNr = 2,
                Price = 70,
                Qty = 15
            });

            defaults.Add(new Invoice
            {
                CustomerName = "HTL",
                Amount = 587,
                CustomerAddress = "Ybbs",
                InvoiceDate = new DateTime(2020, 01, 25),
                Vat = 20,
                //Positions = defaultPositions
            });
            defaults[0].Positions.Add(defaultPositions[0]);
            defaults[0].Positions.Add(defaultPositions[1]);
            defaults[0].Positions.Add(defaultPositions[2]);

            defaults.Add(new Invoice
            {
                CustomerName = "HAK",
                Amount = 200,
                CustomerAddress = "Ybbs",
                InvoiceDate = new DateTime(2020, 01, 15),
                Vat = 20,
                //Positions = defaultPositions
            });
            defaults[1].Positions.Add(defaultPositions[1]);

            defaults.Add(new Invoice
            {
                CustomerName = "HAS",
                Amount = 300,
                CustomerAddress = "Ybbs",
                InvoiceDate = new DateTime(2020, 01, 10),
                Vat = 20,
                //Positions = defaultPositions
            });
            defaults[2].Positions.Add(defaultPositions[2]);
            defaults[2].Positions.Add(defaultPositions[0]);

            defaults.Add(new Invoice
            {
                CustomerName = "Mittelschule",
                Amount = 400,
                CustomerAddress = "Ybbs",
                InvoiceDate = new DateTime(2020, 01, 05),
                Vat = 20
            });

            defaults.Add(new Invoice
            {
                CustomerName = "Volksschule",
                Amount = 500,
                CustomerAddress = "Ybbs",
                InvoiceDate = new DateTime(2020, 01, 01),
                Vat = 20
            });

            context.Invoices.AddRange(defaults);
            context.Positions.AddRange(defaultPositions);

            base.Seed(context);
        }
    }
}
