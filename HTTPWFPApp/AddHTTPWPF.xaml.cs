using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HTTPWFPApp
{
    /// <summary>
    /// Interaction logic for AddHTTPWPF.xaml
    /// </summary>
    public partial class AddHTTPWPF : Window
    {

        public string? username { get; set; }
        public string? surname { get; set; }
        public AddHTTPWPF()
        {
            InitializeComponent();
        }


        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            username = Username.Text;
            surname = Surname.Text;
            Close();
        }
    }
}
