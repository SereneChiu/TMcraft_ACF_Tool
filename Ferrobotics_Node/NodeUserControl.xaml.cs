using System.Net.Sockets;
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
using Ferrobotics_Controller;
using TMcraft;

namespace Ferrobotics_Node
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class NodeUserControl : UserControl, ITMcraftNodeEntry
    {
        private NodeViewModel mNodeViewModel = new NodeViewModel();
        private SetDataUserControl mSetDataUserControl = null;
        private TMcraftNodeAPI mTMcraftNodeAPI = null;
        public NodeUserControl()
        {
            InitializeComponent();

            DataContext = mNodeViewModel.SetDataModel;
            mSetDataUserControl = new SetDataUserControl(mNodeViewModel.SetDataModel);
            panel_ctrl.Content = mSetDataUserControl;
            AddHandler(Keyboard.KeyDownEvent, (KeyEventHandler)HandleKeyDownEvent);

        }

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab && (Keyboard.Modifiers & (ModifierKeys.Control)) == (ModifierKeys.Control))
            {
                mSetDataUserControl?.UpdateEditMode();
            }
        }


        public void InitializeNode(TMcraftNodeAPI NodeAPI)
        {
            mTMcraftNodeAPI = NodeAPI;

            if (mTMcraftNodeAPI == null) { return; }

            string f_target_str = "";
            string f_zero_str = "";
            string f_ramp_str = "";
            string f_payload_str = "";
            string f_do_str = "";

            decimal f_target = 0;
            decimal f_zero = 0;
            decimal f_ramp = 0;
            decimal f_payload = 0;
            bool f_do = false;

            mTMcraftNodeAPI.DataStorageProvider.GetData("f_target", out f_target_str);
            mTMcraftNodeAPI.DataStorageProvider.GetData("f_zero", out f_zero_str);
            mTMcraftNodeAPI.DataStorageProvider.GetData("f_ramp", out f_ramp_str);
            mTMcraftNodeAPI.DataStorageProvider.GetData("f_payload", out f_payload_str);
            mTMcraftNodeAPI.DataStorageProvider.GetData("f_do", out f_do_str);

            if ((false == decimal.TryParse(f_target_str, out f_target))
                || (false == decimal.TryParse(f_zero_str, out f_zero))
                || (false == decimal.TryParse(f_ramp_str, out f_ramp))
                || (false == decimal.TryParse(f_payload_str, out f_payload))
                || (false == bool.TryParse(f_do_str, out f_do))

                )
            {
                return;
            }

            mNodeViewModel.SetDataModel.SetParam1 = Convert.ToDecimal(f_target);
            mNodeViewModel.SetDataModel.SetParam2 = Convert.ToDecimal(f_zero);
            mNodeViewModel.SetDataModel.SetParam3 = Convert.ToDecimal(f_ramp);
            mNodeViewModel.SetDataModel.SetParam4 = Convert.ToDecimal(f_payload);
            mNodeViewModel.SetDataModel.TargetDo = Convert.ToBoolean(f_do);
        }

        public void InscribeScript(ScriptWriteProvider WriteProvider)
        {
            List<string> script_list = new List<string>();

            mNodeViewModel.SetDataModel.MergeStringData();

            string cmd = string.Format("GetBytes(\"{0}\")"
                                      , mNodeViewModel.SetDataModel.WriteData);


            script_list.Add("Socket ferrobotics = var_ferrobotics_ip, var_ferrobotics_port");
            script_list.Add("socket_open(\"ferrobotics\")");
            script_list.Add(string.Format("socket_send(\"ferrobotics\", {0})", cmd));
            script_list.Add("Sleep(5)");
            script_list.Add(string.Format("socket_send(\"ferrobotics\", {0})", cmd));
            script_list.Add("Sleep(5)");
            foreach (string script in script_list) { WriteProvider?.AppendLine(script); }

            Dictionary<string, string> save_data = new Dictionary<string, string>();
            save_data.Add("f_target", mNodeViewModel.SetDataModel.SetParam1.ToString());
            save_data.Add("f_zero", mNodeViewModel.SetDataModel.SetParam2.ToString());
            save_data.Add("f_ramp", mNodeViewModel.SetDataModel.SetParam3.ToString());
            save_data.Add("f_payload", mNodeViewModel.SetDataModel.SetParam4.ToString());
            save_data.Add("f_do", mNodeViewModel.SetDataModel.TargetDo.ToString());
            mTMcraftNodeAPI?.DataStorageProvider.SaveData(save_data);
            mTMcraftNodeAPI?.Close();
        }

        private void radio_tool_state_Checked(object sender, RoutedEventArgs e)
        {
            radio_tool_state.Tag = (radio_tool_state.IsChecked == true) ? "ON" : "OFF";

        }
    }

}
