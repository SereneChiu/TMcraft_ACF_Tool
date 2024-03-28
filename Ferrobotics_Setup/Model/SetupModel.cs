using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrobotics_Setup.Model
{
    public delegate void ModelFunctionCb();

    public class SetupModel : INotifyPropertyChanged
    {
        public ushort DO_CTRL_BOX = 16;
        public ushort DO_END_MODULE = 3;

        private ushort mCurSelectDoIdx = 0;
        private ushort mDoCount = 0;
        public ModelFunctionCb CallbackFunc { get; set; } = null;

        public ushort mEnableDoChannel { get; set; } = 0;

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

        



        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
