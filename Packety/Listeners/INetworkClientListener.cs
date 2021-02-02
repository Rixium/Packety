using System;
using Packety.Consumers;
using Steamworks.Data;

namespace Packety.Listeners
{
    public interface INetworkClientListener
    {

        void OnDisconnectedFromServer(ConnectionInfo info);
        void OnMessageReceived(IntPtr data, int size, long messageNum, long recvTime, int channel);
        void OnConnectedToServer(ConnectionInfo info);
        void AddConsumer(IPacketConsumer packetConsumer);

    }
}