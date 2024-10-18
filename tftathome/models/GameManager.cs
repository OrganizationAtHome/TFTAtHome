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
        ActiveMatchScene node;
        public GameManager(ActiveMatchScene node)
        {
            this.node = node;
            //Upnp upnp = new();
            //SetupUpnp(upnp, 1234, "UDP");

            node.Multiplayer.PeerConnected += (id) =>
            {
                GD.Print("Server connected to client with the ID " + id);
            };
            var args = OS.GetCmdlineArgs();
            if (args.Contains("server"))
            {
                node.Join.Visible = false;
                node.JoinBox.Visible = false;
                ENetMultiplayerPeer server = new();
                StartServer(server, 1234, 4);

                node.Multiplayer.MultiplayerPeer = server;
            } 
        }

        public void JoinServer()
        {
            String ip = node.JoinBox.Text;
            ENetMultiplayerPeer client;

            client = new();
            Error clientError = ConnectClient(client, ip, 1234);

            node.Multiplayer.MultiplayerPeer = client;
        }

    }
}
