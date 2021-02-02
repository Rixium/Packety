## Packety

### About
Packety is a network packet manager for Steamworks.NET.  
It makes sending, and transmitting network packets very dependency lite.

## Usage

### Packet Definitions
Packet definitions should be added to an instance of NetworkMessagePackager, which will allow for different objects to be
serialized and deserialized to network packets.

#### Usage

`var networkMessagePackager = new NetworkMessagePackager();`  
`networkMessagePackager.AddDefinition<SomeModel>();`

The instance of NetworkMessagePackager can then be used by calling the Package method with a defined type:
`networkMessagePackager.Package(instanceOfSomeModel);`

### NetworkManager
A basic implementation of the INetworkManager interface has been provided with this library.  
The SteamNetworkManager will help for fast set-up of server-client networking.

## Usage

An instance of a INetworkManager implementation must be defined and then used for managing the retrieval and sending of
network packets.

`var networkManager = new SteamNetworkManager(networkMessagePackager);`

It must then be updated to retrieve all network packets using:

`networkManager.Update();`

## PacketConsumer

The base class PacketConsumer must be inherited from in order to direct packets.
For each packet registered with the NetworkMessagePackager, a corresponding PacketConsumer can be created
to separate logic from other areas of the code-base.

## Usage

`public class MyPacketConsumer : PacketConsumer<SomeModel> {`  
`       protected override void ConsumePacket(Connection connection, SomeModel packet) {`  
`// Some logic here for consuming the packet.`  
`}`  
`}`


## NetworkClientListener and NetworkServerListener

The network client/server listener is used to hold PacketConsumers, and direct packets to the correct consumer.
