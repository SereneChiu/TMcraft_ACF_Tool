using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ferrobotics_Controller
{
    public class SetDataModel : INotifyPropertyChanged
    {
        private bool mEdit_Mode = false;
        private decimal mSetParam1 = 0;
        private decimal mSetParam2 = 0;
        private decimal mSetParam3 = 0;
        private decimal mSetParam4 = 0;
        private string mWriteData = "";
        private string mNodeName = "ACF Control";
        private bool mTargetDo = false;
        private bool mDisplay = false;
        private Visibility mBtnWriteVisible = Visibility.Visible;

        private string mTitleDesc = "Operation Parameters Information:";
        private string mTargetDesc  = "f__target:  A positive target force means the ACF will be pushing, while a negative target force means pulling.";
        private string mZeroDesc    = "f__zero:   This value is necessary for the Force ramp, for details see the chapter Force ramp functionality.";
        private string mRampDesc    = "t__ramp:  This value is necessary for the Force ramp, for details see the chapter Force ramp functionality.";
        private string mPayloadDesc = "Payload: Determine the weight of the tool, this value is the payload.";
        private string mSpeedDesc   = "Speed:   You enter the speed in 10rpm increments of the maximum speed (10 000 rpm).\r\nHowever, note the maximum permissible speed of the abrasive!";
        private string mDetailDesc = "Force ramp functionality: \r\nThe force ramp allows making contact with a surface smoothly in order to avoid abrupt force onto the workpiece. " +
                                 "\r\nSet a zero force (set_f_zero) that will be applied as long as the ACF does not detect contact." +
                                 "\r\nSet the time of the force ramp with set_t_ramp. Deactivate the force ramp functionality by setting set_t_ramp to 0.";

        public bool Edit_Mode { get { return mEdit_Mode; } set { mEdit_Mode = value; NotifyPropertyChanged("Edit_Mode"); } } 
        public decimal SetParam1 { get { return mSetParam1; } set { mSetParam1 = value; NotifyPropertyChanged("SetParam1"); } }
        public decimal SetParam2 { get { return mSetParam2; } set { mSetParam2 = value; NotifyPropertyChanged("SetParam2"); } }
        public decimal SetParam3 { get { return mSetParam3; } set { mSetParam3 = value; NotifyPropertyChanged("SetParam3"); } }
        public decimal SetParam4 { get { return mSetParam4; } set { mSetParam4 = value; NotifyPropertyChanged("SetParam4"); } }
        public string WriteData { get { return mWriteData; } set { mWriteData = value; NotifyPropertyChanged("WriteData"); } }
        public string NodeName { get { return mNodeName; } set { mNodeName = value; NotifyPropertyChanged("NodeName"); } }

        public bool TargetDo { get { return mTargetDo; } set { mTargetDo = value; NotifyPropertyChanged("TargetDo"); } }
        public bool Display { get { return mDisplay; } set { mDisplay = value; NotifyPropertyChanged("Display"); } }

        public string TitleDesc { get { return mTitleDesc; } set { mTitleDesc = value; NotifyPropertyChanged("TitleDesc"); } }
        public string TargetDesc { get { return mTargetDesc; } set { mTargetDesc = value; NotifyPropertyChanged("TargetDesc"); } }
        public string ZeroDesc { get { return mZeroDesc; } set { mZeroDesc = value; NotifyPropertyChanged("ZeroDesc"); } }
        public string RampDesc { get { return mRampDesc; } set { mRampDesc = value; NotifyPropertyChanged("RampDesc"); } }
        public string PayloadDesc { get { return mPayloadDesc; } set { mPayloadDesc = value; NotifyPropertyChanged("PayloadDesc"); } }
        public string SpeedDesc { get { return mSpeedDesc; } set { mSpeedDesc = value; NotifyPropertyChanged("SpeedDesc"); } }
        public string DetailDesc { get { return mDetailDesc; } set { mDetailDesc = value; NotifyPropertyChanged("DetailDesc"); } }
        public Visibility BtnWriteVisible { get { return mBtnWriteVisible; } set { mBtnWriteVisible = value; NotifyPropertyChanged("BtnWriteVisible"); } }

        public void MergeStringData()
        {
            mWriteData = string.Format("ferbak1040 {0} {1} {2} {3} 0"
                                             , SetParam1.ToString()
                                             , SetParam2.ToString()
                                             , SetParam3.ToString()
                                             , SetParam4.ToString());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
