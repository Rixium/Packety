using System;
using System.Collections.Generic;
using System.Text;
using Packety.Packets;

namespace Packety.Packagers
{
    public class NetworkMessagePackager : INetworkMessagePackager
    {
        private static int _packetTypeCounter;
        private readonly Dictionary<int, IPacketDefinition> _packetDefinitions;
        private readonly Dictionary<Type, int> _typeIdDictionary;

        public NetworkMessagePackager()
        {
            _packetTypeCounter = 0;
            _packetDefinitions = new Dictionary<int, IPacketDefinition>();
            _typeIdDictionary = new Dictionary<Type, int>();
        }

        public IPacketDefinition AddDefinition<T>() where T : INetworkPacket, new()
        {
            var packetDefinition = new PacketDefinition<T>()
            {
                PacketTypeId = _packetTypeCounter++,
                PacketType = typeof(T)
            };

            _packetDefinitions.Add(packetDefinition.PacketTypeId, packetDefinition);
            _typeIdDictionary.Add(typeof(T), packetDefinition.PacketTypeId);

            return packetDefinition;
        }

        public byte[] Package<T>(T value) where T : INetworkPacket
        {
            var packetTypeId = _typeIdDictionary[value.GetType()];
            var packetDefinition = GetPacketDefinition(packetTypeId);
            var asString = packetDefinition.Pack(value);
            return Encoding.UTF8.GetBytes($"{packetDefinition.PacketTypeId}:{asString}");
        }

        public unsafe INetworkPacket Unpack(IntPtr data, int size)
        {
            var output = Encoding.UTF8.GetString((byte*) data, size);

            if (output == null)
                throw new Exception("Could not convert data to string.");

            var split = output.Split(new[] {':'}, 2);
            var packetTypeId = int.Parse(split[0]);
            var packetDefinition = GetPacketDefinition(packetTypeId);
            return packetDefinition.Unpack(split[1]);
        }

        public INetworkPacket Unpack(byte[] data)
        {
            var output = Encoding.UTF8.GetString(data);

            if (output == null)
                throw new Exception("Could not convert data to string.");

            var split = output.Split(new[] {':'}, 2);
            var packetTypeId = int.Parse(split[0]);
            var packetDefinition = GetPacketDefinition(packetTypeId);
            return packetDefinition.Unpack(split[1]);
        }

        /// <summary>
        /// Retrieves the packet definition from the dictionary depending on the packet type id that has been passed.
        /// </summary>
        /// <param name="packetTypeId">The packet type Id of the expected packet definition.</param>
        /// <returns>The packet definition for the given packet type id.</returns>
        public IPacketDefinition GetPacketDefinition(int packetTypeId) => _packetDefinitions[packetTypeId];
    }
}