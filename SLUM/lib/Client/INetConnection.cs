using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SLUM.lib.Client
{
    public interface INetConnection
    {
        IPAddress RemoteAddress { get; }
        bool IsConnected { get; }
        bool DataAvaliable { get; }

        int BytesAvaliable { get; }
        Stream Stream { get; }

        void WriteBytes(byte[] data);
        byte[] ReadBytes(int length, bool peak = false);

        void Disconnect();
    }
}
