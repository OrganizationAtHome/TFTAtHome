using Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static TFTAtHome.util.MultiplayerUtil;

namespace TFTAtHome.models
{
    internal class GameManager
    {
        HomeScreen node;
        public GameManager(HomeScreen node)
        {
            this.node = node;
            //Upnp upnp = new();
            //SetupUpnp(upnp, 1234, "UDP");

            var args = OS.GetCmdlineArgs();
            if (args.Contains("server"))
            {
                ENetMultiplayerPeer server = new();
                StartServer(server, 1234, 4);
                server.PeerConnected += (id) =>
                {
                    GD.Print("client connected with the ID " + id);
                    // node.ClientBody.SetMultiplayerAuthority(((int)id), true);
                };

                node.Multiplayer.MultiplayerPeer = server;
            } 
        }

        public Error JoinServer()
        {
            String ip = node.IPBox.Text;
            ENetMultiplayerPeer client;

            client = new();
            Error clientError = ConnectClient(client, ip, 1234);
            // node.ClientBody.SetMultiplayerAuthority(client.GetUniqueId(), true);

            node.Multiplayer.MultiplayerPeer = client;

            return clientError;
        }
        public Error HostServer()
        {
            ENetMultiplayerPeer server = new();
            Error serverError = StartServer(server, 1234, 4);
            server.PeerConnected += (id) =>
            {
                GD.Print("client connected with the ID " + id);
            };
            node.Multiplayer.MultiplayerPeer = server;
            return serverError;
        }
    }
}
