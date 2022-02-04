using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyERP.Model
{
    class InvoiceList
    {
        public ObservableCollection<Invoice> InvoiceLists { get; set; } = new ObservableCollection<Invoice>();

        public static InvoiceList ConvertList(List<Invoice> liste)
        {
            return new InvoiceList
            {
                InvoiceLists = new ObservableCollection<Invoice>(liste)
            };
        }
    }
}
