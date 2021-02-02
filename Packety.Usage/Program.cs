using System;
using Packety.Listeners;
using Packety.Packagers;
using Steamworks;

namespace Packety.Usage
{
    public static class Program
    {
        public static void Main()
        {
            // Make sure SteamClient has been initialized.
            InitializeSteam();

            // Create a NetworkMessagePackager
            var networkMessagePackager = new NetworkMessagePackager();

            // Add definition for the MyPacket class
            networkMessagePackager.AddDefinition<MyPacket>();

            // Create the network manager and provided the NetworkMessagePackager
            var networkManager = new SteamNetworkManager(networkMessagePackager);


            Server(networkManager, networkMessagePackager);
            Client(networkManager, networkMessagePackager);

            Console.ReadLine();
        }

        private static void InitializeSteam()
        {
            try
            {
                SteamClient.Init(1323490);
            }
            catch (Exception)
            {
                // Couldn't init for some reason (steam is closed etc)
            }
        }

        private static void Client(INetworkManager networkManager, INetworkMessagePackager networkMessagePackager)
        {
            // Create the NetworkListener for either client, or server
            var clientNetworkListener = new ClientNetworkListener(networkManager);

            // Add a consumer to the NetworkListener so it can direct corresponding packets
            clientNetworkListener.AddConsumer(new MyPacketConsumer());

            // Set the Client/Server network listener on the NetworkManager
            networkManager.SetClientNetworkListener(clientNetworkListener);

            // To join a Server
            networkManager.JoinSession("127.0.0.1", "25565");

            var myPacket = new MyPacket
            {
                Name = "Hello, World!"
            };

            var packet = networkMessagePackager.Package(myPacket);
            networkManager.SendMessage(packet);

            // Update at a set interval, so that packets can be picked up from the network.
            networkManager.Update();
        }

        private static void Server(INetworkManager networkManager, INetworkMessagePackager networkMessagePackager)
        {
            var serverNetworkListener = new ServerNetworkListener(networkManager);
            serverNetworkListener.AddConsumer(new MyPacketConsumer());

            networkManager.SetServerNetworkListener(serverNetworkListener);
            
            // Creates the Server Networking Sessions
            networkManager.CreateSession("25565");
            
            // Update at a set interval, so that packets can be picked up from the network.
            networkManager.Update();
        }
    }
}