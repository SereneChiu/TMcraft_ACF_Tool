using Ferrobotics_Controller;
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
        private SetDataUserControl mSetDataUserControl = null;

        private ToolbarViewModel mToolbarViewModel = new ToolbarViewModel();
        private ICommunicationCtrl mCommunicationCtrl = new CommunicationCtrl();
        private IProjectVariableCtrl mProjectVariableCtrl = new ProjectVariableCtrl();

        private static DispatcherTimer readDataTimer = new DispatcherTimer();


        public ToolbarUserControl()
        {
            InitializeComponent();
            DataContext = mToolbarViewModel;
            AddHandler(Keyboard.KeyDownEvent, (KeyEventHandler)HandleKeyDownEvent);
            chb_edit_mode.Visibility = Visibility.Hidden;

            mToolbarViewModel.SetDataModel.TypeSelectable = true;



            mSetDataUserControl = new SetDataUserControl(mToolbarViewModel.SetDataModel);
            page_control.Content = mSetDataUserControl;

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


            mProjectVariableCtrl.UpdateFunctionPtr(mTMcraftToolbarAPI.VariableProvider.GetProjectVariableList);
            mProjectVariableCtrl.UpdateDataFromProjectVariable();

            if (mProjectVariableCtrl.VariableModel.VarTable.ContainsKey("ferrobotics_tool_type") == true)
            {
                mToolbarViewModel.SetDataModel.DevEntry = mProjectVariableCtrl.VariableModel.VarTable["ferrobotics_tool_type"].VarValue.Replace("\"", "");
            }

            if (mProjectVariableCtrl.VariableModel.VarTable.ContainsKey("ferrobotics_tool"))
            {
                this.mToolbarViewModel.SetDataModel.DevSubEntry = mProjectVariableCtrl.VariableModel.VarTable["ferrobotics_tool"]?.VarValue?.Replace("\"", "");
            }

            if (mProjectVariableCtrl.VariableModel.VarTable.ContainsKey("ferrobotics_payload_vel"))
            {
                decimal rtn_value = 0;
                string input_value = mProjectVariableCtrl.VariableModel.VarTable["ferrobotics_payload_vel"]?.VarValue?.Replace("\"", "");
                if (true == decimal.TryParse(input_value, out rtn_value))
                {
                    this.mToolbarViewModel.SetDataModel.SetParam4 = rtn_value;
                }
            }
        }

        private void btn_connect_Click(object sender, RoutedEventArgs e)
        {
            Connect();
        }


        private void Connect(bool ForceConnect = false)
        {
            mCommunicationCtrl.InitConnection(mToolbarViewModel.IP, mToolbarViewModel.Port, ForceConnect);

            mToolbarViewModel.ConnectState = mCommunicationCtrl.ConnectState;

            if (mToolbarViewModel.ConnectState == "Normal")
            {
                readDataTimer.Tick += new EventHandler(TimerFunction);
                readDataTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
                mToolbarViewModel.SetDataModel.MergeStringData();
                readDataTimer.Start();
            }
            else
            {
                readDataTimer.Stop();
            }
        }


        public void TimerFunction(object? sender, EventArgs e)
        {
            mToolbarViewModel.ConnectState = mCommunicationCtrl.ConnectState;

            if (mToolbarViewModel.ConnectState != "Normal") { return; }

            bool rtn_run = false;

            if (mTMcraftToolbarAPI != null)
            {
                mTMcraftToolbarAPI.RobotStatusProvider.ProjectRunOrNot(out rtn_run);
                if (rtn_run == true) 
                {
                    mToolbarViewModel.ConnectState = "Project running";
                    panel_main.IsEnabled = false;
                    return; 
                }
            }

            if (panel_main.IsEnabled == false)
            {
                Connect(true);
                panel_main.IsEnabled = true;
                return;
            }

            mCommunicationCtrl.WriteData(mToolbarViewModel.WriteData);

            string[] data_read = mCommunicationCtrl.ReadData().Split(' ');
            if (data_read.Length == 5) 
            {
                mToolbarViewModel.GetParam1 = Math.Truncate(Convert.ToDecimal(data_read[1]));
                mToolbarViewModel.GetParam2 = Convert.ToDecimal(data_read[2]);
                mToolbarViewModel.GetParam3 = Convert.ToDecimal(data_read[3]);
                mToolbarViewModel.GetParam4 = Convert.ToDecimal(data_read[4].Replace("k", ""));
            }
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
