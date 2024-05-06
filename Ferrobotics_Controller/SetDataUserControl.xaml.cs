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
using VariableManager;

namespace Ferrobotics_Controller
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class SetDataUserControl : UserControl
    {
        private SetDataModel mSetDataModel = null;

        public SetDataUserControl(SetDataModel DataModel)
        {
            InitializeComponent();

            mSetDataModel = DataModel;
            DataContext = mSetDataModel;

            mSetDataModel.InitView();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mSetDataModel.MergeStringData();
        }

        public void UpdateEditMode()
        {
            mSetDataModel.Edit_Mode = !mSetDataModel.Edit_Mode;
        }
    }

}
