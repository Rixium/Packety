using System;
using Steamworks;
using Steamworks.Data;

namespace Packety
{
    public class Client : ConnectionManager
    {
        public SteamNetworkManager NetworkManager;

        public override void OnConnected(ConnectionInfo info)
        {
            base.OnConnected(info);

            NetworkManager.NetworkClientListener.OnConnectedToServer(info);
        }

        public override void OnMessage(IntPtr data, int size, long messageNum, long recvTime, int channel)
        {
            base.OnMessage(data, size, messageNum, recvTime, channel);

            NetworkManager.NetworkClientListener.OnMessageReceived(data, size, messageNum, recvTime, channel);
        }

        public override void OnDisconnected(ConnectionInfo info)
        {
            base.OnDisconnected(info);

            NetworkManager.NetworkClientListener.OnDisconnectedFromServer(info);
        }
    }
}