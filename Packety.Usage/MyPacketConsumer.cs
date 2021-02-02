using System;
using Packety.Consumers;
using Steamworks.Data;

namespace Packety.Usage
{
    public class MyPacketConsumer : PacketConsumer<MyPacket>
    {
        protected override void ConsumePacket(Connection connection, MyPacket packet)
        {
            Console.WriteLine(packet.Name);
        }
    }
}