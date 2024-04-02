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


namespace VariableManager
{
    public class CommunicationCtrl : ICommunicationCtrl
    {
        private const ushort DEFAULTPORT = 7070;
        private const string DEFAULTIP = "192.168.99.1";

        private TcpClient m_client;


        public string ConnectState
        {
            get { return (m_client?.Connected == true) ? "Normal" : "Error"; }
        }

        public void InitConnection(string IP = DEFAULTIP, ushort Port = DEFAULTPORT, bool ForceConnect = false)
        {
            try
            {
                if (m_client != null && ForceConnect == false)
                {
                    if (ConnectState == "Normal") { return; }
                }
                m_client = new TcpClient(DEFAULTIP, DEFAULTPORT);
            }
            catch (ArgumentNullException a)
            {
                Console.WriteLine("ArgumentNullException:{0}", a);
            }
            catch (SocketException ex)
            {
                Console.WriteLine("SocketException:{0}", ex);
            }
        }

        public string ReadData()
        {
            ReadOnlySpan<byte> rtn = ReadUntilTimeout();
            return Encoding.ASCII.GetString(rtn);
        }

        public void WriteData(string Cmd)
        {
            try
            {
                var data = Encoding.GetEncoding(20127).GetBytes(Cmd);
                var stm = m_client.GetStream();
                if (m_client.Connected == false) { return; }
                stm.Write(data, 0, data.Length);
                stm.Flush();
            }
            catch (InvalidOperationException) { }

            catch (IOException ioe) when (ioe.InnerException is SocketException soe)
            {

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

        public ReadOnlySpan<byte> ReadUntilTimeout(TimeSpan? startTimeout = null,
                                                   TimeSpan? readTimeout = null,
                                                   int bufferSize = 8192)
        {
            var writer = new ArrayPoolBufferWriter<byte>();
            int readSize = -1;

            startTimeout = TimeSpan.FromSeconds(10);
            readTimeout = TimeSpan.FromMilliseconds(20);

            try
            {
                var stm = m_client.GetStream();
                if (m_client.Connected == false) { return writer.WrittenSpan; }

                stm.ReadTimeout = (int?)readTimeout?.TotalMilliseconds ?? stm.ReadTimeout;
                // if no data arrives within the start timeout a SocketException will be thrown!
                // ReadTimeout is automatically reset inside the wait bellow after it completes

                WaitForData(stm, startTimeout);

                while (readSize != 0)
                {
                    var buffer = writer.GetSpan(bufferSize);
                    readSize = stm.Read(buffer);
                    writer.Advance(readSize);
                }
            }
            catch (InvalidOperationException) { }
            catch (IOException ioe) when (ioe.InnerException is SocketException soe)
            {
                // ignores read timeout errors since that's what we want: read until closed or no more data available
                //if (soe.SocketErrorCode != SocketError.TimedOut)
                //    throw soe;
            }
            return writer.WrittenSpan;

        }


    }
}
