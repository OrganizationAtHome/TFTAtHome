using Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
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
            var args = OS.GetCmdlineArgs();
            if (args.Contains("server"))
            {
                ENetMultiplayerPeer server = new();
                StartServer(server, 1234, 4);

                node.Multiplayer.MultiplayerPeer = server;
            } else
            {
                ENetMultiplayerPeer client;
                do
                {
                    client = new();
                    Error clientError = ConnectClient(client, "localhost", 1234);

                    node.Multiplayer.MultiplayerPeer = client;

                    Thread.Sleep(400);

                } while (client.GetConnectionStatus() != MultiplayerPeer.ConnectionStatus.Connected);

            }








        }

    }
}
