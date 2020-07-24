using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SLUM.lib.Client
{
    public class TCPConnection : INetConnection
    {
        private Socket _sock;
        private NetworkStream _stream;
        private bool _sockDisposed;


        public TCPConnection(Socket client)
        {
            _sock = client;
            _stream = new NetworkStream(_sock);
            _sockDisposed = false;
        }

        public bool IsConnected =>  _sock == null || _sockDisposed ? false : CheckForError() ? false : _sock.Connected; 
             
        public IPAddress RemoteAddress =>  IsConnected ? ((IPEndPoint)_sock.RemoteEndPoint).Address : IPAddress.None;

        public bool DataAvaliable => _sock.Connected && _sock.Available > 0;

        public int BytesAvaliable => _sock.Available;

        public Stream Stream => _stream;

        private bool CheckForError()
        {
            if (_sockDisposed)
                return true;
            return _sock.Poll(0, SelectMode.SelectError);
        }

        public void WriteBytes(byte[] data)
        {
            if (IsConnected)
                try
                {
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
            SocketFlags sf = SocketFlags.None;
            if (peak) sf = SocketFlags.Peek;
            if (_sock != null && !_sockDisposed)
            {
                byte[] data = new byte[length];
                try
                {
                    int readstate = _sock.Receive(data, length, sf);
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
            }
        }

    }
}
