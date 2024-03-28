using Ferrobotics_Setup.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
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

namespace Ferrobotics_Setup
{
    /// <summary>
    /// Interaction logic for DigitalInputSelectDialog.xaml
    /// </summary>
    /// 
    public partial class DigitalOutputSelectDialog : UserControl
    {
        private SetupModel mSetupModel = new SetupModel();
        private List<RadioButton> mChbList = new List<RadioButton>();

        public DigitalOutputSelectDialog()
        {
            DataContext = mSetupModel;
            mSetupModel.CallbackFunc = UpdateDataFromUserDefine;
            InitializeComponent();
            UpdateDoChannelFromCombobox();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDoChannelFromCombobox();
        }

        private void UpdateDoChannelFromCombobox()
        {
            if (cb_do_type.SelectedIndex == 1)
            {
                mSetupModel.DoCount = mSetupModel.DO_END_MODULE;
                return;
            }

            mSetupModel.DoCount = mSetupModel.DO_CTRL_BOX;
        }
        
        
        private void UpdateDataFromUserDefine()
        {
            UpdateDo(mSetupModel.DoCount);
        }


        private void UpdateDo(ushort DoCount)
        {
            if (page_do == null) { return; }

            mChbList.Clear();
            page_do.Children.Clear();

            int x_offset = 0;
            int y_offset = 0;
            for (ushort idx = 0; idx < DoCount; idx++)
            {
                if (idx % 8 == 0)
                {
                    y_offset = 0;
                    x_offset++;
                }
                RadioButton btn = new RadioButton();
                btn.Style = (Style)this.Resources["chkBullet"];
                btn.IsChecked = false;
                btn.Tag = "On";
                btn.Width = 90;
                btn.Height = 30;
                btn.Margin = new Thickness(10 + (btn.Width * y_offset), (10 + (btn.Height * x_offset)), 0, 0);
                btn.Checked += Btn_Checked;
                btn.Content = (idx + 1).ToString();
                page_do.Children.Add(btn);
                mChbList.Add(btn);
                y_offset++;
            }

        }

        private void Btn_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            ushort rtn_channel = 0;
            if (false == GetEnableDoChannel(ref rtn_channel)) { return; }
        }


        private bool GetEnableDoChannel(ref ushort RtnChannel)
        {
            ushort idx = 0;
            foreach (RadioButton chb in mChbList)
            {
                if (chb.IsChecked == true)
                {
                    RtnChannel = idx;
                    return true;
                }
                idx++;
            }
            return false;
        }

        private void btn_test_output_Click(object sender, RoutedEventArgs e)
        {
            ushort rtn_channel = 0;
            if (false == GetEnableDoChannel(ref rtn_channel)) { return; }

        }
    }
}
