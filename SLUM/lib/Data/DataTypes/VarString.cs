using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SLUM.lib.Data.DataTypes
{
    public class VarString : IDataType
    {

        protected string _data;
        public string Data { get { return _data; } set { _data = checkString(value); } }

        public int Length { get { return _data.Length; } }
        public int MaxLength { get; set; }

        public VarString()
        {
            MaxLength = 32767;
            _data = "";
        }

        public VarString(string text) : this()
        {
            _data = text;
        }

        protected virtual string checkString(string data)
        {
            if (!checkLength(data))
                return data.Substring(0, MaxLength);
            return data;
        }
        protected bool checkLength(string data)
        {
            return data.Length <= MaxLength;
        }

        public static implicit operator VarString(string value) => new VarString(value);
        public static implicit operator string(VarString value) => value.Data;

    }
}
