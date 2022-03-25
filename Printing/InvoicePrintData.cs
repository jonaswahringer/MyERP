using _4_06_EF_ERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace _4_06_EF_ERP.Printing
{
    class InvoicePrintData
    {
        public DateTime PrintingDate => DateTime.Now;
        public Invoice Invoice { get; set; }
        public String Anschrift { get; set; } = "Schulzentrum Ybbs";
        public String Adresse { get; set; } = "Schulring 6, 3370 Ybbs/Donau";
        public BitmapSource BarCode { get; set; }
        public BitmapSource QrCode { get; set; }
    }
}
