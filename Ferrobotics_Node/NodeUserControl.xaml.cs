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
using VariableManager;

namespace Ferrobotics_Node
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class NodeUserControl : UserControl, ITMcraftNodeEntry
    {
        private IProjectVariableCtrl mProjectVariableCtrl = new ProjectVariableCtrl();
        private NodeViewModel mNodeViewModel = new NodeViewModel();
        private SetDataUserControl mSetDataUserControl = null;
        private TMcraftNodeAPI mTMcraftNodeAPI = null;
        public NodeUserControl()
        {
            InitializeComponent();
        }


        private void InitView()
        {
            mNodeViewModel.SetDataModel.BtnWriteVisible = Visibility.Hidden;
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

            mProjectVariableCtrl.UpdateFunctionPtr(mTMcraftNodeAPI.VariableProvider.IsProjectVariableExist
                                                 , mTMcraftNodeAPI.VariableProvider.CreateProjectVariable
                                                 , mTMcraftNodeAPI.VariableProvider.ChangeProjectVariableValue
                                                 , mTMcraftNodeAPI.VariableProvider.GetProjectVariableList);
            GetDataFromNode();
            InitView();
            mProjectVariableCtrl.UpdateDataFromProjectVariable();
        }

        public void InscribeScript(ScriptWriteProvider WriteProvider)
        {
            List<string> script_list = new List<string>();
            bool rtn_result = false;

            mNodeViewModel.SetDataModel.MergeStringData();

            Dictionary<string, string> save_data_table = new Dictionary<string, string>();
            save_data_table.Add("node_name", mNodeViewModel.SetDataModel.NodeName);
            save_data_table.Add("f_target", mNodeViewModel.SetDataModel.SetParam1.ToString());
            save_data_table.Add("f_zero", mNodeViewModel.SetDataModel.SetParam2.ToString());
            save_data_table.Add("t_ramp", mNodeViewModel.SetDataModel.SetParam3.ToString());
            save_data_table.Add("f_payload", mNodeViewModel.SetDataModel.SetParam4.ToString());
            save_data_table.Add("f_do", mNodeViewModel.SetDataModel.TargetDo.ToString());
            save_data_table.Add("display", mNodeViewModel.SetDataModel.Display.ToString());
            mTMcraftNodeAPI?.DataStorageProvider.SaveData(save_data_table);

            if (mProjectVariableCtrl.CheckVariableExist("var_ferrobotics_do_type") == false
                || mProjectVariableCtrl.CheckVariableExist("var_ferrobotics_do_channel") == false)
            {
                MessageBox.Show("Please set do channel in Start Node first!");
                return;
            }

            foreach(string str in save_data_table.Keys)
            {
                mProjectVariableCtrl.VariableModel.AddProjectVariable(str, save_data_table[str]);
            }

            mProjectVariableCtrl.UpdateProjectVariableFromData(ref rtn_result);

            if (mNodeViewModel.SetDataModel.Display == true)
            {
                //string script_cmd = string.Format("Display(\"Node name = \"+var_node_name+Ctrl(\"\\r\\n\")+\"f_target = \"+var_f_target+Ctrl(\"\\r\\n\")+\"f_zero = \"+var_f_zero+Ctrl(\"\\r\\n\")+\"t_ramp = \"+var_t_ramp + Ctrl(\"\\r\\n\")+\"Payload or Velocity = \"+var_f_payload+ Ctrl(\"\\r\\n\")+\"DO Status = \"+IO[\"{0}\"].DO[{1}])", );
                script_list.Add("Display(\"Node name = \"+var_node_name+Ctrl(\"\\r\\n\")+\"f_target = \"+var_f_target+Ctrl(\"\\r\\n\")+\"f_zero = \"+var_f_zero+Ctrl(\"\\r\\n\")+\"t_ramp = \"+var_t_ramp + Ctrl(\"\\r\\n\")+\"Payload or Velocity = \"+var_f_payload+ Ctrl(\"\\r\\n\")+\"DO Status = \"+IO[\"EndModule\"].DO[2])");
                //script_list.Add(script_cmd);
            }

            string cmd = string.Format("GetBytes(\"{0}\")"
                                   , mNodeViewModel.SetDataModel.WriteData);

            string target_do = (mNodeViewModel.SetDataModel.TargetDo == true) ? "1" : "0";

            script_list.Add("Socket ferrobotics = var_ferrobotics_ip, var_ferrobotics_port");
            script_list.Add("socket_open(\"ferrobotics\")");
            script_list.Add(string.Format("socket_send(\"ferrobotics\", {0})", cmd));
            script_list.Add("Sleep(5)");
            script_list.Add(string.Format("socket_send(\"ferrobotics\", {0})", cmd));
            script_list.Add("Sleep(5)");
            script_list.Add(string.Format("IO[var_ferrobotics_do_type].DO[var_ferrobotics_do_channel] = {0}"
                                        , target_do));

            foreach (string script in script_list) { WriteProvider?.AppendLine(script); }
        }

        private void GetDataFromNode()
        {
            if (mTMcraftNodeAPI == null) { return; }

            string f_target_str = "";
            string f_zero_str = "";
            string t_ramp_str = "";
            string f_payload_str = "";
            string f_do_str = "";
            string node_name_str = "";
            string display_str = "";

            decimal f_target = 0;
            decimal f_zero = 0;
            decimal t_ramp = 0;
            decimal f_payload = 0;
            bool f_do = false;
            bool display = false;

            mTMcraftNodeAPI.DataStorageProvider.GetData("node_name", out node_name_str);
            mTMcraftNodeAPI.DataStorageProvider.GetData("f_target", out f_target_str);
            mTMcraftNodeAPI.DataStorageProvider.GetData("f_zero", out f_zero_str);
            mTMcraftNodeAPI.DataStorageProvider.GetData("t_ramp", out t_ramp_str);
            mTMcraftNodeAPI.DataStorageProvider.GetData("f_payload", out f_payload_str);
            mTMcraftNodeAPI.DataStorageProvider.GetData("f_do", out f_do_str);
            mTMcraftNodeAPI.DataStorageProvider.GetData("display", out display_str);

            if ((false == decimal.TryParse(f_target_str, out f_target))
             || (false == decimal.TryParse(f_zero_str, out f_zero))
             || (false == decimal.TryParse(t_ramp_str, out t_ramp))
             || (false == decimal.TryParse(f_payload_str, out f_payload))
             || (false == bool.TryParse(f_do_str, out f_do))
             || (false == bool.TryParse(display_str, out display))
                )
            {
                return;
            }

            mNodeViewModel.SetDataModel.SetParam1 = Convert.ToDecimal(f_target);
            mNodeViewModel.SetDataModel.SetParam2 = Convert.ToDecimal(f_zero);
            mNodeViewModel.SetDataModel.SetParam3 = Convert.ToDecimal(t_ramp);
            mNodeViewModel.SetDataModel.SetParam4 = Convert.ToDecimal(f_payload);
            mNodeViewModel.SetDataModel.TargetDo = f_do;
            mNodeViewModel.SetDataModel.NodeName = node_name_str;
            mNodeViewModel.SetDataModel.Display = display;
        }

        private void radio_tool_state_Checked(object sender, RoutedEventArgs e)
        {
            radio_tool_state.Tag = (radio_tool_state.IsChecked == true) ? "ON" : "OFF";
        }

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            mTMcraftNodeAPI?.Close();
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            GetDataFromNode();
            mTMcraftNodeAPI?.Close();
        }

    }

}
