using System;
using System.Collections.Generic;
using System.Text;
using StormLib.Exceptions;

namespace SLUM.lib.Exception
{
    public class ByteParseException : StormLibException
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
