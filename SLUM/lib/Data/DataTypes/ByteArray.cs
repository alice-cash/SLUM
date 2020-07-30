using System;
using System.Collections.Generic;
using System.Text;

namespace SLUM.lib.Data.DataTypes
{
    public struct ByteArray : IDataType
    {
        public byte[] Value { get; set; }
        public int Length { get { return Value.Length; }  }

        public ByteArray(byte[] value)
        {
            Value = value;
        }

        public static implicit operator ByteArray(byte[] value) => new ByteArray(value);
        public static implicit operator byte[](ByteArray value) => value.Value;
    }
}
