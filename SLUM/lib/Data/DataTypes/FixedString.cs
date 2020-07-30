using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SLUM.lib.Data.DataTypes
{
    public class FixedString : VarString
    {
        public FixedString(string text) : base(text)
        {
        }

        public static implicit operator FixedString(string value) => new FixedString(value);
        public static implicit operator string(FixedString value) => value.Data;

    }
}
