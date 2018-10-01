using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Stack_Machine
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Value
    {
        [FieldOffset(4)]
        public TYPES type;

        [FieldOffset(0)]
        public byte Byte0;
        [FieldOffset(1)]
        public byte Byte1;
        [FieldOffset(2)]
        public byte Byte2;
        [FieldOffset(3)]
        public byte Byte3;

        [FieldOffset(0)]
        public int INT;
        [FieldOffset(0)]
        public float FLOAT;

        public Value(int value)
        {
            type = TYPES.INT;
            Byte0 = 0;
            Byte1 = 0;
            Byte2 = 0;
            Byte3 = 0;
            FLOAT = 0;
            INT = value;
        }

        public Value(float value)
        {
            type = TYPES.INT;
            Byte0 = 0;
            Byte1 = 0;
            Byte2 = 0;
            Byte3 = 0;
            INT = 0;
            FLOAT = value;
        }

        public Value(TYPES typ)
        {
            type = typ;
            Byte0 = 0;
            Byte1 = 0;
            Byte2 = 0;
            Byte3 = 0;
            FLOAT = 0;
            INT = 0;
        }
    }
}
