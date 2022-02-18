﻿using System;
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
                using (InvoiceCTX)
                {
                    return (from data in InvoiceCTX.Invoices select data).ToList();
                }
            }
        }

        public Invoice InvoiceToAdd { get; set; } = new Invoice();
        public Invoice InvoiceToRemove { get; set; } = new Invoice();

        #endregion

        #region constructor
        public InvoiceViewModel()
        {
            //temp method to test the 1:n relationship
            TestRelationship();

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
                RemoveInvoice(InvoiceToRemove);
            }, c => InvoiceToRemove != null);
        }
        #endregion

        #region methods
        public void TestRelationship()
        {
            using(InvoiceCTX = new InvoiceContext())
            {
                // Invoice myInnervoice = InvoiceCTX.Invoices.Include(inv => inv.InvoicePositions.Select(ud => ud.ItemNr)).FirstOrDefault(inv => inv.Id == 1);
                Console.WriteLine(InvoiceCTX.Invoices.Include("InvoicePositions").FirstOrDefault(inv => inv.Id == 1));
            }
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