using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MyERP.Context;
using MyERP.Model;

namespace MyERP.ViewModel
{
    class InvoiceViewModel : INotifyPropertyChanged
    {
        #region properties
        public InvoiceContext InvoiceCTX { get; set; }
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
                addUser();
            }, c => true);

            RemoveCommand = new RelayCommand(e =>
            {
                
            }, c => true);
        }
        #endregion

        #region methods
        public void addUser()
        {
            using (InvoiceCTX = new InvoiceContext())
            {
                Invoice inv = new Invoice()
                {
                    CustomerName = "Mike",
                    CustomerAddress = "King's Street 1",
                    Amount = 10000.0,
                    Vat = 10,
                    InvoiceDate = DateTime.Now
                };

                try
                {
                    Console.WriteLine(inv.Id);
                    InvoiceCTX.invoices.Add(inv);
                    InvoiceCTX.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                
            }
        }
        #endregion

        #region commands
        public ICommand ExitCommand { get; private set; }
        public ICommand AddCommand { get; private set; }
        public ICommand RemoveCommand { get; private set; }
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
