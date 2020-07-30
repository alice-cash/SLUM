using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.IO;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace SLUM.lib.Client
{
    public class TCPConnection : INetConnection
    {
        private Socket _sock;
        private NetworkStream _stream;
        private bool _sockDisposed;

        public bool Encryption { get; private set; }

        private Stream _encryptor;
        //private Stream _decryptor;




        public TCPConnection(Socket client)
        {
            _sock = client;
            _stream = new NetworkStream(_sock);
            _sockDisposed = false;
        }

        public bool IsConnected => _sock == null || _sockDisposed ? false : CheckForError() ? false : _sock.Connected;

        public IPAddress RemoteAddress => IsConnected ? ((IPEndPoint)_sock.RemoteEndPoint).Address : IPAddress.None;

        public bool DataAvaliable => _sock.Connected && _sock.Available > 0;

        public int BytesAvaliable => _sock.Available;

        public Stream Stream => _stream;

        private bool CheckForError()
        {
            if (_sockDisposed)
                return true;
            return _sock.Poll(0, SelectMode.SelectError);
        }

        public void EnableEncryption(byte[] key, byte[] iv)
        {
            var keyParam = new KeyParameter(key);

            var param = new ParametersWithIV(keyParam, iv);

            var cipherEncrypt = CipherUtilities.GetCipher("AES/CFB8/NoPadding");
            var cipherDecrypt = CipherUtilities.GetCipher("AES/CFB8/NoPadding");

             
            cipherEncrypt.Init(true, param);
            cipherDecrypt.Init(false, param);

            _encryptor = new CipherStream(_stream, cipherEncrypt, cipherDecrypt);
            
            //_encryptor = new CryptoStream(_stream, Aes.Create().CreateEncryptor(param, iv), CryptoStreamMode.Write, true);
            //_decryptor = new CryptoStream(_stream, Aes.Create().CreateDecryptor(param, iv), CryptoStreamMode.Read, true);
            Encryption = true;
        }

        public void WritePacket(byte[] data)
        {
            if (IsConnected)
                try
                {
                    if (Encryption)
                        _encryptor.Write(data);
                    else
                        _stream.Write(data);
                }
                catch
                {
                    Disconnect();
                }
            //_sock.Send(data);
        }

        public byte[] ReadBytes(int length, bool peak = false)
        {
            if(peak && Encryption)
                throw new InvalidOperationException("Cannot peek the network stack when encrypted.");

            SocketFlags sf = SocketFlags.None;
            if (peak) sf = SocketFlags.Peek;
            if (_sock != null && !_sockDisposed)
            {
                byte[] data = new byte[length];
                try
                {
                    int readstate;
                    if (Encryption)
                        readstate = _encryptor.Read(data, 0, length);
                    else
                        readstate = _sock.Receive(data, length, sf);
                    if (readstate == length)
                        return data;
                }
                catch
                {
                    Disconnect();
                }
            }
            return new byte[0];
        }

        public void Disconnect()
        {
            System.Diagnostics.Trace.WriteLine(string.Format("Disconnected {0}", _sock.RemoteEndPoint.ToString()));
            if (_sock != null && !_sockDisposed)
            {
                if (_sock.Connected)
                    _sock.Disconnect(false);
                _sock.Close();
                _sockDisposed = true;

                if (Encryption)
                {
                    //_decryptor.Close();
                    //_decryptor.Dispose();
                    _encryptor.Close();
                    _encryptor.Dispose();
                }
            }
        }

    }
}
