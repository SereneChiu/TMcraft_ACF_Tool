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

namespace Ferrobotics_Setup
{
    /// <summary>
    /// Interaction logic for DigitalInputSelectDialog.xaml
    /// </summary>
    public partial class DigitalOutputSelectDialog : UserControl
    {
        public DigitalOutputSelectDialog()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (page_do == null) { return; }
            Button btn = new Button();
            btn.Content = "Hello";
            page_do.Children.Add(btn);

        }
    }
}
