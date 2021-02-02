using System;
using Packety.Packets;
using Steamworks.Data;

namespace Packety.Consumers
{
    public interface IPacketConsumer
    {
        void Consume(Connection connection, INetworkPacket packet);
        void Consume(INetworkPacket packet);
        Type PacketType { get; set; }
    }
}