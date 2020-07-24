//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Text;

//namespace SLUM.lib.Data.DataTypes
//{
//    public class VarString: IData
//    {

//        private string _data;
//        private byte[] _cache;
//        private bool _cache_dirty = false;
//        public string Data
//        {
//            get { return _data; }
//            set { _data = value; _cache_dirty = true; }
//        }

//        public byte[] ByteData
//        {
//            get
//            {
//                if (_cache_dirty)
//                {
//                    _cache = UTF8Encoding.UTF8.GetBytes(Data);
//                    _cache_dirty = false;
//                }
//                return _cache;
//            }
//        }

//        public VarInt Length
//        {
//            get
//            {
//                if (_cache_dirty)
//                {
//                    _cache = UTF8Encoding.UTF8.GetBytes(Data);
//                    _cache_dirty = false;
//                }
//                return _cache.Length;
//            }
//        }

//        public byte[] GeneratePacketData()
//        {
//            ByteWriter bw = new ByteWriter();
//            bw.Write(Length);
//            bw.Write(ByteData);
//            return bw.GetArray();
//        }

//        public VarString() { }

//        public VarString(string text)
//        {
//            Data = text;
//        }


//        public static implicit operator VarString(string value) => new VarString(value);
//        public static implicit operator string(VarString value) => value.Data;



//    }
//}
