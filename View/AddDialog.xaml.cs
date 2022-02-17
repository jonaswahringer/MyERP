using _4_06_EF_ERP.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _4_06_EF_ERP.View
{
    /// <summary>
    /// Interaktionslogik für AddDialog.xaml
    /// </summary>
    public partial class AddDialog : Window
    {
        public AddDialog()
        {
            InitializeComponent();
        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void AddInvoice(object sender, RoutedEventArgs e)
        {
            var viewModel = (ERPViewModel) DataContext;

            if (viewModel.AddCommand.CanExecute(null))
            {
                viewModel.AddCommand.Execute(null);
            }
                
            
            this.Close();
        }
    }
}
