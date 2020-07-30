using System;
using System.Collections.Generic;
using System.Text;
using SLUM.lib.Data.DataTypes;

namespace SLUM.lib.Client.Protocol.Netty
{
    internal class PacketFieldAttribute : Attribute
    {
        public int FieldID { get; }

        public Types Type { get; }

        public int MaxLength { get; }

        public bool PrependLength { get; }


        public PacketFieldAttribute(int fieldID, Types type, int maxLength = -1, bool prependLength = true)
        {
            this.FieldID = fieldID;
            this.Type = type;
            this.MaxLength = maxLength;
            this.PrependLength = prependLength;
        }

    }
}
