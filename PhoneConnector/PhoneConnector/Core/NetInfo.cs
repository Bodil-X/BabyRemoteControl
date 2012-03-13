using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace PhoneConnector.Core
{
    public class NetInfo
    {
        private IList<IPEndPoint> _usedIPEndPoints; 
        public IList<IPEndPoint> UsedIPEndPoints
        {
            get
            {
                if (_usedIPEndPoints == null || _usedIPEndPoints.Count == 0) RefreshUsedIPEndPoints();
                return _usedIPEndPoints;
            }
        }

        public void RefreshUsedIPEndPoints()
        {
            _usedIPEndPoints = new List<IPEndPoint>();
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPointTCP = ipGlobalProperties.GetActiveTcpListeners();
            IPEndPoint[] ipEndPointUDP = ipGlobalProperties.GetActiveUdpListeners();
            TcpConnectionInformation[] tcpConnectionInformation = ipGlobalProperties.GetActiveTcpConnections();
            foreach (IPEndPoint iep in ipEndPointTCP) _usedIPEndPoints.Add(iep);
            foreach (IPEndPoint iep in ipEndPointUDP) _usedIPEndPoints.Add(iep);
            foreach (TcpConnectionInformation tci in tcpConnectionInformation) _usedIPEndPoints.Add(tci.LocalEndPoint);
        }

        public bool IsUsedPort(int port)
        {
            return UsedIPEndPoints.Any(p => p.Port == port);
        }
    }
}
