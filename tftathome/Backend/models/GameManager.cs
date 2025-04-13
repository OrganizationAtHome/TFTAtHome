using Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TFTAtHome.Backend.models.Matches;
using TFTAtHome.Backend.storage;
using static TFTAtHome.util.MultiplayerUtil;

namespace TFTAtHome.Backend.models
{
    public class GameManager
    {

        public long Id { get; set; }
        private readonly List<Player> _players = new List<Player>();
        private List<Card> _activeCardPool;
        private List<Card> _inactiveCards = new List<Card>();
        private List<Match> matches = new List<Match>();
        public GameState GameState { get; set; }

        public GameManager(HomeScreen node, long id)
        {
            Id = id;
            GameState = GameState.AWAITING_PLAYERS;
            _activeCardPool = LocalStorage.getCards();
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

        public Error JoinServer(Node node, string IP)
        {
            ENetMultiplayerPeer client = new();
            Error clientError = ConnectClient(client, IP, 1234);
            // node.ClientBody.SetMultiplayerAuthority(client.GetUniqueId(), true);

            node.Multiplayer.MultiplayerPeer = client;

            return clientError;
        }
        public Error HostServer(Node node)
        {
            ENetMultiplayerPeer server = new();
            Error serverError = StartServer(server, 1234, 4);
            server.PeerConnected += (id) =>
            {
                GD.Print("client connected with the ID " + id);
                Player player = new(id, "Test" + id);
                _players.Add(player);
            };
            node.Multiplayer.MultiplayerPeer = server;
            return serverError;
        }

        public List<Card> GetActiveCardPool()
        {
            return _activeCardPool;
        }
    }
}
