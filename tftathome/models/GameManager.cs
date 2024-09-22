using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TFTAtHome.util.ZimmyUtil;

namespace TFTAtHome.models
{
    internal class GameManager
    {
        public GameManager(Node2D node)
        {
            //Upnp upnp = new();
            //SetupUpnp(upnp, 1234, "UDP");

            node.Multiplayer.PeerConnected += (id) =>
            {
                GD.Print("Server connected to client with the ID " + id);
            };

            ENetMultiplayerPeer server = new();
            server.PeerConnected += (id) =>
            {
                GD.Print("Client connected to server with the ID " + id);
            };
            server.PeerDisconnected += (id) =>
            {
                GD.Print("Client disconnected from server with the ID " + id);
            };
            StartServer(server, 1234, 4);

            node.Multiplayer.MultiplayerPeer = server;



            ENetMultiplayerPeer client2 = new();
            Error clientError2 = ConnectClient(client2, "127.0.0.1", 1234);
            server.DisconnectPeer(client2.GetUniqueId());


        }

    }
}
