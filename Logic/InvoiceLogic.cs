using _4_06_EF_ERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _4_06_EF_ERP.Context;

namespace _4_06_EF_ERP.Logic
{
    class InvoiceLogic
    {
        public static void AddInvoice(Invoice invoice)
        {
            invoice.InvoiceDate = DateTime.Now;
            try
            {
                using (var ctx = new InvoiceContext())
                {
                    ctx.Invoices.Add(invoice);
                    ctx.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void RemoveInvoice(Invoice invoice)
        {
            try
            {
                using (var ctx = new InvoiceContext())
                {
                    var found = ctx.Invoices.Find(invoice.Id);

                    if (found != null)
                    {
                        ctx.Invoices.Remove(found);
                        ctx.SaveChanges();
                    }

                    ctx.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
