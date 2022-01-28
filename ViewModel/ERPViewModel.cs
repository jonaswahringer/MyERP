using _4_06_EF_ERP.Context;
using _4_06_EF_ERP.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _4_06_EF_ERP.ViewModel    
{
    class ERPViewModel : INotifyPropertyChanged
    {
        public IList<Invoice> Invoices
        {
            get
            {
                using (InvoiceContext ctx = new InvoiceContext())
                {
                    return (from data in ctx.Invoices select data).ToList();
                }
            }
        }
        public Invoice InvoiceToAdd { get; set; } = new Invoice();
        public Invoice InvoiceToDelete { get; set; } = new Invoice();

        public ERPViewModel()
        {

            AddCommand = new RelayCommand(e =>
            {
                AddInvoice(InvoiceToAdd);
                RaisePropertyChanged(nameof(Invoices));
            }, c => true);

            RemoveCommand = new RelayCommand(e =>
            {
                RemoveInvoice(InvoiceToDelete);
                RaisePropertyChanged(nameof(Invoices));
            }, c => true);
        }

        public ICommand AddCommand { get; private set; }
        public ICommand RemoveCommand { get; private set; }

        private void AddInvoice(Invoice invoice)
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

        private void RemoveInvoice(Invoice invoice)
        {
            try
            {
                using (var ctx = new InvoiceContext())
                {
                    var found = ctx.Invoices.Find(invoice.Id);

                    if(found != null)
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
