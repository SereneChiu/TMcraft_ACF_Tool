using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrobotics_Controller;

namespace Ferrobotics_Toolbar
{

    public class ToolbarViewModel : INotifyPropertyChanged
    {
        private SetDataModel mSetDataModel = new SetDataModel();

        private decimal mGetParam1 = 0;
        private decimal mGetParam2 = 0;
        private decimal mGetParam3 = 0;
        private decimal mGetParam4 = 0;

        private string mIp = "192.168.99.1";
        private ushort mPort = 7070;
        private string mConnectState = "Unknown";
        private string mWriteData = "ferbak1040 0 0 0 0 0";


        public SetDataModel SetDataModel { get { return mSetDataModel; } }
        public bool Edit_Mode { get { return mSetDataModel.Edit_Mode; } set { mSetDataModel.Edit_Mode = value; NotifyPropertyChanged("Edit_Mode"); } }

        public string IP { get { return mIp; } set { mIp = value; NotifyPropertyChanged("IP"); } }
        public ushort Port { get { return mPort; } set { mPort = value; NotifyPropertyChanged("Port"); } }
        public string ConnectState { get { return mConnectState; } set { mConnectState = value; NotifyPropertyChanged("ConnectState"); } }

        public decimal GetParam1 { get { return mGetParam1; } set { mGetParam1 = value; NotifyPropertyChanged("GetParam1"); } }
        public decimal GetParam2 { get { return mGetParam2; } set { mGetParam2 = value; NotifyPropertyChanged("GetParam2"); } }
        public decimal GetParam3 { get { return mGetParam3; } set { mGetParam3 = value; NotifyPropertyChanged("GetParam3"); } }
        public decimal GetParam4 { get { return mGetParam4; } set { mGetParam4 = value; NotifyPropertyChanged("GetParam4"); } }

        public decimal SetParam1 { get { return mSetDataModel.SetParam1; } }
        public decimal SetParam2 { get { return mSetDataModel.SetParam2; } }
        public decimal SetParam3 { get { return mSetDataModel.SetParam3; } }
        public decimal SetParam4 { get { return mSetDataModel.SetParam4; } }
        public string WriteData { get { return mSetDataModel.WriteData; } }


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
