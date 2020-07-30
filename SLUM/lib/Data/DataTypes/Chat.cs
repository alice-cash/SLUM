using System;
using System.Collections.Generic;
using System.Text;

namespace SLUM.lib.Data.DataTypes
{
    
    public class Chat : VarString
    {

        public Chat(string text) : base()
        {
            // Data = "{\"text\": \"" + text + "\" }";
            Data = text;
        }

    }
}
