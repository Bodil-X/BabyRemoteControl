using PhoneConnector.Core;
using GalaSoft.MvvmLight;

namespace PhoneConnector.ViewModel
{
    public class PhoneConnectorViewModel : ViewModelBase
    {
        private readonly Server _listenerServer;


        private string _systemMessage;
        public string SystemMessage
        {
            get { return _systemMessage; }
            set
            {
                _systemMessage = value;
                RaisePropertyChanged("SystemMessage");
            }
        }

        private string _receiveMessage;
        public string ReceiveMessage
        {
            get { return _receiveMessage; }
            set
            {
                _receiveMessage = value;
                RaisePropertyChanged("ReceiveMessage");
            }
        }

        private bool _isCanStart = true;
        public bool IsCanStart
        {
            get { return _isCanStart; }
            set
            {
                _isCanStart = value;
                RaisePropertyChanged("IsCanStart");
            }
        }

        private string _connectButtonText = "Start to Listen";
        public string ConnectButtonText
        {
            get { return _connectButtonText; }
            set
            {
                _connectButtonText = value;
                RaisePropertyChanged("ConnectButtonText");
            }
        }

        public PhoneConnectorViewModel()
        {
            _listenerServer = new Server();
        }

        public ActionCommand StartOrStopCommand
        {
            get
            {
                return new ActionCommand(() =>
                                             {
                                                 if (IsCanStart)
                                                 {
                                                     _listenerServer.Start();
                                                     _listenerServer.ReceivedMessageEvent +=
                                                         msg =>
                                                         {
                                                             ReceiveMessage = string.Format("{0}\r\n", msg);
                                                         };
                                                     SystemMessage = "Waiting for phone client to connect.";
                                                     IsCanStart = false;
                                                     ConnectButtonText = "Stop to Listen";
                                                 }
                                                 else
                                                 {
                                                     IsCanStart = true;
                                                     ConnectButtonText = "Start to Listen";
                                                     SystemMessage = "Server has been Closed.";
                                                     _listenerServer.Close();
                                                 }
                                             });
            }
        }
    }
}
