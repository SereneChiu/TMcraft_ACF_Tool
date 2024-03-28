using System.Text;
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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class SetupUserControl : UserControl
    {
        public SetupUserControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_browse_Click(object sender, RoutedEventArgs e)
        {
            Window win_tool = new Window();
            win_tool.Width = 800;
            win_tool.Height = 300;
            win_tool.Content = new DigitalOutputSelectDialog();
            win_tool.Title = "Digital Output Select Dialog";
            win_tool.WindowStyle = WindowStyle.ToolWindow;
            win_tool.Show();
        }
    }

}
