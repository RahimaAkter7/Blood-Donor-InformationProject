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

namespace Donor
{
    /// <summary>
    /// Interaction logic for ViewWindow.xaml
    /// </summary>
    public partial class ViewWindow : Window
    {
        public ViewWindow()
        {
            InitializeComponent();
        }

        private void btn_BACK_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            //ShowAllWindow showAll = new ShowAllWindow();
            //showAll.Show();
            main.ShowData();
            this.Close();
           
            

        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();

        }
    }
}
