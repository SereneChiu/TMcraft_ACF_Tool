using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMcraft;
using VariableManager;

namespace Ferrobotics_Setup.Model
{
    public delegate void ModelFunctionCb();

    public class SetupModel : INotifyPropertyChanged
    {
        public TMcraftSetupAPI TMcraftSetupAPI { get; set; } = null;
        public ModelFunctionCb CallbackFunc { get; set; } = null;

        public ushort DO_CTRL_BOX = 16;
        public ushort DO_END_MODULE = 3;


        private static List<string> mVarNameList = new List<string>()
        {
            "ferrobotics_ip"
          , "ferrobotics_port"
          , "ferrobotics_do_type"
          , "ferrobotics_do_channel"
          , "ferrobotics_do_status"
        };

        private Dictionary<string, VariableModel> mVarDict = new Dictionary<string, VariableModel>();


        private ushort mDoCount = 0;
        private ushort mCurSelectDoIdx = 0;
        private ushort mCurSelectDoType = 0;
        private string mIp = "\"192.168.99.1\"";
        private ushort mPort = 7070;
        private string mDevName = "ACF-K ER";
        private string mConnectState = "Unknown";
        private bool mDoStatus = false;
        private bool mEdit_Mode = false;

        public Dictionary<string, VariableModel> VarTable { get { return mVarDict; } }
        public string Ip { get { return mIp; } set { mIp = value; NotifyPropertyChanged("Ip"); } }
        public ushort Port { get { return mPort; } set { mPort = value; NotifyPropertyChanged("Port"); } }
        public string DeviceName { get { return mDevName; } set { mDevName = value; NotifyPropertyChanged("DeviceName"); } }
        public string ConnectState { get { return mConnectState; } set { mConnectState = value; NotifyPropertyChanged("ConnectState"); } }
        public bool DoStatus { get { return mDoStatus; } set { mDoStatus = value; NotifyPropertyChanged("DoStatus"); } }
        public bool Edit_Mode { get { return mEdit_Mode; } set { mEdit_Mode = value; NotifyPropertyChanged("Edit_Mode"); } }

        
        public ushort CurSelectDoIdx 
        { 
            get 
            { 
                return mCurSelectDoIdx; 
            } 
            set 
            { 
                mCurSelectDoIdx = value;
                NotifyPropertyChanged("CurSelectDoIdx");
                NotifyPropertyChanged("CurSelectDoStr");
            }
        }
        public ushort CurSelectDoType
        {
            get
            {
                return mCurSelectDoType;
            }
            set
            {
                mCurSelectDoType = value;
                NotifyPropertyChanged("CurSelectDoType");
                NotifyPropertyChanged("CurSelectDoStr");
            }
        }

        public string CurSelectDoStr
        {
            get
            {
                string type_str = (CurSelectDoType == 0) ? "Control Box" : "End Module";
                return string.Format("{0}: DO {1}", type_str, CurSelectDoIdx.ToString());
            }
        }

        public ushort DoCount 
        { 
            get { return mDoCount; } 
            set 
            { 
                mDoCount = value; 
                if (CallbackFunc != null)
                {
                    CallbackFunc();
                }
                NotifyPropertyChanged("DoCount"); 
            } 
        }

        public void UpdateDictData()
        {
            mVarDict.Clear();
            mVarDict = new Dictionary<string, VariableModel>()
            {
                { mVarNameList[0], new VariableModel(mVarNameList[0], VariableType.String, Ip) }
              , { mVarNameList[1], new VariableModel(mVarNameList[1], VariableType.Integer, Port.ToString()) }
              , { mVarNameList[2], new VariableModel(mVarNameList[2], VariableType.Integer, CurSelectDoType.ToString()) }
              , { mVarNameList[3], new VariableModel(mVarNameList[3], VariableType.Integer, CurSelectDoIdx.ToString()) }
              , { mVarNameList[4], new VariableModel(mVarNameList[4], VariableType.Boolean, DoStatus.ToString()) }
            };
        }



        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
