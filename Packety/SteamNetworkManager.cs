using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Packety.Listeners;
using Packety.Packagers;
using Steamworks;
using Steamworks.Data;

namespace Packety
{
    public class SteamNetworkManager : INetworkManager
    {
        public INetworkServerListener NetworkServerListener { get; private set; }
        public INetworkClientListener NetworkClientListener { get; private set; }

        private List<Connection> Connections { get; } = new();

        public ConnectionManager Client { get; private set; }
        public SocketManager Server { get; private set; }
        public INetworkMessagePackager MessagePackager { get; }

        public SteamNetworkManager(INetworkMessagePackager messagePackager)
        {
            MessagePackager = messagePackager;
        }

        public SocketManager CreateSession(string port)
        {
            var netAddress = NetAddress.AnyIp(ushort.Parse(port));
            Server = SteamNetworkingSockets.CreateNormalSocket<SocketManager>(netAddress);

            SteamMatchmaking.OnLobbyCreated += (result, lobby) => OnLobbyCreated(result, lobby, port);
            SteamMatchmaking.CreateLobbyAsync();

            return Server;
        }

        private static void OnLobbyCreated(Result _, Lobby lobby, string port)
        {
            lobby.SetPublic();
            lobby.SetData("ip", new WebClient().DownloadString("http://ipv4.icanhazip.com/"));
            lobby.SetData("port", port);
        }

        public ConnectionManager JoinSession(string ip, string port)
        {
            Client = SteamNetworkingSockets.ConnectNormal<ConnectionManager>(NetAddress.From(ip, ushort.Parse(port)));
            return Client;
        }

        public void Update()
        {
            Server?.Receive();
            Client?.Receive();
        }

        public void SetServerNetworkListener(INetworkServerListener networkServerListener) =>
            NetworkServerListener = networkServerListener;

        public void SetClientNetworkListener(INetworkClientListener clientNetworkListener) =>
            NetworkClientListener = clientNetworkListener;

        public void SendMessage(byte[] data)
        {
            if (Server?.Connected != null)
                foreach (var connection in Server.Connected)
                {
                    connection.SendMessage(data);
                }

            Client?.Connection.SendMessage(data);
        }

        public void SendMessage(byte[] data, Connection to) => to.SendMessage(data);

        public void RelayMessage(IntPtr data, int size, Connection ignore)
        {
            if (Server?.Connected == null) return;

            foreach (var connection in Server.Connected.Where(connection => connection != ignore))
                connection.SendMessage(data, size);
        }
    }
}