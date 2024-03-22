using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Timers;
using System.Threading;
using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;


namespace ModbusVariableManager
{
    public interface ICommunicationCtrl
    {
        string ConnectState { get; }
        void InitConnection();
        string ReadData();
        void WriteData(string Cmd);
    }


    public class CommunicationCtrl : ICommunicationCtrl
    {
        private bool mIsConnectServer = false;

        private const string DEFAULTPORT = "7070";

        private const int RCVBUFFERSIZE = 256; // buffer size for receive buffer
        private const string DEFAULTIP = "192.168.99.1";

        private TcpClient m_client;

        public string ConnectState
        {
            get { return (mIsConnectServer == false) ? "Error" : "Normal"; }
        }

        public void InitConnection()
        {
            try
            {
                m_client = new TcpClient(DEFAULTIP, 7070);
            }
            catch (ArgumentNullException a)
            {
                Console.WriteLine("ArgumentNullException:{0}", a);
            }
            catch (SocketException ex)
            {
                Console.WriteLine("SocketException:{0}", ex);
            }

            mIsConnectServer = true;
        }

        public string ReadData()
        {
            ReadOnlySpan<byte> rtn = ReadUntilTimeout(m_client.GetStream());
            return Encoding.ASCII.GetString(rtn);
        }

        public void WriteData(string Cmd)
        {
            var data = Encoding.GetEncoding(20127).GetBytes(Cmd);
            var stm = m_client.GetStream();
            stm.Write(data, 0, data.Length);
            stm.Flush();
        }
        
        private void CheckConnection()
        {
            if (m_client.Connected == false)
            {
                InitConnection();
            }
        }

        public void WaitForData(NetworkStream stream, TimeSpan? timeout)
        {
            if (timeout is null) return;
            int originalReadTimeout = stream.ReadTimeout;
            stream.ReadTimeout = (int)timeout.Value.TotalMilliseconds;
            stream.Read(Array.Empty<byte>(), 0, 0);
            stream.ReadTimeout = originalReadTimeout;
        }

        public ReadOnlySpan<byte> ReadUntilTimeout(NetworkStream stream,
            TimeSpan? startTimeout = null,
            TimeSpan? readTimeout = null,
            int bufferSize = 8192)
        {
            startTimeout = TimeSpan.FromSeconds(10);
            readTimeout = TimeSpan.FromMilliseconds(20);

            stream.ReadTimeout = (int?)readTimeout?.TotalMilliseconds ?? stream.ReadTimeout;
            // if no data arrives within the start timeout a SocketException will be thrown!
            // ReadTimeout is automatically reset inside the wait bellow after it completes

            WaitForData(stream, startTimeout);
            var writer = new ArrayPoolBufferWriter<byte>();
            int readSize = -1;
            try
            {
                while (readSize != 0)
                {
                    var buffer = writer.GetSpan(bufferSize);
                    readSize = stream.Read(buffer);
                    writer.Advance(readSize);
                }
            }
            catch (IOException ioe) when (ioe.InnerException is SocketException soe)
            {
                // ignores read timeout errors since that's what we want: read until closed or no more data available
                if (soe.SocketErrorCode != SocketError.TimedOut)
                    throw soe;
            }
            return writer.WrittenSpan;
        }


    }
}
