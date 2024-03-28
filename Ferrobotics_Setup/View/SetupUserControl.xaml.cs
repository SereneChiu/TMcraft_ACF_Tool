using Ferrobotics_Setup.Model;
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
using TMcraft;
using VariableManager;
using static TMcraft.TMcraftSetupAPI;

namespace Ferrobotics_Setup
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class SetupUserControl : UserControl, ITMcraftSetupEntry
    {

        private SetupModel mSetupModel = new SetupModel();
   
        private static List<string> var_name_list = new List<string>()
        {
            "var_ferrobotics_ip"
          , "var_ferrobotics_port"
          , "var_ferrobotics_do_type"
          , "var_ferrobotics_do_channel"
          , "var_ferrobotics_do_status"
        };

        private Dictionary<string, VariableModel> mVarDict = new Dictionary<string, VariableModel>();

        public SetupUserControl()
        {
            DataContext = mSetupModel;
            InitializeComponent();
            InitDictData();
        }

        public void InitializeSetup(TMcraftSetupAPI TMcraftSetupAPI)
        {
            mSetupModel.TMcraftSetupAPI = TMcraftSetupAPI;
        }

        private void InitDictData()
        {
            mVarDict = new Dictionary<string, VariableModel>()
            {
                { var_name_list[0], new VariableModel(var_name_list[0], VariableType.String, mSetupModel.Ip) }
              , { var_name_list[1], new VariableModel(var_name_list[1], VariableType.Integer, mSetupModel.Port) }
              , { var_name_list[2], new VariableModel(var_name_list[2], VariableType.Integer, mSetupModel.CurSelectDoType) }
              , { var_name_list[3], new VariableModel(var_name_list[3], VariableType.Integer, mSetupModel.CurSelectDoIdx) }
              , { var_name_list[4], new VariableModel(var_name_list[3], VariableType.Boolean, mSetupModel.DoStatus) }
            };
        }

        private void btn_browse_Click(object sender, RoutedEventArgs e)
        {
            Window win_tool = new Window();
            win_tool.Width = 800;
            win_tool.Height = 300;
            win_tool.Content = new DigitalOutputSelectDialog(mSetupModel);
            win_tool.Title = "Digital Output Select Dialog";
            win_tool.WindowStyle = WindowStyle.ToolWindow;
            win_tool.ResizeMode = ResizeMode.NoResize;
            win_tool.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win_tool.Show();
        }

        
        private void btn_apply_Click(object sender, RoutedEventArgs e)
        {
            SetupVariableProvider variable_provider = mSetupModel.TMcraftSetupAPI.VariableProvider;
           
            foreach(VariableModel var in mVarDict.Values)
            {
                if (false == variable_provider.IsProjectVariableExist(var.VarName))
                {
                    variable_provider.CreateProjectVariable(var.VarName, var.VarType, null);
                }
            }

           
        }

        private void btn_connect_Click(object sender, RoutedEventArgs e)
        {
            ICommunicationCtrl communication_ctrl = new CommunicationCtrl();
            communication_ctrl.InitConnection(mSetupModel.Ip, mSetupModel.Port);
        }
    }

    

}
