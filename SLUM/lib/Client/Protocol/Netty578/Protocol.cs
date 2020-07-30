using SLUM.lib.Client.Protocol.Netty;

namespace SLUM.lib.Client.Protocol.Netty578
{
    /// <summary>
    /// Protocol Handling for Release 1.15.2
    /// </summary>
    public class Protocol : Netty.Protocol
    {
        public Protocol(RemoteClient client) : base(client) { }

        public override uint Version => 578;
        protected override void SetupAllowedPacket()
        {
            switch (_client.State)
            {
                case ClientState.Handshaking:

                    
                    break;
                case ClientState.Status:


                    break;

                case ClientState.Login:


                    break;

                case ClientState.Play:


                    break;
            }
        }

        protected internal override bool _handlePacket(IPacket packet)
        {

            return false;
        }
    }
}
