using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace PhoneConnector.Core
{
    public class Server
    {
        public string OutMessage { get; set; }

        public delegate void ReceivedMessageHandler(string receiveMsg);
        public event ReceivedMessageHandler ReceivedMessageEvent;

        private int _port = 10086;
        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        private Thread _createServerThread;
        private volatile bool _isRunning;
        private TcpListener listener;

        public void Start()
        {
            _createServerThread = new Thread(CreateListener) { Name = "PhoneConnectorCreateThread" };
            _createServerThread.Start();
        }

        public void Close()
        {
            if (_createServerThread.IsAlive && _createServerThread.ThreadState == ThreadState.Running)
            {
                Thread.Sleep(1);
                _isRunning = false;
                listener.Stop();
                _createServerThread.Join();
            }
        }

        private void CreateListener()
        {
            NetInfo netInfo = new NetInfo();
            if (netInfo.IsUsedPort(Port))
                throw new Exception(string.Format("The port number:{0} has been use.", Port));

            listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();
            _isRunning = true;
            try
            {
                while (_isRunning)
                {
                    TcpClient tcpClient = listener.AcceptTcpClient();
                    Thread tcpClientThread = new Thread(ReceivingMessage);
                    tcpClientThread.Start(tcpClient);
                    Thread.Sleep(10);
                }
            }
            catch (SocketException socketEx)
            {
                OutMessage = socketEx.Message;
                OnReceivedMessage(OutMessage);
            }
            finally
            {
                listener.Stop();
            }
        }

        private void ReceivingMessage(object receiveClientObj)
        {
            using (TcpClient receiveClient = receiveClientObj as TcpClient)
            {
                if (receiveClient != null)
                    using (NetworkStream networkStream = receiveClient.GetStream())
                    {
                        do
                        {
                            byte[] receiveBuffer =
                                new byte[receiveClient.ReceiveBufferSize];
                            int readLength = networkStream.Read(receiveBuffer, 0, receiveBuffer.Length);
                            byte[] trimReceiveBuffer = new byte[readLength];
                            Array.Copy(receiveBuffer, trimReceiveBuffer, readLength);
                            OutMessage = Encoding.ASCII.GetString(trimReceiveBuffer);
                            OnReceivedMessage(OutMessage);
                        } while (receiveClient.Connected &&
                                 OutMessage.ToLower() != "over");
                    }
            }
        }

        private void OnReceivedMessage(string messageStr)
        {
            if (ReceivedMessageEvent != null && !string.IsNullOrEmpty(messageStr))
                ReceivedMessageEvent(messageStr);
        }
    }
}
