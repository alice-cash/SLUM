using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Text;
using SLUM.lib.Client;
using SLUM.lib.Client.Protocol.Netty;
using SLUM.lib.Data.DataTypes;

namespace SLUM.lib.Data
{
    public class ByteWriter
    {
        private List<byte> _data;
        private INetConnection _connection;

        private Encoding _encoder;

        public int BufferLength { get { return _data.Count; } }

        public ByteWriter(INetConnection connection)
        {
            _connection = connection;
            _data = new List<byte>();
            _encoder = Encoding.UTF8;
        }

        public void Flush()
        {
            _connection.WritePacket(_data.ToArray());
            _data.Clear();
        }

        public void Clear()
        {
            _data.Clear();
        }
        public byte[] GetArray()
        {
            return _data.ToArray();
        }


        private IEnumerable<byte> GetBytes(short value)
        {
            return GetBytes((ushort)value);
        }
        private IEnumerable<byte> GetBytes(ushort value)
        {
            return new[]
            {
                (byte)((value & 0xFF00) >> 8),
                (byte)(value & 0xFF)
            };
        }
        private IEnumerable<byte> GetBytes(int value)
        {
            return GetBytes((uint)value);
        }
        private IEnumerable<byte> GetBytes(uint value)
        {
            return new[]
            {
                (byte)((value & 0xFF000000) >> 24),
                (byte)((value & 0xFF0000) >> 16),
                (byte)((value & 0xFF00) >> 8),
                (byte)(value & 0xFF)
            };
        }
        private IEnumerable<byte> GetBytes(long value)
        {
            return GetBytes((ulong)value);
        }
        private IEnumerable<byte> GetBytes(ulong value)
        {
            return new[]
            {
                (byte)((value & 0xFF00000000000000) >> 56),
                (byte)((value & 0xFF000000000000) >> 48),
                (byte)((value & 0xFF0000000000) >> 40),
                (byte)((value & 0xFF00000000) >> 32),
                (byte)((value & 0xFF000000) >> 24),
                (byte)((value & 0xFF0000) >> 16),
                (byte)((value & 0xFF00) >> 8),
                (byte)(value & 0xFF)
            };
        }

        private unsafe IEnumerable<byte> GetBytes(float value)
        {
            return GetBytes(*(uint*)&value);
        }

        private unsafe IEnumerable<byte> GetBytes(double value)
        {
            return GetBytes(*(ulong*)&value);
        }

        private void AddRange(IEnumerable<byte> value, bool prepend)
        {
            if (prepend) _data.InsertRange(0, value);
            else _data.AddRange(value);
        }

        private void Add(byte value, bool prepend)
        {
            if (prepend) _data.Insert(0, value);
            else _data.Add(value);
        }

        public void Write(byte[] value, bool prepend = false)
        {
            AddRange(value, prepend);
        }


        public void Write(byte value, bool prepend = false)
        {
            Add(value, prepend);
        }

        public void Write(bool value, bool prepend = false)
        {
            Add(value ? (byte)1 : (byte)0, prepend);
        }

        public void Write(ushort value, bool prepend = false)
        {
            AddRange(GetBytes(value), prepend);
        }

        public void Write(uint value, bool prepend = false)
        {
            AddRange(GetBytes(value), prepend);
        }

        public void Write(ulong value, bool prepend = false)
        {
            AddRange(GetBytes(value), prepend);
        }

        public void Write(short value, bool prepend = false)
        {
            AddRange(GetBytes(value), prepend);
        }

        public void Write(sbyte  value, bool prepend = false)
        {
            Add((byte)value, prepend);
        }

        public void Write(int value, bool prepend = false)
        {
            AddRange(GetBytes(value), prepend);
        }

        public void Write(long value, bool prepend = false)
        {
            AddRange(GetBytes(value), prepend);
        }

        public void Write(VarString value, bool prepend = false)
        {
            writeString(value, prepend);
        }
        public void Write(FixedString value, bool prepend = false)
        {
            writeString(value, prepend);
        }

        public void Write(VarInt value, bool prepend = false)
        {
            writeVarInt(value, prepend);
        }
        public void Write(VarLong value, bool prepend = false)
        {
            writeVarLong(value, prepend);
        }

        public void Write(NextState nextState)
        {
            Write(new VarInt((int)nextState));
        }
        private void writeVarInt(int value, bool prepend)
        {
            byte[] data = new byte[5];
            byte length = 0;
            uint workingValue = (uint)value;
            do
            {
                byte temp = (byte)(workingValue & 0b01111111);

                workingValue >>= 7;
                if (workingValue != 0)
                {
                    temp |= 0b10000000;
                }
                data[length] = temp;
                length++;
            } while (workingValue != 0);

            byte[] return_data = new byte[length];
            for (byte i = 0; i < length; i++)
            {
                return_data[i] = data[i];
            }
            if(prepend)
                _data.InsertRange(0, return_data);
            else
                _data.AddRange(return_data);
        }

        private void writeVarLong(long value, bool prepend)
        {
            byte[] data = new byte[10];
            byte length = 0;
            ulong workingValue = (ulong)value;
            do
            {
                byte temp = (byte)(workingValue & 0b01111111);

                workingValue >>= 7;
                if (workingValue != 0)
                {
                    temp |= 0b10000000;
                }
                data[length] = temp;
                length++;
            } while (workingValue != 0);
            byte[] return_data = new byte[length - 1];
            for (byte i = 0; i < length - 1; i++)
            {
                return_data[i] = data[i];
            }
            if (prepend)
                _data.InsertRange(0, return_data);
            else
                _data.AddRange(return_data);
        }

        private void writeString(VarString value, bool prepend)
        {
            byte[] data = _encoder.GetBytes(value);

            writeVarInt(data.Length, prepend);

            if (prepend)
                _data.InsertRange(0, data);
            else
                _data.AddRange(data);
        }
        private void writeString(FixedString value, bool prepend)
        {
            byte[] data = _encoder.GetBytes(value);

            Write(data.Length, prepend);
            if (prepend)
                _data.InsertRange(0, data);
            else
                _data.AddRange(data);
        }

        public void GZipEncode()
        {
            byte[] rawData = _data.ToArray();
            _data.Clear();

            MemoryStream memoryStream = new MemoryStream(rawData);
            GZipStream gZipStream = new GZipStream(memoryStream, CompressionLevel.Fastest);

            gZipStream.Write(rawData, 0, rawData.Length);
            gZipStream.Close();

            _data.AddRange(memoryStream.ToArray());
            memoryStream.Close();
            gZipStream.Dispose();
            memoryStream.Dispose();
        }


    }
}
