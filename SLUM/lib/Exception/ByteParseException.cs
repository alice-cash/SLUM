using System;
using System.Collections.Generic;
using System.Text;

namespace SLUM.lib.Exception
{
    public class ByteParseException : System.Exception
    {
        public ByteParseException(string message) : base(message)
        {
        }

        public ByteParseException()
        {
        }

        public ByteParseException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

    }
}
