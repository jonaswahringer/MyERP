using MyERP.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml;

// get all the data we need for printing

namespace MyERP.Printing
{
    class InvoicePrintData
    {
        #region properties
        public DateTime PrintingDate => DateTime.Now;
        public Invoice Invoice { get; set; }
        //optional, weil EagerLoading
        public ICollection<InvoicePosition> Positions { get; set; }
        #endregion
    }
}
