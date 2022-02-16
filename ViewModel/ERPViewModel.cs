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
using System.Windows;
using System.Windows.Input;
using _4_06_EF_ERP.Logic;

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
                InvoiceLogic.AddInvoice(InvoiceToAdd);
                ReloadInvoices();
            }, c => true);

            RemoveCommand = new RelayCommand(e =>
            {
                MessageBoxResult messageBoxResult =
                    MessageBox.Show("Wollen Sie die Rechnung löschen?", "Löschen", MessageBoxButton.YesNo);
                switch (messageBoxResult)
                {
                    case MessageBoxResult.Yes:
                        InvoiceLogic.RemoveInvoice(InvoiceToDelete);
                        ReloadInvoices();
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }, c => true);
        }

        public void ReloadInvoices()
        {
            Trace.WriteLine("Reload Invoices");
            RaisePropertyChanged(nameof(Invoices));
            Trace.WriteLine("Fertig!");
            foreach (var invoice in Invoices)
            {
                Trace.WriteLine(invoice);
            }
        }

        public ICommand AddCommand { get; private set; }
        public ICommand RemoveCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
