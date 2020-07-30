using System;
using System.Collections.Generic;
using System.Text;
using SLUM.lib.Client;
using SLUM.lib.Client.Events;
using SLUM.lib.Client.Protocol;
using SLUM.lib.Client.Protocol.Netty;
using SLUM.lib.Client.Protocol.Netty.StatusState.ClientBound;
using StormLib;
using StormLib.Module;

namespace SLUM.lib.Modules.Client.Status
{

    class StatusModule : IModuleLoader
    {

        public Version Version
        {
            get { return new Version(1, 0, 0, 0); }
        }

        public string Name
        {
            get { return "Status Module"; }
        }

        public void Load()
        {
            EventManager.StatusEvent.RegisterStatus(10, RegisterFunction);

        }

        private bool RegisterFunction(object caller, StatusEventArgs args)
        {
            if(args.SendingClient.ClientProtocol.Format == NetworkProtocol.JavaNetty)
            {
                if (args.SendingClient.Data.StatusRequest && !args.SendingClient.Data.StatusRespond)
                {
                    StatusResponse packet = new StatusResponse()
                    {
                        StatusData = new Data.DataTypes.Chat( "{\"version\":{\"name\":\"Wak, 1.15.x\",\"protocol\":"+ args .SendingClient.ClientProtocol.Version+ "},\"players\":{\"max\":300,\"online\":0,\"sample\":[{\"name\":\"Wak\"" +
                    ",\"id\":\"00000000-0000-0000-0000-000000000000\"}" +
                    "]},\"description\":{\"extra\":[{\"color\":\"white\",\"text\":\"grief survival!\\n\"},{\"color\":\"dark_purple\",\"text\":\"DRAGONS!\"}],\"text\":\"\"}}")
                    };
                    PacketManager.WriteToClient(args.SendingClient, packet);

                    return true;
                }
            }
            return false;
        }


    }
}

