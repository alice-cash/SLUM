using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using SLUM.lib.Client.Protocol.Netty.AnyState.ClientBound;
using SLUM.lib.Client.Protocol.Netty.HandshakeState.ServerBound;
using SLUM.lib.Client.Protocol.Netty.LoginState.ClientBound;
using SLUM.lib.Client.Protocol.Netty.LoginState.ServerBound;
using SLUM.lib.Client.Protocol.Netty.StatusState.ClientBound;
using SLUM.lib.Client.Protocol.Netty.StatusState.ServerBound;



namespace SLUM.lib.Client.Protocol.Netty
{
    public abstract class Protocol : IProtocol
    {
        public abstract uint Version { get; }
        protected RemoteClient _client;
        public NetworkProtocol Format => NetworkProtocol.JavaNetty;

        public static Dictionary<int, Type> PacketsHandshakeServerbound;
        public static Dictionary<int, Type> PacketsHandshakeClientbound;

        public static Dictionary<int, Type> PacketsStatusServerbound;
        public static Dictionary<int, Type> PacketsStatusClientbound;

        public static Dictionary<int, Type> PacketsLoginServerbound;
        public static Dictionary<int, Type> PacketsLoginClientbound;

        public static Dictionary<int, Type> PacketsPlayServerbound;
        public static Dictionary<int, Type> PacketsPlayClientbound;

        static Protocol()
        {
            PacketsHandshakeServerbound = new Dictionary<int, Type>();
            PacketsHandshakeServerbound[Handshake.PacketID] = typeof(Handshake);

            PacketsHandshakeClientbound = new Dictionary<int, Type>();
            PacketsHandshakeClientbound[Disconnect.PacketID] = typeof(Disconnect);


            PacketsStatusServerbound = new Dictionary<int, Type>();
            PacketsStatusServerbound[StatusRequest.PacketID] = typeof(StatusRequest);
            PacketsStatusServerbound[StatusPing.PacketID] = typeof(StatusPing);

            PacketsStatusClientbound = new Dictionary<int, Type>();
            PacketsStatusClientbound[StatusResponse.PacketID] = typeof(StatusResponse);
            PacketsStatusClientbound[StatusPong.PacketID] = typeof(StatusPong);


            PacketsLoginServerbound = new Dictionary<int, Type>();
            PacketsLoginServerbound[LoginStart.PacketID] = typeof(LoginStart);
            PacketsLoginServerbound[LoginEncryptionRequest.PacketID] = typeof(LoginEncryptionRequest);
            PacketsLoginServerbound[LoginPluginResponse.PacketID] = typeof(LoginPluginResponse);

            PacketsLoginClientbound = new Dictionary<int, Type>();
            PacketsLoginClientbound[Disconnect.PacketID] = typeof(Disconnect);
            PacketsLoginClientbound[LoginEncryptionRequest.PacketID] = typeof(LoginEncryptionRequest);
            PacketsLoginClientbound[LoginSucess.PacketID] = typeof(LoginSucess);
            PacketsLoginClientbound[LoginSetCompression.PacketID] = typeof(LoginSetCompression);
            PacketsLoginClientbound[LoginPluginRequest.PacketID] = typeof(LoginPluginRequest);


            PacketsPlayServerbound = new Dictionary<int, Type>();

            PacketsPlayClientbound = new Dictionary<int, Type>();
        }

        public Protocol(RemoteClient client)
        {
            _client = client;
            client.StateChange += (o, e) => SetupAllowedPacket();
        }

        protected abstract void SetupAllowedPacket();

        public void ReadClient()
        {
            if (_client.ClientConnection.DataAvaliable)
            {
                var Length = _client.StreamReader.ReadVarInt();
                var PacketID = _client.StreamReader.ReadVarInt();
                IPacket packet = null;

                if (Length.Sucess && PacketID.Sucess)
                {
                    if(_client.State == ClientState.Status)
                    {
                        // Handshake is taken care of in the UnknownProtocol class
                        if (PacketsStatusServerbound.ContainsKey(PacketID.Result))
                            packet = (IPacket)Activator.CreateInstance(PacketsStatusServerbound[PacketID.Result]);
                              
                    }
                    else if (_client.State == ClientState.Login)
                    {
                        if (PacketsLoginServerbound.ContainsKey(PacketID.Result))
                            packet = (IPacket)Activator.CreateInstance(PacketsLoginServerbound[PacketID.Result]);
                    }

                }

                if (packet == null)
                    Trace.WriteLine("Packet not handled!: " + PacketID.Result);
                else
                {
                    packet.PacketLength = Length.Result;
                    PacketManager.ReadFromClient(_client, ref packet);

                    HandlePacket(packet);
                }
 
            }
        }

        public bool HandlePacket(IPacket packet)
        {
            Console.WriteLine(packet.GetPacketID + " <- " +  packet.GetType().ToString());

            if (packet.PacketGood == false)
            {
                _client.Disconnect();
                return false;
            }

            if (packet is Handshake)
            {
                if (((Handshake)packet).ProtocolVersion != Version)
                {
                    Trace.WriteLine(string.Format("Protocol versiom mismatch! Check handling for {0}", this.GetType().ToString()));
                    _client.Disconnect();
                    return false;
                }
                if (((Handshake)packet).NextState == NextState.Status)
                    _client.State = ClientState.Status;
                else if (((Handshake)packet).NextState == NextState.Login)
                    _client.State = ClientState.Login;
                else
                {
                    _client.Disconnect();
                    return false;
                }
                return true;
            }

            if (packet is StatusRequest)
            {
                _client.Data.StatusRequest = true;
                _client.Data.StatusRespond = false;
                return true;
            }

            if (packet is StatusPing)
            {
                _client.Data.StatusPingPayload = ((StatusPing)packet).StatusPingPayload;
                _client.Data.StatusPing = true;
            }



            if (packet is LoginStart)
            {
                _client.Data.Username = ((LoginStart)packet).Name;
                _client.Data.LoginRespondRequired = true;
                return true;
            }

            if (packet is LoginEncryptionResponse)
            {
                _client.Data.LoginSecret = ((LoginEncryptionResponse)packet).Secret;
                _client.Data.LoginVerifyToken = ((LoginEncryptionResponse)packet).Token;
                _client.Data.LoginEncryptionResponseRequired = true;
                return true;
            }


            return _handlePacket(packet);
        }

        protected internal abstract bool _handlePacket(IPacket packet);

        public void TickStatus()
        {
            if (_client.Data.StatusRequest && !_client.Data.StatusRespond)
            {
                var ev = EventManager.StatusEvent.StatusEvent(this, new Events.StatusEventArgs(_client));
                _client.Data.StatusRespond = true;
                return;
            }
            if (_client.Data.StatusPing)
            {
                var packet = new StatusPong() { PingPayload = _client.Data.StatusPingPayload };
                PacketManager.WriteToClient(_client, packet);
                _client.Disconnect();
                return;
            }

        }

        public void TickLogin()
        {
            if (_client.Data.LoginRespondRequired)
            {
                _client.State = ClientState.Play;
                _client.Data.UUID = "d973b9ce-ce3d-11ea-87d0-0242ac130003";
                var packet = new LoginSucess() { Username = _client.Data.Username, UUID = _client.Data.UUID };
                PacketManager.WriteToClient(_client, packet);
                _client.Data.LoginEncryptionResponseRequired = false;
                _client.Data.LoginRespondRequired = false;
                return;
                
                _client.Data.Token = getRandomBytes(40);


                var keyParams = Encryption.PublicKey;

                /*var packet = new EncryptionRequest() { 
                    ServerID = getRandomString(20), 
                    PublicKey = keyParams, 
                    VerifyToken = _client.Data.Token 
                };

                packet.SendPacket(_client);
                */
                _client.Data.LoginRespondRequired = false;
                return;
            }

            if (_client.Data.LoginEncryptionResponseRequired)
            {
                _client.Data.LoginSecret = Encryption.RSAHandle.Decrypt(_client.Data.LoginSecret, RSAEncryptionPadding.Pkcs1);
                _client.Data.LoginVerifyToken = Encryption.RSAHandle.Decrypt(_client.Data.LoginVerifyToken, RSAEncryptionPadding.Pkcs1);
                if (!checkByteArrays(_client.Data.LoginVerifyToken, _client.Data.Token))
                {
                    _client.Disconnect();
                    return;
                }

//                var key = Encryption.RSAHandle.ExportRSAPrivateKey();

                (_client.ClientConnection as TCPConnection).EnableEncryption(_client.Data.LoginSecret, _client.Data.LoginSecret);

                _client.Data.UUID = "d973b9ce-ce3d-11ea-87d0-0242ac130003";

                var packet = new LoginSucess() { Username = _client.Data.Username, UUID = _client.Data.UUID };
                PacketManager.WriteToClient(_client, packet);
                _client.Data.LoginEncryptionResponseRequired = false;
                return;
            }
        }

        private static bool checkByteArrays(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length) return false;
            for (int i = 0; i < array1.Length; i++)
                if (array1[i] != array2[i]) return false;
            return true;
        }

        private static byte[] getRandomBytes(int length)
        {
            byte[] data = new byte[length];
            Encryption.GetRandom.NextBytes(data);
            return data;
        }


        private static string getRandomString(int length, string source = "abcdef1234567890")
        {
            char[] data = new char[length];
            for(int i = 0; i < length; i++)
                data[i] = source[Encryption.GetRandom.Next(0,source.Length-1)];
            return new string(data);
        }
    }
}
