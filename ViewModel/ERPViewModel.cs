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
using System.Data.Entity;
using System.Windows.Input;
using _4_06_EF_ERP.Logic;
using System.Windows.Documents;
using System.IO;
using System.Windows.Markup;
using System.Xml;
using _4_06_EF_ERP.Printing;
using System.Windows.Controls;

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
                    return (from data in ctx.Invoices.Include(p => p.Positions) select data).ToList();
                }
            }
        }
        public Invoice invoiceToAdd = new Invoice();
        public Invoice InvoiceToAdd
        {
            get => invoiceToAdd;
            set
            {
                invoiceToAdd = value;
                RaisePropertyChanged();
            }
        }
        public Invoice selectedInvoice = new Invoice();
        public Invoice SelectedInvoice
        {
            get => selectedInvoice;
            set
            {
                selectedInvoice = value;
                RaisePropertyChanged();
            }
        }

        public List<Position> SelectedPositions { get; set; } = new List<Position>();
        public int selectedPositionsSum = 0;
        public int SelectedPositionsSum
        {
            get => selectedPositionsSum;
            set
            {
                selectedPositionsSum = value;
                RaisePropertyChanged();
            }
        }

        public ERPViewModel()
        {

            AddCommand = new RelayCommand(e =>
            {
                InvoiceToAdd.InvoiceDate = DateTime.Now;
                InvoiceLogic.AddInvoice(InvoiceToAdd);
                foreach (var invoice in Invoices)
                {
                    Trace.WriteLine(invoice.Vat);
                }
                RaisePropertyChanged(nameof(Invoices));
            }, c => true);

            RemoveCommand = new RelayCommand(e =>
            {
                MessageBoxResult messageBoxResult =
                    MessageBox.Show("Wollen Sie die Rechnung löschen?", "Löschen", MessageBoxButton.YesNo);
                switch (messageBoxResult)
                {
                    case MessageBoxResult.Yes:
                        InvoiceLogic.RemoveInvoice(SelectedInvoice);
                        RaisePropertyChanged(nameof(Invoices));
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }, c => true);

            PrintCommand = new RelayCommand(e =>
            {
                FlowDocument document = ERPViewModel.getFlowDocument("Printing/Invoice.xaml");

                var invoicePrintData = new InvoicePrintData();
                invoicePrintData.Invoice = SelectedInvoice;
                document.DataContext = invoicePrintData;

                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                    printDialog.PrintDocument((document as IDocumentPaginatorSource).DocumentPaginator, "Invoice");
            }, c => true);
        }

        private static FlowDocument getFlowDocument(String path)
        {
            String rawDocument = "";
            using (StreamReader streamReader = File.OpenText(path))
            {
                rawDocument = streamReader.ReadToEnd();
            }

            FlowDocument flowDocument = XamlReader.Load(new XmlTextReader(new StringReader(rawDocument))) as FlowDocument;
            return flowDocument;
        }

        public ICommand AddCommand { get; private set; }
        public ICommand RemoveCommand { get; private set; }
        public ICommand PrintCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
