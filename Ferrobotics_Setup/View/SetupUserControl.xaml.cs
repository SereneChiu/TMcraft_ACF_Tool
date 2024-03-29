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
        private DigitalOutputSelectDialog mDo_Editor = null;


        private SetupModel mSetupModel = new SetupModel();
   
        
        public SetupUserControl()
        {
            DataContext = mSetupModel;
            InitializeComponent();
            mSetupModel.UpdateDictData();
            mDo_Editor = new DigitalOutputSelectDialog(mSetupModel);
            AddHandler(Keyboard.KeyDownEvent, (KeyEventHandler)HandleKeyDownEvent);

        }

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab && (Keyboard.Modifiers & (ModifierKeys.Control)) == (ModifierKeys.Control))
            {
                mSetupModel.Edit_Mode = !mSetupModel.Edit_Mode;
            }
        }


        public void InitializeSetup(TMcraftSetupAPI TMcraftSetupAPI)
        {
            mSetupModel.TMcraftSetupAPI = TMcraftSetupAPI;
        }

       

        private void btn_browse_Click(object sender, RoutedEventArgs e)
        {
            Window win_tool = new Window();
            win_tool.Width = 800;
            win_tool.Height = 400;
            win_tool.Content = mDo_Editor;
            win_tool.Title = "Digital Output Select Dialog";
            win_tool.WindowStyle = WindowStyle.ToolWindow;
            win_tool.ResizeMode = ResizeMode.NoResize;
            win_tool.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win_tool.Show();
           
        }

        
        private void btn_apply_Click(object sender, RoutedEventArgs e)
        {
            bool ans = false;

            if (mDo_Editor.SaveData == true)
            {
                mSetupModel.UpdateDictData();
            }

            if (mSetupModel.TMcraftSetupAPI == null) { return; }

            SetupVariableProvider variable_provider = mSetupModel.TMcraftSetupAPI.VariableProvider;
            SetupScriptWriteProvider script_write_provider = mSetupModel.TMcraftSetupAPI.ScriptWriteProvider;

            List<string[]> value_list = new List<string[]>();
            foreach (VariableModel var in mSetupModel.VarTable.Values)
            {
                if (false == variable_provider.IsProjectVariableExist(var.VarName))
                {
                    variable_provider.CreateProjectVariable(var.VarName, var.VarType, var.VarValue);
                    value_list.Add(new string[] { var.VarName, var.VarValue });
                    ans = true;
                }
            }
            if (ans == false) { return; }

            variable_provider.ChangeProjectVariableValue(value_list);

            script_write_provider.ClearBuffer();
            script_write_provider.AppendScriptToBuffer("Display(\"Initialization Complete\")");
            Thread.Sleep(50);
            script_write_provider.SaveBufferAsScript();

            MessageBox.Show("Set successfully!");
        }

        private void btn_connect_Click(object sender, RoutedEventArgs e)
        {
            ICommunicationCtrl communication_ctrl = new CommunicationCtrl();
            communication_ctrl.InitConnection(mSetupModel.Ip, mSetupModel.Port);
        }
    }

    

}
