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
                Qty = 1
            });

            defaultPositions.Add(new Position
            {
                ItemNr = 1,
                Price = 200,
                Qty = 10
            });

            defaultPositions.Add(new Position
            {
                ItemNr = 2,
                Price = 300,
                Qty = 100
            });

            defaults.Add(new Invoice
            {
                CustomerName = "Andi Arbeit",
                Amount = 587,
                CustomerAddress = "Andi Arbeit Straße 1",
                InvoiceDate = new DateTime(2020, 01, 01),
                Vat = 20,
                //Positions = defaultPositions
            });
            defaults[0].Positions.Add(defaultPositions[0]);
            defaults[0].Positions.Add(defaultPositions[1]);
            defaults[0].Positions.Add(defaultPositions[2]);

            defaults.Add(new Invoice
            {
                CustomerName = "Mike Rhosoft",
                Amount = 200,
                CustomerAddress = "Mike Rhosoft Straße 2",
                InvoiceDate = new DateTime(2020, 01, 02),
                Vat = 20,
                //Positions = defaultPositions
            });
            defaults[1].Positions.Add(defaultPositions[1]);

            defaults.Add(new Invoice
            {
                CustomerName = "Sue Permarkt",
                Amount = 300,
                CustomerAddress = "Sue Permarkt Straße 3",
                InvoiceDate = new DateTime(2020, 01, 03),
                Vat = 20,
                //Positions = defaultPositions
            });
            defaults[2].Positions.Add(defaultPositions[2]);
            defaults[2].Positions.Add(defaultPositions[0]);

            defaults.Add(new Invoice
            {
                CustomerName = "Externer Außerirdischer",
                Amount = 400,
                CustomerAddress = "Außerirdische Straße 4",
                InvoiceDate = new DateTime(2020, 01, 04),
                Vat = 20
            });

            defaults.Add(new Invoice
            {
                CustomerName = "Jürgen Kühnl",
                Amount = 500,
                CustomerAddress = "Jürgen Kühnl Straße 5",
                InvoiceDate = new DateTime(2020, 01, 05),
                Vat = 20
            });

            context.Invoices.AddRange(defaults);
            context.Positions.AddRange(defaultPositions);

            base.Seed(context);
        }
    }
}
