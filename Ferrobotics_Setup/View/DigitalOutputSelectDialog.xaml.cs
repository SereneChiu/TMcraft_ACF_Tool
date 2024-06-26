﻿using Ferrobotics_Setup.Model;
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
using TMcraft;

namespace Ferrobotics_Setup
{
    /// <summary>
    /// Interaction logic for DigitalInputSelectDialog.xaml
    /// </summary>
    /// 
    public partial class DigitalOutputSelectDialog : UserControl
    {
        private readonly SetupModel mSetupModel = null;
        private List<RadioButton> mChbList = new List<RadioButton>();
        
        public bool SaveData { get; private set; }


        public DigitalOutputSelectDialog(SetupModel SetupModel)
        {
            mSetupModel = SetupModel;
            DataContext = mSetupModel;
            mSetupModel.CallbackFunc = UpdateDataFromUserDefine;

            InitializeComponent();

            AddHandler(Keyboard.KeyDownEvent, (KeyEventHandler)HandleKeyDownEvent);
        }

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab && (Keyboard.Modifiers & (ModifierKeys.Control)) == (ModifierKeys.Control))
            {
                mSetupModel.Edit_Mode = !mSetupModel.Edit_Mode;
            }
        }

        public void UpdateView()
        {
            if (mSetupModel.CurSelectDoType >= cb_do_type.Items.Count)
            {
                mSetupModel.CurSelectDoType = 0;
            }

            cb_do_type.SelectedIndex = mSetupModel.CurSelectDoType;
            UpdateDoChannelFromCombobox();

            if ((mSetupModel.CurSelectDoIdx >= mChbList.Count) || (mChbList.Count == 0)) 
            { 
                cb_do_type.SelectionChanged += Cb_do_type_SelectionChanged;
                return; 
            }

            mChbList[mSetupModel.CurSelectDoIdx].IsChecked = true;
            mChbList[mSetupModel.CurSelectDoIdx].Tag = "ON";

            cb_do_type.SelectionChanged += Cb_do_type_SelectionChanged;
        }

        private void Cb_do_type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mSetupModel.CurSelectDoType = (ushort)cb_do_type.SelectedIndex;
            UpdateDoChannelFromCombobox();
            UpdateDataFromUserDefine();

        }
        private void UpdateDoChannelFromCombobox()
        {
            if (cb_do_type.SelectedIndex == 1)
            {
                mSetupModel.DoCount = mSetupModel.DO_END_MODULE;
                return;
            }

            cb_do_type.SelectedIndex = 0;
            mSetupModel.DoCount = mSetupModel.DO_CTRL_BOX;
        }
        
        
        private void UpdateDataFromUserDefine()
        {
            InitDoLeds(mSetupModel.DoCount);
        }


        private void InitDoLeds(ushort DoCount)
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
                btn.Tag = "OFF";
                btn.Width = 90;
                btn.Height = 30;
                btn.Margin = new Thickness(10 + (btn.Width * y_offset), (10 + (btn.Height * x_offset)), 0, 0);
                btn.Content = idx.ToString();
                page_do.Children.Add(btn);
                btn.Click += Btn_Click;
                mChbList.Add(btn);
                y_offset++;
            }

        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            RadioButton btn = sender as RadioButton;
            if (btn == null) { return; }

            if (btn.IsChecked == true)
            {
                if (btn.Tag.ToString() == "OFF")
                {
                    foreach (var item in mChbList)
                    {
                        item.Tag = "OFF";
                    }

                    btn.Tag = "ON";
                    return;
                }
                btn.IsChecked = false;
                btn.Tag = "OFF";
            }

        }

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            ushort rtn_channel = 0;
            mSetupModel.DoStatus = GetEnableDoChannel(ref rtn_channel);

            if (true == mSetupModel.DoStatus) 
            {
                mSetupModel.CurSelectDoIdx = rtn_channel;
            }
            else
            {
                mSetupModel.CurSelectDoIdx = 0;
                mSetupModel.CurSelectDoType = 0;
            }
            SaveData = true;

            if (false == (this.Parent is Window)) { return; }
            ((Window)(this.Parent as Window)).Close();
        }


        private bool GetEnableDoChannel(ref ushort RtnChannel)
        {
            ushort idx = 0;
            foreach (RadioButton chb in mChbList)
            {
                if (chb.IsChecked == true)
                {
                    RtnChannel = idx;
                    mSetupModel.CurSelectDoIdx = idx;
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

            if (mSetupModel.TMcraftSetupAPI == null) { return; }

            bool target_status = (btn_test_output.Content == "Test Output") ? true : false;

            IO_TYPE cur_io_type = (mSetupModel.CurSelectDoType == 1) ? IO_TYPE.END_MODULE : IO_TYPE.CONTROL_BOX;
            uint rtn_error = mSetupModel.TMcraftSetupAPI.IOProvider.WriteDigitOutput(cur_io_type
                                                                                   , 0
                                                                                   , mSetupModel.CurSelectDoIdx
                                                                                   , target_status);

            if (rtn_error != 0) { MessageBox.Show("Set DO value failed!"); return; }
            btn_test_output.Content = (target_status == true) ? "Stop Test" : "Test Output";
        }
        
    }
}
