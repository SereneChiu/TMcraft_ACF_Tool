using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using VariableManager;
using Ferrobotics_Toolbar;
using Ferrobotics_Setup;
using Ferrobotics_Node;

namespace TMcraft_ACF_K_UserControl
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        private void btn_toolbar_Click(object sender, RoutedEventArgs e)
        {
            Window win_tool = new Window();
            win_tool.Width = 500;
            win_tool.Height = 750;
            win_tool.Content = new ToolbarUserControl();
            win_tool.Title = "Toolbar";
            win_tool.WindowStyle = WindowStyle.ToolWindow;
            win_tool.Show();
        }

        private void btn_node_Click(object sender, RoutedEventArgs e)
        {
            Window win = new Window();
            win.Width = 1100;
            win.Height = 600;
            win.Content = new NodeUserControl();
            win.Title = "Node";
            win.WindowStyle = WindowStyle.ToolWindow;
            win.Show();
        }

        private void btn_setup_Click(object sender, RoutedEventArgs e)
        {
            Window win_tool = new Window();
            win_tool.Width = 950;
            win_tool.Height = 500;
            win_tool.Content = new SetupUserControl();
            win_tool.Title = "Toolbar";
            win_tool.WindowStyle = WindowStyle.ToolWindow;
            win_tool.Show();
             
        }
    }
}