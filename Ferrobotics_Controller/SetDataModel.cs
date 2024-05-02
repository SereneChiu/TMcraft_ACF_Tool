using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using VariableManager;

namespace Ferrobotics_Controller
{
    public class SetDataModel : INotifyPropertyChanged
    {
        private AcfDevTypeModel mAcfDevTypeModel = new AcfDevTypeModel();

        private bool mEdit_Mode = false;
        private decimal mSetParam1 = 0;
        private decimal mSetParam2 = 0;
        private decimal mSetParam3 = 0;
        private decimal mSetParam4 = 0;
        private string mWriteData = "";
        private string mNodeName = "ACF Control";
        private string mSetParamName_4 = "Payload:";
        private string mSetParamUnit_4 = "Kg";
        private bool mTargetDo = false;
        private bool mDisplay = false;
        private bool mInputValueEditable = false;
        private Visibility mBtnWriteVisible = Visibility.Visible;
        private CollectionView mDevEntries;
        private string mDevEntry = "ACF / ACF-K / ATK";
        private string mDevSubEntry = "ACF / ACF-K / ATK";

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
        public decimal SetParam1 
        { 
            get 
            { 
                return mSetParam1; 
            } 
            set 
            {
                mSetParam1 = value; 
                NotifyPropertyChanged("SetParam1"); 
            } 
        }
        public decimal SetParam2 
        { 
            get 
            {
                return mSetParam2; 
            } 
            set 
            { 
                mSetParam2 = value; 
                NotifyPropertyChanged("SetParam2"); 
            } 
        }
        public decimal SetParam3 
        { 
            get 
            { 
                return mSetParam3; 
            } 
            set 
            { 
                if (value > 10 || value < 0)
                {
                    return;
                }
                mSetParam3 = value; 
                NotifyPropertyChanged("SetParam3"); 
            } 
        }
        public decimal SetParam4 
        { 
            get 
            { 
                return mSetParam4; 
            } 
            set 
            {
                //decimal maximun = Math.Abs(SetParam1) / 10;
                //if (value > maximun)
                //{
                //    return;
                //}
                mSetParam4 = value; 
                NotifyPropertyChanged("SetParam4"); 
            } 
        }
        public string WriteData { get { return mWriteData; } set { mWriteData = value; NotifyPropertyChanged("WriteData"); } }
        public string NodeName { get { return mNodeName; } set { mNodeName = value; NotifyPropertyChanged("NodeName"); } }

        public string SetParamName_4 { get { return mSetParamName_4; } set { mSetParamName_4 = value; NotifyPropertyChanged("SetParamName_4"); } }
        public string SetParamUnit_4 { get { return mSetParamUnit_4; } set { mSetParamUnit_4 = value; NotifyPropertyChanged("SetParamUnit_4"); } }

        public bool TargetDo { get { return mTargetDo; } set { mTargetDo = value; NotifyPropertyChanged("TargetDo"); } }
        public bool Display { get { return mDisplay; } set { mDisplay = value; NotifyPropertyChanged("Display"); } }
        public bool InputValueEditable { get { return mInputValueEditable; } set { mInputValueEditable = value; NotifyPropertyChanged("InputValueEditable"); } }

        public string TitleDesc { get { return mTitleDesc; } set { mTitleDesc = value; NotifyPropertyChanged("TitleDesc"); } }
        public string TargetDesc { get { return mTargetDesc; } set { mTargetDesc = value; NotifyPropertyChanged("TargetDesc"); } }
        public string ZeroDesc { get { return mZeroDesc; } set { mZeroDesc = value; NotifyPropertyChanged("ZeroDesc"); } }
        public string RampDesc { get { return mRampDesc; } set { mRampDesc = value; NotifyPropertyChanged("RampDesc"); } }
        public string PayloadDesc { get { return mPayloadDesc; } set { mPayloadDesc = value; NotifyPropertyChanged("PayloadDesc"); } }
        public string SpeedDesc { get { return mSpeedDesc; } set { mSpeedDesc = value; NotifyPropertyChanged("SpeedDesc"); } }
        public string DetailDesc { get { return mDetailDesc; } set { mDetailDesc = value; NotifyPropertyChanged("DetailDesc"); } }
        public Visibility BtnWriteVisible { get { return mBtnWriteVisible; } set { mBtnWriteVisible = value; NotifyPropertyChanged("BtnWriteVisible"); } }


        public void InitView()
        {
            mDevEntries = new CollectionView(mAcfDevTypeModel.DevEntries);
        }

        

        public CollectionView DevEntries
        {
            get { return mDevEntries; }
        }

        public CollectionView DevSubEntries
        {
            get 
            {
                CollectionView data;
                switch (DevEntry)
                {
                    case "ACF-K": data = new CollectionView(mAcfDevTypeModel.DevSubEntries_Acfk); break;
                    case "ACF / ATK": data = new CollectionView(mAcfDevTypeModel.DevSubEntries_Acf); break;
                    case "AOK-AAK": data = new CollectionView(mAcfDevTypeModel.DevSubEntries_Aok); break;
                    default:
                        DevEntry = "ACF-K";
                        data = new CollectionView(mAcfDevTypeModel.DevSubEntries_Acfk); break;
                }

               
                return data;
            }
        }

        public string DevEntry
        {
            get { return mDevEntry; }
            set
            {
                if (mDevEntry == value) return;
                mDevEntry = value;

                NotifyPropertyChanged("DevEntries");
                NotifyPropertyChanged("DevSubEntries");

                SetParamName_4 = (mDevEntry == "AOK-AAK") ? "Speed:" : "Payload:";
                SetParamUnit_4 = (mDevEntry == "AOK-AAK") ? "RPM" : "Kg";

                NotifyPropertyChanged("SetParamName_4");
                NotifyPropertyChanged("SetParamUnit_4");
            }
        }

        public string DevSubEntry
        {
            get { return mDevSubEntry; }
            set
            {
                mDevSubEntry = value;
                AcfDevType type = mAcfDevTypeModel.DevInfoTable.Keys.First(p => p.TypeName == DevEntry);
                if (type == null) return;

                if (false == mAcfDevTypeModel.DevInfoTable[type].ContainsKey(mDevSubEntry))
                {
                    return;
                }

                SetParam4 = mAcfDevTypeModel.DevInfoTable[type][mDevSubEntry];

                InputValueEditable = (SetParam4 == (decimal)0.0) ? true : false;
                NotifyPropertyChanged("SetParam4");
            }
        }

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
