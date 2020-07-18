using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SLUM.lib.Exception;

namespace SLUM.lib.Data.DataTypes
{
    public static class VarInt
    {
        
        public static int readVarInt(BinaryReader bs)
        {
            if (bs == null) throw new ArgumentNullException(nameof(bs));
            
            int numRead = 0;
            int result = 0;
            byte read;
            do
            {
                read = bs.ReadByte();
                int value = (read & 0b01111111);
                result |= (value << (7 * numRead));

                numRead++;
                if (numRead > 5)
                {
                    throw new ByteParseException("VarInt is too big");
                }
            } while ((read & 0b10000000) != 0);

            return result;
        }

        public static void writeVarInt(int value, BinaryWriter bw)
        {
            if (bw == null) throw new ArgumentNullException(nameof(bw));

            uint workingValue = (uint)value;
            do
            {
                byte temp = (byte)(workingValue & 0b01111111);

                workingValue >>= 7;
                if (workingValue != 0)
                {
                    temp |= 0b10000000;
                }
                bw.Write(temp);
            } while (workingValue != 0);
        }
    }
}
