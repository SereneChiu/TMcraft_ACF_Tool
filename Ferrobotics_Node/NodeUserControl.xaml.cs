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

        public NodeUserControl()
        {
            InitializeComponent();

            mSetDataUserControl = new SetDataUserControl(mNodeViewModel.SetDataModel);
            panel_ctrl.Content = mSetDataUserControl;
        }

        public void InitializeNode(TMcraftNodeAPI NodeAPI)
        {

        }

        public void InscribeScript(ScriptWriteProvider WriteProvider)
        {

        }
    }

}
