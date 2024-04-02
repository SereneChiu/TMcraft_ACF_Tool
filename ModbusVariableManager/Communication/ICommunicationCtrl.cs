using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableManager
{
    public interface ICommunicationCtrl
    {
        string ConnectState { get; }
        void InitConnection(string IP, ushort Port, bool ForceConnect = false);
        string ReadData();
        void WriteData(string Cmd);
    }
}
