using System;

namespace Packety.Packets
{
    public interface IPacketDefinition
    {
        int PacketTypeId { get; set; }
        Type PacketType { get; set; }
        string Pack(INetworkPacket data);
        INetworkPacket Unpack(string data);
    }
    
}