using System.ComponentModel;
using System.Globalization;
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
using System.Windows.Threading;
using TMcraft;
using VariableManager;

namespace Ferrobotics_Toolbar
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ToolbarUserControl : UserControl, ITMcraftToolbarEntry
    {
        private TMcraftToolbarAPI mTMcraftToolbarAPI = null;
        private ToolbarViewModel mToolbarViewModel = new ToolbarViewModel();
        private ICommunicationCtrl mCommunicationCtrl = new CommunicationCtrl();

        private static DispatcherTimer readDataTimer = new DispatcherTimer();



        public ToolbarUserControl()
        {
            InitializeComponent();
            DataContext = mToolbarViewModel;
            AddHandler(Keyboard.KeyDownEvent, (KeyEventHandler)HandleKeyDownEvent);
            chb_edit_mode.Visibility = Visibility.Hidden;
        }

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab && (Keyboard.Modifiers & (ModifierKeys.Control)) == (ModifierKeys.Control))
            {
                chb_edit_mode.Visibility = (chb_edit_mode.Visibility == Visibility.Visible) ? Visibility.Hidden : Visibility.Visible;
            }
        }

    
        public void InitializeToolbar(TMcraftToolbarAPI Api)
        {
            mTMcraftToolbarAPI = Api;
        }

        private void btn_connect_Click(object sender, RoutedEventArgs e)
        {
            Connect();
        }


        private void Connect()
        {
            mCommunicationCtrl.InitConnection(mToolbarViewModel.IP, mToolbarViewModel.Port);

            mToolbarViewModel.ConnectState = mCommunicationCtrl.ConnectState;

            if (mToolbarViewModel.ConnectState == "Normal")
            {
                readDataTimer.Tick += new EventHandler(TimerFunction);
                readDataTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
                readDataTimer.Start();
            }
            else
            {
                readDataTimer.Stop();
            }
        }


        public void TimerFunction(object? sender, EventArgs e)
        {
            if (mToolbarViewModel.ConnectState != "Normal") { return; }

            bool rtn_run = false;
            mTMcraftToolbarAPI.RobotStatusProvider.ProjectRunOrNot(out rtn_run);

            if (rtn_run == true) { return; }

            mCommunicationCtrl.WriteData(mToolbarViewModel.WriteData);

            string[] data_read = mCommunicationCtrl.ReadData().Split(' ');
            if (data_read.Length == 5) 
            {
                mToolbarViewModel.GetParam1 = Convert.ToDecimal(data_read[1]);
                mToolbarViewModel.GetParam2 = Convert.ToDecimal(data_read[2]);
                mToolbarViewModel.GetParam3 = Convert.ToDecimal(data_read[3]);
                mToolbarViewModel.GetParam4 = Convert.ToDecimal(data_read[4].Replace("k", ""));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (mToolbarViewModel.ConnectState != "Normal") { return; }

            string data_write = string.Format("ferbak1040 {0} {1} {2} {3} 0"
                                             , mToolbarViewModel.SetParam1.ToString()
                                             , mToolbarViewModel.SetParam2.ToString()
                                             , mToolbarViewModel.SetParam3.ToString()
                                             , mToolbarViewModel.SetParam4.ToString());

            mToolbarViewModel.WriteData = data_write;
        }
    }



    public class TextToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == "False")
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ReverTextToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == "True")
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
