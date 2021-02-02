using System;
using Packety.Listeners;
using Packety.Packagers;
using Steamworks;
using Steamworks.Data;

namespace Packety
{
    /// <summary>
    /// Allows for a common abstraction for all types of network management. Be it steam or peer-to-peer by IP.
    /// </summary>
    public interface INetworkManager
    {
        ConnectionManager Client { get; }
        SocketManager Server { get; }
        INetworkMessagePackager MessagePackager { get; }
        SocketManager CreateSession(string port);
        ConnectionManager JoinSession(string ip, string port);
        void Update();
        void SetServerNetworkListener(INetworkServerListener networkServerListener);
        void SetClientNetworkListener(INetworkClientListener clientNetworkListener);
        void SendMessage(byte[] data);
        void SendMessage(byte[] data, Connection other);
        void RelayMessage(IntPtr data, int size, Connection ignore);
    }
}