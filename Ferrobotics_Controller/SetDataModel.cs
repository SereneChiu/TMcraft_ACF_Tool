using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private bool mTargetDo = false;
        public bool Edit_Mode { get { return mEdit_Mode; } set { mEdit_Mode = value; NotifyPropertyChanged("Edit_Mode"); } } 
        public decimal SetParam1 { get { return mSetParam1; } set { mSetParam1 = value; NotifyPropertyChanged("SetParam1"); } }
        public decimal SetParam2 { get { return mSetParam2; } set { mSetParam2 = value; NotifyPropertyChanged("SetParam2"); } }
        public decimal SetParam3 { get { return mSetParam3; } set { mSetParam3 = value; NotifyPropertyChanged("SetParam3"); } }
        public decimal SetParam4 { get { return mSetParam4; } set { mSetParam4 = value; NotifyPropertyChanged("SetParam4"); } }
        public string WriteData { get { return mWriteData; } set { mWriteData = value; NotifyPropertyChanged("WriteData"); } }
        public bool TargetDo { get { return mTargetDo; } set { mTargetDo = value; NotifyPropertyChanged("TargetDo"); } }

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
