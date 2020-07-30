using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SLUM.lib.Data;
using SLUM.lib.Data.DataTypes;

namespace SLUM.lib.Client.Protocol.Netty
{
    public static class PacketManager
    {
        //public bool PacketGood { get; set; }
        //public bool Compression { get; set; }


        //public int PacketLength { get; protected internal set; }

        //public IPacket Packet { get; protected internal set; }

        //public PacketManager(IPacket packet)
        //{
        //    Packet = packet;
        //}
        public static void ReadFromClient(RemoteClient client, ref IPacket packet)
        {
            var fields = GetPacketFields(packet);
            foreach (var field in fields)
            {
                if (!ReadField(client, field.Value, packet))
                {
                    packet.PacketGood = false;
                    return;
                }
            }
            packet.PacketGood = true;
            //TryReadStream(client);
        }

        public static void WriteToClient(RemoteClient client, IPacket packet)
        {
            Console.WriteLine(packet.GetPacketID + " -> " + packet.GetType().ToString());

            VarInt id = packet.GetPacketID;
            client.StreamWriter.Write(id, false);
            VarInt length = client.StreamWriter.BufferLength;

            var fields = GetPacketFields(packet);
            
            foreach (var field in fields)
            {
                WriteField(client, field.Value, packet);
            }

            if (client.Data.Compression)
            {
                client.StreamWriter.GZipEncode();
                client.StreamWriter.Write(length, true);
            }

            length = client.StreamWriter.BufferLength;

            client.StreamWriter.Write(length, true);
            client.StreamWriter.Flush();

        }
        private static bool ReadField(RemoteClient client, PropertyInfo field, IPacket packet) {
            var attributes = ((PacketFieldAttribute)field.GetCustomAttributes(typeof(PacketFieldAttribute), false)[0]);

            switch (attributes.Type)
            {
                case Types.VarInt:
                    var readVarInt = client.StreamReader.ReadVarInt();
                    if (!readVarInt) return false;
                    field.SetValue(packet, readVarInt.Result);
                    break;
                case Types.VarLong:
                    var readVarLong = client.StreamReader.ReadVarLong();
                    if (!readVarLong) return false;
                    field.SetValue(packet, readVarLong.Result);
                    break;

                case Types.Byte:
                    var readByte = client.StreamReader.ReadByte();
                    if (!readByte) return false;
                    field.SetValue(packet, readByte.Result);
                    break;
                case Types.UShort:
                    var readUShort = client.StreamReader.ReadUShort();
                    if (!readUShort) return false;
                    field.SetValue(packet, readUShort.Result);
                    break;
                case Types.UInt:
                    var readUInt = client.StreamReader.ReadUInt();
                    if (!readUInt) return false;
                    field.SetValue(packet, readUInt.Result);
                    break;
                case Types.ULong:
                    var readULong = client.StreamReader.ReadULong();
                    if (!readULong) return false;
                    field.SetValue(packet, readULong.Result);
                    break;

                case Types.SByte:
                    var readSByte = client.StreamReader.ReadSByte();
                    if (!readSByte) return false;
                    field.SetValue(packet, readSByte.Result);
                    break;
                case Types.Short:
                    var readShort = client.StreamReader.ReadShort();
                    if (!readShort) return false;
                    field.SetValue(packet, readShort.Result);
                    break;
                case Types.Int:
                    var readInt = client.StreamReader.ReadInt();
                    if (!readInt) return false;
                    field.SetValue(packet, readInt.Result);
                    break;
                case Types.Long:
                    var readLong = client.StreamReader.ReadLong();
                    if (!readLong) return false;
                    field.SetValue(packet, readLong.Result);
                    break;

                case Types.Boolean:
                    var readBoolean = client.StreamReader.ReadBoolean();
                    if (!readBoolean) return false;
                    field.SetValue(packet, readBoolean.Result);
                    break;

                case Types.VarString:
                case Types.Chat:
                    var readVarString = client.StreamReader.ReadVarString(attributes.MaxLength);
                    if (!readVarString) return false;
                    field.SetValue(packet, readVarString.Result);
                    break;
                case Types.FixedString:
                    var readFixedString = client.StreamReader.ReadFixedString(attributes.MaxLength);
                    if (!readFixedString) return false;
                    field.SetValue(packet, readFixedString.Result);
                    break;

                case Types.EnumNextState:

                    var readEnumNextState = client.StreamReader.ReadVarInt();
                    if (!readEnumNextState) return false;
                    if (Enum.IsDefined(typeof(NextState), (int)readEnumNextState.Result))
                        field.SetValue(packet, (NextState)(int)readEnumNextState.Result);
                    else return false;
                    break;

                default:
                    throw new System.Exception("Forget somthing?");
            }

            return true;
        }


        private static void WriteField(RemoteClient client, PropertyInfo field, IPacket packet) {
            var attributes = ((PacketFieldAttribute)field.GetCustomAttributes(typeof(PacketFieldAttribute), false)[0]);

            switch (attributes.Type)
            {
                case Types.VarInt:
                    client.StreamWriter.Write((VarInt)field.GetValue(packet));
                    break;
                case Types.VarLong:
                    client.StreamWriter.Write((VarLong)field.GetValue(packet));
                    break;

                case Types.Byte:
                    client.StreamWriter.Write((byte)field.GetValue(packet));
                    break;
                case Types.UShort:
                    client.StreamWriter.Write((ushort)field.GetValue(packet));
                    break;
                case Types.UInt:
                    client.StreamWriter.Write((uint)field.GetValue(packet));
                    break;
                case Types.ULong:
                    client.StreamWriter.Write((ulong)field.GetValue(packet));
                    break;

                case Types.SByte:
                    client.StreamWriter.Write((SByte)field.GetValue(packet));
                    break;
                case Types.Short:
                    client.StreamWriter.Write((short)field.GetValue(packet));
                    break;
                case Types.Int:
                    client.StreamWriter.Write((int)field.GetValue(packet));
                    break;
                case Types.Long:
                    client.StreamWriter.Write((long)field.GetValue(packet));
                    break;

                case Types.Boolean:
                    client.StreamWriter.Write((Boolean)field.GetValue(packet));
                    break;

                case Types.VarString:
                    client.StreamWriter.Write((VarString)field.GetValue(packet));
                    break;

                case Types.Chat:
                    client.StreamWriter.Write((Chat)field.GetValue(packet));
                    break;

                case Types.EnumNextState:
                    client.StreamWriter.Write((NextState)field.GetValue(packet));
                    break;

                default:
                    throw new System.Exception("Forget somthing?");
            }

        }

        /*public static void SendPacket(RemoteClient client, IPacket packet)
        {
            Console.WriteLine(packet.GetPacketID + " -> " + packet.GetType().ToString());
            WriteToClient(client, packet);
        }*/

        private static SortedList<int, PropertyInfo> GetPacketFields(IPacket packet)
        {
            SortedList<int, PropertyInfo> Fields = new SortedList<int, PropertyInfo>();
            foreach(var property in packet.GetType().GetProperties())
            {
                var attributes = property.GetCustomAttributes(typeof(PacketFieldAttribute), false);
                if (attributes.Count() > 0)
                    Fields.Add(((PacketFieldAttribute)attributes[0]).FieldID, property);
            }
            return Fields;
        }
    }
}
