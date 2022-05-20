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
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Drawing.Imaging;
using QRCoder;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using LiveCharts.Configurations;
using _4_06_EF_ERP.MQTT;

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
                RaisePropertyChanged(nameof(SeriesAmountInvoicePosition));
                RaisePropertyChanged();
            }
        }

        public List<Position> SelectedPositions { get; set; } = new List<Position>();
        public int selectedPositionsSum = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public int SelectedPositionsSum
        {
            get => selectedPositionsSum;
            set
            {
                selectedPositionsSum = value;
                RaisePropertyChanged();
            }
        }

        public SeriesCollection SeriesCollectionInvoiceAmount
        {
            get
            {
                //DB Zugriff
                using (var context = new InvoiceContext())
                {
                    var invoices = context.Invoices.OrderBy(i => i.InvoiceDate);

                    var seriesCollection = new SeriesCollection();

                    var lineSeries = new LineSeries
                    {
                        Title = "Rechnungsverlauf",
                        Values = new ChartValues<DateTimePoint>(),
                    };

                    foreach (var invoice in invoices)
                    {
                        lineSeries.Values.Add(new DateTimePoint
                        {
                            DateTime = invoice.InvoiceDate,
                            Value = invoice.Amount
                        });
                    }
                    seriesCollection.Add(lineSeries);
                    return seriesCollection;
                }
            }
        }
        public string[] LabelsInvoiceAmount { get; set; }
        public Func<double, string> YFormatterInvoiceAmount { get; set; }
        public Func<double, string> XFormatterInvoiceAmount { get; set; }

        public SeriesCollection SeriesAmountInvoicePosition
        {
            get
            {
                Func<ChartPoint, string> labelPoint = chartPoint => $"{chartPoint.Y}({chartPoint.Participation:P})";
                var seriesCollection = new SeriesCollection();

                if (SelectedInvoice != null)
                {
                    foreach (var position in SelectedInvoice.Positions)
                    {
                        seriesCollection.Add(new PieSeries
                        {
                            Title = position.ItemNr.ToString(),
                            Values = new ChartValues<double> { position.Qty },
                            PushOut = position.Id == 1 ? 10 : 0,
                            DataLabels = true,
                            LabelPoint = labelPoint
                        });
                    }
                }
                return seriesCollection;
            }
        }

        public SeriesCollection SeriesCollectionBubbleChart
        {
            get
            {
                using (var context = new InvoiceContext())
                {
                    List<BubbleChartData> bubbleChartData = new List<BubbleChartData>();

                    foreach(var invoice in Invoices)
                    {
                        bubbleChartData.Add(new BubbleChartData
                        {
                            InvoiceDate = invoice.InvoiceDate,
                            Amount = invoice.Amount,
                            AmountOfPosition = invoice.Positions.Count
                        });
                    }

                    var seriesCollection = new SeriesCollection()
                    {
                        new ScatterSeries
                        {
                            Values = new ChartValues<BubbleChartData>(bubbleChartData),
                            Configuration = Mappers.Weighted<BubbleChartData>()
                            .X(i => i.InvoiceDate.Ticks)
                            .Y(i => i.Amount)
                            .Weight(i => i.AmountOfPosition)
                        }
                    };
                    
                    return seriesCollection;
                }
            }
        }



        public ERPViewModel()
        {
            MqttClient mqttClient = new MqttClient();
            mqttClient.ClientId = "CoolerClient";
            mqttClient.ServerURL = "localhost";
            mqttClient.Init();

            AddCommand = new RelayCommand(e =>
            {
                InvoiceToAdd.InvoiceDate = DateTime.Now;
                InvoiceLogic.AddInvoice(InvoiceToAdd);
                RaisePropertyChanged(nameof(Invoices));
                RaisePropertyChanged(nameof(SeriesCollectionInvoiceAmount));
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
                        RaisePropertyChanged(nameof(SeriesCollectionInvoiceAmount));
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
                invoicePrintData.BarCode = this.CreateBarCode("12345");
                invoicePrintData.QrCode = this.CreateQrCode("12345");
                document.DataContext = invoicePrintData;

                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                    printDialog.PrintDocument((document as IDocumentPaginatorSource).DocumentPaginator, "Invoice");
            }, c => true);

            FreigabeCommand = new RelayCommand(async e =>
            {
                Console.WriteLine("POSITION COUNT: {0}", SelectedPositions.Count);
                if (SelectedPositions.Count > 0)
                {
                    Console.WriteLine("SENDING POSITIONS ...");
                    foreach (var position in SelectedPositions)
                    {
                        if (await mqttClient.SendInvoicePosition(position) == false)
                        {
                            showMessageBox();
                        };
                    }
                }
                else if (SelectedInvoice != null)
                {
                    Console.WriteLine("SENDING INVOICE ...");
                    if (await mqttClient.SendInvoice(SelectedInvoice) == false)
                    {
                        showMessageBox();
                    };
                }
            }, c => SelectedInvoice != null || SelectedPositions.Count>0);

            YFormatterInvoiceAmount = value => value.ToString("C");
            XFormatterInvoiceAmount = value => new DateTime((long)value).ToString("dd.MM.yyyy");
        }

        private static void showMessageBox()
        {
            MessageBoxResult messageBoxResult =
                MessageBox.Show("ES KONNTE KEINE VERBINDUNG ZU MQTT HERGESTELLT WERDEN!", "MQTT", MessageBoxButton.OK);
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

        public ICommand AddCommand { get; private set; }
        public ICommand RemoveCommand { get; private set; }
        public ICommand PrintCommand { get; private set; }
        public ICommand FreigabeCommand { get; private set; }

        private void RaisePropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
