using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MyERP.Context;
using MyERP.Model;
using System.Data.Entity;
using System.Windows.Documents;
using System.IO;
using System.Windows.Markup;
using System.Xml;
using MyERP.Printing;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Drawing.Imaging;
using QRCoder;
using System.Windows;

namespace MyERP.ViewModel
{
    class InvoiceViewModel : INotifyPropertyChanged
    {
        #region properties

        public InvoiceContext InvoiceCTX = new InvoiceContext();

        public IList<Invoice> InvoiceList
        {
            get
            {
                using (InvoiceCTX = new InvoiceContext())
                {
                    return (from data in InvoiceCTX.Invoices.Include(p => p.Positions) select data).ToList();
                }
            }
        }

        public Invoice InvoiceToAdd { get; set; } = new Invoice();
        private Invoice selectedInvoice { get; set; }
        public Invoice SelectedInvoice
        {
            get => selectedInvoice;
            set
            {
                selectedInvoice = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region constructor
        public InvoiceViewModel()
        {
            ExitCommand = new RelayCommand(e =>
            {
                System.Environment.Exit(0);
            }, c => true);

            AddCommand = new RelayCommand(e =>
            {
                AddInvoice();
                RaisePropertyChanged();
                InvoiceToAdd = new Invoice();
            }, c => InvoiceToAdd.CustomerName != null && InvoiceToAdd.CustomerAddress != null);

            RemoveCommand = new RelayCommand(e =>
            {
                RemoveInvoice(SelectedInvoice);
            }, c => SelectedInvoice != null);

            PrintCommand = new RelayCommand(e =>
            {
                FlowDocument document = InvoiceViewModel.getFlowDocument("Printing/InvoicePrint.xaml");

                var invoicePrintData = new InvoicePrintData();
                invoicePrintData.Invoice = SelectedInvoice;
                invoicePrintData.Positions = SelectedInvoice.Positions;
                invoicePrintData.QrCode = CreateQrCode("123");
                invoicePrintData.BarCode = CreateBarCode("123");
                document.DataContext = invoicePrintData;

                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                    printDialog.PrintDocument((document as IDocumentPaginatorSource).DocumentPaginator, "Invoice");
            }, c => SelectedInvoice != null);
        }
        #endregion

        #region methods
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

        private BitmapSource CreateBarCode(string toCode)
        {
            BarcodeLib.Barcode b = new BarcodeLib.Barcode();
            System.Drawing.Image img = b.Encode(BarcodeLib.TYPE.CODE93, toCode, Color.Black, Color.White, 100, 50);

            using (var memory = new MemoryStream())
            {
                img.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        private BitmapSource CreateQrCode(string toCode)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(toCode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap result = qrCode.GetGraphic(20);

            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                         result.GetHbitmap(),
                         IntPtr.Zero,
                         Int32Rect.Empty,
                         BitmapSizeOptions.FromEmptyOptions());
        }

        public void AddInvoice()
        {
            InvoiceToAdd.InvoiceDate = DateTime.Now;
            
            using (InvoiceCTX = new InvoiceContext())
            {
                try
                {
                    InvoiceCTX.Invoices.Add(InvoiceToAdd);
                    InvoiceCTX.SaveChanges();
                    RaisePropertyChanged(nameof(InvoiceList));
                    InvoiceToAdd = new Invoice();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void RemoveInvoice(Invoice invToRemove)
        {
            using (InvoiceCTX = new InvoiceContext())
            {
                try
                {
                    Invoice temp = InvoiceCTX.Invoices.Find(invToRemove.Id);
                    InvoiceCTX.Invoices.Remove(temp);
                    InvoiceCTX.SaveChanges();
                    RaisePropertyChanged(nameof(InvoiceList));
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            ;
        }
        #endregion

        #region commands
        public ICommand ExitCommand { get; private set; }
        public ICommand AddCommand { get; private set; }
        public ICommand RemoveCommand { get; private set; }
        public ICommand PrintCommand { get; private set; }
        #endregion

        #region notify
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
