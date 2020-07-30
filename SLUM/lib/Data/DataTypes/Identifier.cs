using System;
using System.Collections.Generic;
using System.Text;

namespace SLUM.lib.Data.DataTypes
{
    public class Identifier : VarString
    {
        private string _namespace, _name;
        private readonly string _allowedChars = "01​​234​5​6​78​9abcdefghijklmnopqrstuvwxyz-_";

        
        
        public string Namespace { get { return _namespace; } set { _namespace = stripString(value); _data = ToString(); } }
        public string Name { get { return _name; } set { _name = value; _data = ToString(); } }

         
        private string stripString(string value)
        {
            StringBuilder sb = new StringBuilder(value.Length);
            foreach(char c in value)
            {
                if (_allowedChars.Contains(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }


        protected override string checkString(string data)
        {
            int pos = data.IndexOf(':');
            if (pos == -1)
                throw new ArgumentOutOfRangeException("data", "String is not a proper identifier in the form of 'namespace:name'");
            string ns = stripString(data.Substring(0, pos));
            string name = data.Substring(pos);

            return base.checkString(string.Format("{0}:{1}",ns,name));
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", _namespace, _name);
        }
    }
}
