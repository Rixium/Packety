using System;
using Packety.Packets;
using Steamworks.Data;

namespace Packety.Consumers
{
    public abstract class PacketConsumer<T> : IPacketConsumer where T : INetworkPacket
    {
        public Type PacketType { get; set; }

        protected PacketConsumer() => PacketType = typeof(T);

        void IPacketConsumer.Consume(Connection connection, INetworkPacket packet) =>
            ConsumePacket(connection, (T) packet);

        public void Consume(INetworkPacket packet) =>
            ConsumePacket(default, (T) packet);
        
        protected abstract void ConsumePacket(Connection connection, T packet);
    }
}