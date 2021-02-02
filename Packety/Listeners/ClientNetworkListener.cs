using System;
using System.Collections.Generic;
using Packety.Consumers;
using Packety.Packagers;
using Steamworks.Data;

namespace Packety.Listeners
{
    public class ClientNetworkListener : INetworkClientListener
    {
        private readonly INetworkMessagePackager _messagePackager;

        private readonly Dictionary<Type, IPacketConsumer> _packetConsumers =
            new Dictionary<Type, IPacketConsumer>();

        public ClientNetworkListener(INetworkManager networkManager)
        {
            _messagePackager = networkManager.MessagePackager;
            networkManager.SetClientNetworkListener(this);
        }

        public void OnDisconnectedFromServer(ConnectionInfo info)
        {
        }

        public void OnMessageReceived(IntPtr data, int size, long messageNum, long recvTime, int channel)
        {
            var received = _messagePackager.Unpack(data, size);
            var receivedType = received.GetType();

            if (!_packetConsumers.ContainsKey(receivedType))
                return;

            var consumer = _packetConsumers[receivedType];
            consumer.Consume(received);
        }

        public void OnConnectedToServer(ConnectionInfo info)
        {
        }

        public void AddConsumer(IPacketConsumer packetConsumer) =>
            _packetConsumers.Add(packetConsumer.PacketType, packetConsumer);
    }
}