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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dev.Wpf.HostCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid() == false)
                return;


        }

        private bool IsValid()
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(textUserName.Text))
            {
                isValid = isValid && false;
            }

            if (string.IsNullOrEmpty(textPassword.Text))
            {
                isValid = isValid && false;
            }
            
            return isValid;
        }
    }
}
