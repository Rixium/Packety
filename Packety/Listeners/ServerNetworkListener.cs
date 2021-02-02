using System;
using System.Collections.Generic;
using Packety.Consumers;
using Packety.Packagers;
using Steamworks.Data;

namespace Packety.Listeners
{
    public class ServerNetworkListener : INetworkServerListener
    {
        private readonly INetworkManager _networkManager;
        private readonly INetworkMessagePackager _messagePackager;

        private readonly Dictionary<Type, IPacketConsumer> _packetConsumers 
            = new Dictionary<Type, IPacketConsumer>();

        public ServerNetworkListener(INetworkManager networkManager)
        {
            _networkManager = networkManager;
            _messagePackager = networkManager.MessagePackager;
            _networkManager.SetServerNetworkListener(this);
        }
        
        public void OnNewConnection(Connection connection, ConnectionInfo info)
        {
            
        }

        public void OnConnectionLeft(Connection connection, ConnectionInfo info)
        {
            
        }

        public void OnMessageReceived(Connection connection, NetIdentity identity, IntPtr data, int size, long messageNum,
            long recvTime, int channel)
        {
            var received = _messagePackager.Unpack(data, size);
            var receivedType = received.GetType();
            
            if (!_packetConsumers.ContainsKey(receivedType)) 
                return;
            
            var consumer = _packetConsumers[receivedType];
            consumer.Consume(connection, received);
            
            _networkManager.RelayMessage(data, size, connection);
        }

        public void OnConnectionChanged(Connection connection, ConnectionInfo info)
        {
            
        }
        
        public void AddConsumer(IPacketConsumer packetConsumer) => 
            _packetConsumers.Add(packetConsumer.PacketType, packetConsumer);
        
    }
}