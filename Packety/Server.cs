using System;
using System.Linq;
using Steamworks;
using Steamworks.Data;

namespace Packety
{
    public class Server : SocketManager
    {
        public SteamNetworkManager NetworkManager { get; set; }

        public override void OnConnected(Connection connection, ConnectionInfo info)
        {
            base.OnConnected(connection, info);

            NetworkManager.NetworkServerListener.OnNewConnection(connection, info);
        }

        public override void OnDisconnected(Connection connection, ConnectionInfo info)
        {
            base.OnDisconnected(connection, info);

            NetworkManager.NetworkServerListener.OnConnectionLeft(connection, info);
        }

        public override void OnMessage(Connection connection, NetIdentity identity, IntPtr data, int size,
            long messageNum, long recvTime,
            int channel)
        {
            base.OnMessage(connection, identity, data, size, messageNum, recvTime, channel);

            foreach (var connected in Connected.Where(connected => connected != connection))
            {
                connected.SendMessage(data, size);
            }

            NetworkManager.NetworkServerListener.OnMessageReceived(connection, identity, data, size, messageNum,
                recvTime, channel);
        }

        public override void OnConnectionChanged(Connection connection, ConnectionInfo info)
        {
            base.OnConnectionChanged(connection, info);

            NetworkManager.NetworkServerListener.OnConnectionChanged(connection, info);
        }
    }
}