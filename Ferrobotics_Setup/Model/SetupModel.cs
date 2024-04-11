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
        public TMcraftSetupAPI TMcraftSetupAPI 
        { 
            get
            {
                return mTMcraftSetupAPI;
            }
            set
            {
                mTMcraftSetupAPI = value;
                mProjectVarCtrl.UpdateFunctionPtr(mTMcraftSetupAPI.VariableProvider.IsProjectVariableExist
                                                 , mTMcraftSetupAPI.VariableProvider.CreateProjectVariable
                                                 , mTMcraftSetupAPI.VariableProvider.ChangeProjectVariableValue
                                                 , mTMcraftSetupAPI.VariableProvider.GetProjectVariableList);
            }
        }
        public ModelFunctionCb CallbackFunc { get; set; } = null;

        public ushort DO_CTRL_BOX = 16;
        public ushort DO_END_MODULE = 3;

        private IProjectVariableCtrl mProjectVarCtrl = new ProjectVariableCtrl();

        private TMcraftSetupAPI mTMcraftSetupAPI = null;
        private ushort mDoCount = 0;
        private ushort mCurSelectDoIdx = 0;
        private ushort mCurSelectDoType = 0;
        private string mIp = "192.168.99.1";
        private ushort mPort = 7070;
        private string mDevName = "ACF-K ER";
        private string mConnectState = "Unknown";
        private bool mDoStatus = false;
        private bool mEdit_Mode = false;

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
                if (DoStatus == false) { return "Unknown"; }

                string type_str = (CurSelectDoType == 0) ? "ControlBox" : "EndModule";
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

        public void UpdateDictData(bool UpdateDo)
        {
            if (UpdateDo == true)
            {
                string do_source = (CurSelectDoType == 0) ? "ControlBox" : "EndModule";
                mProjectVarCtrl.VariableModel.UpdateDictData(mProjectVarCtrl.VariableModel.VarTable.ElementAt(2).Key, string.Format("\"{0}\"", do_source));
            }

            mProjectVarCtrl.VariableModel.UpdateDictData(mProjectVarCtrl.VariableModel.VarTable.ElementAt(0).Key, string.Format("\"{0}\"", Ip));
            mProjectVarCtrl.VariableModel.UpdateDictData(mProjectVarCtrl.VariableModel.VarTable.ElementAt(1).Key, Port.ToString());
            mProjectVarCtrl.VariableModel.UpdateDictData(mProjectVarCtrl.VariableModel.VarTable.ElementAt(3).Key, CurSelectDoIdx.ToString());
            mProjectVarCtrl.VariableModel.UpdateDictData(mProjectVarCtrl.VariableModel.VarTable.ElementAt(4).Key, DoStatus.ToString());
        }
        public bool UpdateProjectVariableValueFromData()
        {
            bool rtn_state = false;
            mProjectVarCtrl.UpdateProjectVariableFromData(ref rtn_state);
            return rtn_state;
        }

        public bool GetProjectVariable()
        {
            bool rtn_result = false;
            rtn_result = mProjectVarCtrl.UpdateDataFromProjectVariable();
            if (rtn_result == false) { return rtn_result; }

            ushort out_port = 0;

            this.Ip = mProjectVarCtrl.VariableModel.VarTable["ferrobotics_ip"].VarValue.Replace("\"","");
            if (true == UInt16.TryParse(mProjectVarCtrl.VariableModel.VarTable["ferrobotics_port"].VarValue, out out_port))
            {
                this.Port = out_port;
            }

            ushort out_do_index = 0;
            if (true == UInt16.TryParse(mProjectVarCtrl.VariableModel.VarTable["ferrobotics_do_channel"].VarValue, out out_do_index))
            {
                this.CurSelectDoIdx = out_do_index;
            }

            this.CurSelectDoType = (mProjectVarCtrl.VariableModel.VarTable["ferrobotics_do_type"].VarValue.Replace("\"", "") == "ControlBox") ? (ushort)0 : (ushort)1;
            this.DoStatus = (mProjectVarCtrl.VariableModel.VarTable["ferrobotics_do_status"].VarValue == "True") ? true : false;
            return rtn_result;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
