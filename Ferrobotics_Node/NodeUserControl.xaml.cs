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

            mSetDataUserControl = new SetDataUserControl(mNodeViewModel.SetDataModel);
            panel_ctrl.Content = mSetDataUserControl;
        }

        public void InitializeNode(TMcraftNodeAPI NodeAPI)
        {
            mTMcraftNodeAPI = NodeAPI;

            if (mTMcraftNodeAPI == null) { return; }

            string f_target_str = "";
            string f_zero_str = "";
            string f_ramp_str = "";
            string f_payload_str = "";

            decimal f_target = 0;
            decimal f_zero = 0;
            decimal f_ramp = 0;
            decimal f_payload = 0;

            mTMcraftNodeAPI.DataStorageProvider.GetData("f_target", out f_target_str);
            mTMcraftNodeAPI.DataStorageProvider.GetData("f_zero", out f_zero_str);
            mTMcraftNodeAPI.DataStorageProvider.GetData("f_ramp", out f_ramp_str);
            mTMcraftNodeAPI.DataStorageProvider.GetData("f_payload", out f_payload_str);

            if ((false == decimal.TryParse(f_target_str, out f_target))
                || (false == decimal.TryParse(f_zero_str, out f_zero))
                || (false == decimal.TryParse(f_ramp_str, out f_ramp))
                || (false == decimal.TryParse(f_payload_str, out f_payload))
                )
            {
                return;
            }

            mNodeViewModel.SetDataModel.SetParam1 = Convert.ToDecimal(f_target);
            mNodeViewModel.SetDataModel.SetParam2 = Convert.ToDecimal(f_zero);
            mNodeViewModel.SetDataModel.SetParam3 = Convert.ToDecimal(f_ramp);
            mNodeViewModel.SetDataModel.SetParam4 = Convert.ToDecimal(f_payload);

        }

        public void InscribeScript(ScriptWriteProvider WriteProvider)
        {
            List<string> script_list = new List<string>();


            string f_target = mNodeViewModel.SetDataModel.SetParam1.ToString();
            string f_zero = mNodeViewModel.SetDataModel.SetParam2.ToString();
            string f_ramp = mNodeViewModel.SetDataModel.SetParam3.ToString();
            string f_payload = mNodeViewModel.SetDataModel.SetParam4.ToString();

            string cmd = string.Format("GetBytes(\"ferbak1040 {0} {1} {2} {3} 0\")"
                                      , f_target
                                      , f_zero
                                      , f_ramp
                                      , f_payload);


            script_list.Add("Socket ferrobotics = var_ferrobotics_ip, var_ferrobotics_port");
            script_list.Add("socket_open(\"ferrobotics\")");
            script_list.Add(string.Format("socket_send(\"ferrobotics\", {0})", cmd));
            script_list.Add("Sleep(5)");
            script_list.Add(string.Format("socket_send(\"ferrobotics\", {0})", cmd));
            script_list.Add("Sleep(5)");
            foreach (string script in script_list) { WriteProvider?.AppendLine(script); }

            Dictionary<string, string> save_data = new Dictionary<string, string>();
            save_data.Add("f_target", f_target);
            save_data.Add("f_zero", f_zero);
            save_data.Add("f_ramp", f_ramp);
            save_data.Add("f_payload", f_payload);
            mTMcraftNodeAPI?.DataStorageProvider.SaveData(save_data);
            mTMcraftNodeAPI?.Close();
        }
    }

}
