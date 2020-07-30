using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SLUM.lib.Exception;

namespace SLUM.lib.Data.DataTypes
{
    public struct VarLong : IDataType
    {

        long _value;

        public byte PacketSize
        {
            get
            {
                byte length = 0;
                ulong workingValue = (ulong)Value;
                do
                {
                    workingValue >>= 7;
                    length++;
                } while (workingValue != 0);
                return length;
            }
        }

        public long Value
        {
            get { return _value; }
            set
            {
                _value = value;
            }
        }

        public VarLong(long value) : this()
        {
            Value = value;
        }

        public static implicit operator VarLong(long value) => new VarLong(value);
        public static implicit operator long(VarLong value) => value.Value;

        

    }
}
