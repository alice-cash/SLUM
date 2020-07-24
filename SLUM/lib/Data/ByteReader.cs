using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using SLUM.lib.Client;
using SLUM.lib.Data.DataTypes;
using StormLib;

namespace SLUM.lib.Data
{


    /// <summary>
    /// Reads from a byte array.
    /// </summary>
    public class ByteReader
    {
        //private byte[] _array;
        private ByteConverter _bc;
        private Encoding _encoder;

        private INetConnection _connection;
        private Stream _stream;
        //BinaryReader _br;

        /*
        public ByteReader(byte[] array, int offset, int length)
        {
            if (offset == 0 && array.Length == length)
            {
                _array = new byte[length];
                Array.Copy(array, offset, _array, 0, length);
            }
            else
            {
                _array = array;
            }
            Init();
        }
        
        public ByteReader(byte[] array)
            : this(array, 0, array.Length)
        {

        }
        */
        public ByteReader(INetConnection connection)
        {
            _connection = connection;
            _stream = _connection.Stream;
           // _br = new BinaryReader(_stream);
            //_array = new byte[length];
            //stream.Read(_array, offset, length);
            Init();
        }
        /*
        public ByteReader(BinaryReader stream, int length)
        {
            _array = new byte[length];
            _array = stream.ReadBytes(length);
            Init();
        }

        public void LoadNewArray(byte[] array)
        {
            _pos = 0;
            _array = array;
        }
        
        public byte[] DumpDebugInfo()
        {
            List<byte> debug = new List<byte>();
            debug.AddRange(BitConverter.GetBytes(_pos));
            debug.AddRange(_array);
            return debug.ToArray();
        }*/

        private void Init()
        {
            _encoder = Encoding.UTF8;
            _bc = new ByteConverter();
            
        }


        private ExecutionState EnforceLength(int length)
        {
            if (length > _connection.BytesAvaliable) return ExecutionState.Failed();
            return ExecutionState.Succeeded();
        }


        private ulong _readUInt64() => unchecked(((ulong)_readByte() << 56) | ((ulong)_readByte() << 48) | ((ulong)_readByte() << 32) |
                    ((ulong)_readByte() << 24) | ((ulong)_readByte() << 16) | ((ulong)_readByte() << 8) | (ulong)_readByte());
        private uint _readUInt32() => unchecked(((uint)_readByte() << 24) | ((uint)_readByte() << 16) | ((uint)_readByte() << 8) | (uint)_readByte());
        private ushort _readUInt16() => unchecked((ushort)(((ushort)_readByte() << 8) | (ushort)_readByte()));
        private byte _readByte(bool peek = false) => _connection.ReadBytes(1, peek)[0];

        public ExecutionState<byte> ReadByte(bool peek = false)
        {
            if (!EnforceLength(sizeof(byte))) return ExecutionState<byte>.Failed("Failed to enforce length of byte");
            return ExecutionState<byte>.Succeeded(_readByte(peek));
        }

        public ExecutionState<byte[]> ReadBytes(int length)
        {
            if (!EnforceLength(sizeof(byte) * length)) return ExecutionState<byte[]>.Failed("Failed to enforce length of byte[]");
            return ExecutionState<byte[]>.Succeeded(_connection.ReadBytes(length));
        }

        public ExecutionState<sbyte> ReadSByte()
        {
            if (!EnforceLength(sizeof(sbyte))) return ExecutionState<sbyte>.Failed("Failed to enforce length of sbyte");
            return ExecutionState<sbyte>.Succeeded((sbyte)_readByte());
        }

        public ExecutionState<short> ReadShort()
        {
            if (!EnforceLength(sizeof(short))) return ExecutionState<short>.Failed("Failed to enforce length of short");
            var result = ExecutionState<short>.Succeeded((short)_readUInt16());
            return result;
        }

        public ExecutionState<ushort> ReadUShort()
        {
            if (!EnforceLength(sizeof(ushort))) return ExecutionState<ushort>.Failed("Failed to enforce length of ushort");
            var result = ExecutionState<ushort>.Succeeded(_readUInt16());
            return result;
        }

        public ExecutionState<int> ReadInt()
        {
            if (!EnforceLength(sizeof(int))) return ExecutionState<int>.Failed("Failed to enforce length of int");
            var result = ExecutionState<int>.Succeeded((int)_readUInt32());
            return result;
        }

        public ExecutionState<uint> ReadUInt()
        {
            if (!EnforceLength(sizeof(uint))) return ExecutionState<uint>.Failed("Failed to enforce length of uint");
            var result = ExecutionState<uint>.Succeeded(_readUInt32());
            return result;
        }

 
        public ExecutionState<long> ReadLong()
        {
            if (!EnforceLength(sizeof(long))) return ExecutionState<long>.Failed("Failed to enforce length of long");
            var result = ExecutionState<long>.Succeeded((long)_readUInt64());
            return result;
        }

        public ExecutionState<ulong> ReadULong()
        {
            if (!EnforceLength(sizeof(ulong))) return ExecutionState<ulong>.Failed("Failed to enforce length of ulong");
            var result = ExecutionState<ulong>.Succeeded(_readUInt64());
            return result;
        }



        public unsafe ExecutionState<float> ReadSingle()
        {
            if (!EnforceLength(sizeof(float))) return ExecutionState<float>.Failed("Failed to enforce length of float");
            uint value = _readUInt32();
            var result = ExecutionState<float>.Succeeded(*(float*)&value);
            return result;
        }

        public unsafe ExecutionState<double> ReadDouble()
        {
            if (!EnforceLength(sizeof(double))) return ExecutionState<double>.Failed("Failed to enforce length of double");
            ulong value = _readUInt64();
            var result = ExecutionState<double>.Succeeded(*(double*)&value);
            return result;
        }

        public ExecutionState<bool> ReadBoolean()
        {
            if (!EnforceLength(sizeof(bool))) return ExecutionState<bool>.Failed("Failed to enforce length of bool");
            var result = ExecutionState<bool>.Succeeded(_readByte() != 0);
            return result;
        }

        public ExecutionState<string> ReadVarString()
        {
            var stateLen = ReadVarInt();
            if (!stateLen) return ExecutionState<string>.Failed("Failed to read Length");
            var length = stateLen.Result;
            var stateData = ReadBytes(length);
            if (!stateData) return ExecutionState<string>.Failed("Failed to read Data off of length");
            return ExecutionState<string>.Succeeded(_encoder.GetString(stateData.Result));
        }

        public ExecutionState<int> ReadVarInt()
        {
            int result = 0;
            int numRead = 0;
            byte read;
            do
            {
                if (!_connection.DataAvaliable) return ExecutionState<int>.Failed("Failed to read VarInt length");
                read = _readByte();
                int value = ((byte)read & 0b01111111);
                result |= (value << (7 * numRead));

                numRead++;
                if (numRead > 5)
                    return ExecutionState<int>.Failed("Length too long"); ;
            } while ((read & 0b10000000) != 0);

            return ExecutionState<int>.Succeeded(result);
        }
        public ExecutionState<long> ReadVarLong()
        {
            long result = 0;
            int numRead = 0;
            byte read;
            do
            {
                if (!_connection.DataAvaliable) return ExecutionState<long>.Failed("Failed to read VarLong length");
                read = _readByte();
                long value = ((byte)read & 0b01111111);
                result |= value << (7 * numRead);

                numRead++;
                if (numRead > 10)
                    return ExecutionState<long>.Failed("Length too long"); ;
            } while ((read & 0b10000000) != 0);

            return ExecutionState<long>.Succeeded(result);
        }
    }
}


