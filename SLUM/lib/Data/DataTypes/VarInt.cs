﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SLUM.lib.Exception;

namespace SLUM.lib.Data.DataTypes
{
    public struct VarInt: IDataType
    {

        int _value;

        public byte PacketSize
        {
            get
            {
                byte length = 0;
                uint workingValue = (uint)Value;
                do
                {
                    workingValue >>= 7;
                    length++;
                } while (workingValue != 0);
                return length;
            } 
        }

        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
            }
        }

        public VarInt(int value) : this()
        {
            Value = value;
        }

        public static implicit operator VarInt(int value) => new VarInt(value);
        public static implicit operator int(VarInt value) => value.Value;

        

    }
}
