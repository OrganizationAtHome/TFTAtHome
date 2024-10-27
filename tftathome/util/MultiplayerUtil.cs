using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFTAtHome.util
{
    public static class MultiplayerUtil
    {
        public static Error ConnectClient(ENetMultiplayerPeer client, string host, int port)
        {
            Error clientError = client.CreateClient(host, port);
            
            if (clientError == Error.Ok)
            {
                GD.Print("Client connecting to server");
            }
            else if (clientError == Error.Failed)
            {
                GD.Print("Client failed to connect to server");
            }
            else if (clientError == Error.AlreadyInUse)
            {
                GD.Print("Client already connected to a server");
            }
            else if (clientError == Error.CantCreate)
            {
                GD.Print("Client failed to create");
            }
            else if (clientError == Error.CantResolve)
            {
                GD.Print("Client failed to resolve");
            }
            else if (clientError == Error.Timeout)
            {
                GD.Print("Client timed out");
            }
            else
            {
                GD.Print("Client failed to connect to server");
            }
            return clientError;
        }

        public static Error StartServer(ENetMultiplayerPeer server, int port, int maxPlayers) {
            Error serverError = server.CreateServer(port);
            if (serverError == Error.Ok)
            {
                GD.Print("Server started");
            }
            else if (serverError == Error.Failed)
            {
                GD.Print("Server failed to start");
            }
            else if (serverError == Error.AlreadyInUse)
            {
                GD.Print("Server already started");
            }
            else if (serverError == Error.CantCreate)
            {
                GD.Print("Server failed to create");
            }
            else if (serverError == Error.CantResolve)
            {
                GD.Print("Server failed to resolve");
            }
            else if (serverError == Error.Timeout)
            {
                GD.Print("Server timed out");
            }
            else
            {
                GD.Print("Server failed to start");
            }
            return serverError;
        }

        public static Upnp.UpnpResult SetupUpnp(Upnp upnp, int port, string proto)
        {
            upnp.Discover();
            Upnp.UpnpResult result;
            result = (Upnp.UpnpResult)upnp.AddPortMapping(port, 0, "TFTAtHome", proto, 120);


            if (result == Upnp.UpnpResult.Success)
            {
                GD.Print("Port mapping successful");
            }
            else if (result == Upnp.UpnpResult.InvalidArgs)
            {
                GD.Print("Port mapping invalid args");
            }
            else if (result == Upnp.UpnpResult.NoGateway)
            {
                GD.Print("Port mapping no gateway");
            }
            else if (result == Upnp.UpnpResult.ActionFailed)
            {
                GD.Print("Port mapping action failed");
            }
            else
            {
                GD.Print("Port mapping failed");
            }

            return result;

        }

    }
}
