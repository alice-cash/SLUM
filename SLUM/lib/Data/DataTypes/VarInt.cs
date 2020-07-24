//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.IO;
//using SLUM.lib.Exception;

//namespace SLUM.lib.Data.DataTypes
//{
//    public struct VarInt: IData
//    {

//        public int Value
//        {
//            get { return _value; }
//            set
//            {
//                _cache_dirty = true;
//                _value = value;
//            }
//        }

//        public byte[] Data
//        {
//            get
//            {
//                if (_cache_dirty)
//                {
//                    _cache = WriteVarInt(_value);
//                    _cache_dirty = false;
//                }
//                return _cache;
//            }
//            set
//            {
//                if (value.Length > 5) throw new ByteParseException("Array is to long");
//                _cache = value;
//                _value = ReadVarInt(_cache);
//            }
//        }

//        public int Length { get { return Data.Length; } }

//        private int _value;
//        private byte[] _cache;
//        private bool _cache_dirty;

//        public VarInt(int val) : this()
//        {
//            Value = val;
//        }

//        public byte[] GeneratePacketData()
//        {
//            return Data;
//        }


//        public static implicit operator VarInt(int value) => new VarInt(value);
//        public static implicit operator int(VarInt value) => value.Value;

  



//    }
//}
