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
        private ICommunicationCtrl mICommunicationCtrl = new CommunicationCtrl();
        private static DispatcherTimer mReadDataTimer = new DispatcherTimer();


        public UserControl1()
        {
            InitializeComponent();
            tb_send.Text = "ferbak1040 21 0.5 0.5 0.5 0";
        }

        private void btn_connect_Click(object sender, RoutedEventArgs e)
        {
            mICommunicationCtrl.InitConnection("192.168.99.1", 7070);

            if (mICommunicationCtrl.ConnectState == "Normal")
            {
                mReadDataTimer.Tick += new EventHandler(Timer_UpdateData);
                mReadDataTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
                mReadDataTimer.Start();
            }

        }

        private void btn_write_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Timer_UpdateData(object? sender, EventArgs e)
        {
            lb_state.Content = mICommunicationCtrl.ConnectState;
            mICommunicationCtrl.WriteData(tb_send.Text);
            string recv_data = mICommunicationCtrl.ReadData();
            tb_recv.Text = recv_data;
        }

        private void btn_toolbar_Click(object sender, RoutedEventArgs e)
        {
            Window win_tool = new Window();
            win_tool.Width = 440;
            win_tool.Height = 700;
            win_tool.Content = new ToolbarUserControl();
            win_tool.Title = "Toolbar";
            win_tool.WindowStyle = WindowStyle.ToolWindow;
            win_tool.Show();
        }

        private void btn_node_Click(object sender, RoutedEventArgs e)
        {
            Window win = new Window();
            win.Width = 800;
            win.Height = 600;
            win.Content = new NodeUserControl();
            win.Title = "Node";
            win.WindowStyle = WindowStyle.ToolWindow;
            win.Show();
        }

        private void btn_setup_Click(object sender, RoutedEventArgs e)
        {
            Window win_tool = new Window();
            win_tool.Width = 900;
            win_tool.Height = 600;
            win_tool.Content = new SetupUserControl();
            win_tool.Title = "Toolbar";
            win_tool.WindowStyle = WindowStyle.ToolWindow;
            win_tool.Show();
             
        }
    }
}