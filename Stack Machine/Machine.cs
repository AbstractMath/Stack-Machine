using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Stack_Machine.OpCodes;

//Okay, so the addressing encoding works perfectly now
//TODO: Define big endian types and also little endian

namespace Stack_Machine
{
    enum AddressingMode
    {
        REGISTER,
        DIRECT,
        INDIRECT,
        INDIRECTDISPLACED,
        INDIRECTSCALED,
        LITERALFLOAT,
        LITERALINTEGER
    }// I need a way of getting literal values for the jmp and call instructions

    public class Machine
    {
        byte[] code;
        Value[] Memory;//stores both stack and global memory. Addressing to this memory may or may not be inside the stack. 

        int ip;
        int fp;
        int sp = -1;
        Value sta
        {
            get { return Memory[sp]; }
        }
        Value stb
        {
            get { return sp > 0 ? Memory[sp - 1] : new Value(0x0); }
        }
        Value stc
        {
            get { return sp > 1 ? Memory[sp - 2] : new Value(0x0); }
        }
        Value std
        {
            get { return sp > 2 ? Memory[sp - 3] : new Value(0x0); }
        }

        Value peek()
        {
            return Memory[sp];
        }

        void push(Value val)
        {
            Memory[++sp] = val;
        }

        Value pop()
        {
            return Memory[sp--];
        }

        void printStack()
        {
            for (int i = 0; i <= sp; i++)
            {
                Console.Write((Memory[i].type == TYPES.INT ? Memory[i].INT : Memory[i].FLOAT) + ", ");
            }
            Console.WriteLine();
        }

        public Machine(byte[] code, int main, int dataSize)
        {
            this.code = code;
            this.ip = main;
            Memory = new Value[dataSize];
        }

        public void Disassemble()
        {
            int i = 0;

            while (i < code.Length)
            {
                byte opcode = (byte)(code[i] & 0x3F);
                byte ldfrom = (byte)((code[i] & 0xC0) >> 6);
                Console.Write("Address " + i + ":");
                i++;

                switch (opcode)
                {
                    case HALT:
                        {
                            Console.WriteLine("HALT");
                            break;
                        }
                    case LOAD:
                        {
                            Console.Write("LOAD ");
                            byte Addressing = code[i++];
                            byte reg1 = (byte)((Addressing & 0b11100000) >> 5);
                            byte reg2 = (byte)((Addressing & 0b00011100) >> 2);
                            byte Scale = (byte)Math.Pow(2, (Addressing & 0b00000011));
                            AddressingMode mod = DecodeAddressing(ldfrom, reg1, reg2);

                            printAddressingMode(ref i, mod, reg1, reg2, Scale);

                            break;
                        }
                    case STORE:
                        {
                            Console.Write("STORE ");
                            byte Addressing = code[i++];
                            byte reg1 = (byte)((Addressing & 0b11100000) >> 5);
                            byte reg2 = (byte)((Addressing & 0b00011100) >> 2);
                            byte Scale = (byte)Math.Pow(2, (Addressing & 0b00000011));
                            AddressingMode mod = DecodeAddressing(ldfrom, reg1, reg2);

                            printAddressingMode(ref i, mod, reg1, reg2, Scale);

                            break;
                        }
                    case POP:
                        {
                            Console.WriteLine("POP");
                            break;
                        }
                    case CALL:
                        {
                            Console.Write("CALL ");
                            byte Addressing = code[i++];
                            byte nargs = code[i++];
                            byte reg1 = (byte)((Addressing & 0b11100000) >> 5);
                            byte reg2 = (byte)((Addressing & 0b00011100) >> 2);
                            byte Scale = (byte)Math.Pow(2, (Addressing & 0b00000011));
                            AddressingMode mod = DecodeAddressing(ldfrom, reg1, reg2);
                            Console.Write(nargs + ", ");
                            printAddressingMode(ref i, mod, reg1, reg2, Scale);

                            break;
                        }
                    case RET:
                        {
                            Console.WriteLine("RET");
                            break;
                        }
                    case BR:
                        {
                            Console.Write("BR ");
                            byte Addressing = code[i++];
                            byte reg1 = (byte)((Addressing & 0b11100000) >> 5);
                            byte reg2 = (byte)((Addressing & 0b00011100) >> 2);
                            byte Scale = (byte)Math.Pow(2, (Addressing & 0b00000011));
                            AddressingMode mod = DecodeAddressing(ldfrom, reg1, reg2);
                            printAddressingMode(ref i, mod, reg1, reg2, Scale);
                            break;
                        }
                    case BRT:
                        {
                            Console.Write("BRT ");
                            byte Addressing = code[i++];
                            byte reg1 = (byte)((Addressing & 0b11100000) >> 5);
                            byte reg2 = (byte)((Addressing & 0b00011100) >> 2);
                            byte Scale = (byte)Math.Pow(2, (Addressing & 0b00000011));
                            AddressingMode mod = DecodeAddressing(ldfrom, reg1, reg2);
                            printAddressingMode(ref i, mod, reg1, reg2, Scale);
                            break;
                        }
                    case BRF:
                        {
                            Console.Write("BRF ");
                            byte Addressing = code[i++];
                            byte reg1 = (byte)((Addressing & 0b11100000) >> 5);
                            byte reg2 = (byte)((Addressing & 0b00011100) >> 2);
                            byte Scale = (byte)Math.Pow(2, (Addressing & 0b00000011));
                            AddressingMode mod = DecodeAddressing(ldfrom, reg1, reg2);
                            printAddressingMode(ref i, mod, reg1, reg2, Scale);
                            break;
                        }
                    case PRINT:
                        {
                            Console.WriteLine("PRINT");
                            break;
                        }
                    case ADD:
                        {
                            Console.WriteLine("ADD");
                            break;
                        }
                    case SUB:
                        {
                            Console.WriteLine("SUB");
                            break;
                        }
                    case MUL:
                        {
                            Console.WriteLine("MUL");
                            break;
                        }
                    case DIV:
                        {
                            Console.WriteLine("DIV");
                            break;
                        }
                    case EQ:
                        {
                            Console.WriteLine("EQ");
                            break;
                        }
                    case LT:
                        {
                            Console.WriteLine("LT");
                            break;
                        }
                }
            }
        }

        void printAddressingMode(ref int i, AddressingMode mod, byte reg1, byte reg2, byte Scale)
        {
            if (mod == AddressingMode.REGISTER)
            {
                printRegisterName(reg1);
                Console.WriteLine();
            }
            else if (mod == AddressingMode.DIRECT)
            {
                Value addr = new Value(TYPES.INT);
                addr.Byte3 = code[i++];
                addr.Byte2 = code[i++];
                addr.Byte1 = code[i++];
                addr.Byte0 = code[i++];

                Console.WriteLine("[" + addr.INT + "]");
            }
            else if (mod == AddressingMode.INDIRECT)
            {
                Console.Write("[");
                printRegisterName(reg1);
                Console.Write("]");
                Console.WriteLine();
            }
            else if (mod == AddressingMode.INDIRECTSCALED)
            {
                Console.Write("[");
                printRegisterName(reg1);
                Console.Write(" + " + Scale + " * ");
                printRegisterName(reg2);
                Console.Write("]");
                Console.WriteLine();
            }
            else if (mod == AddressingMode.INDIRECTDISPLACED)
            {
                Value displace = new Value(TYPES.INT);
                displace.Byte3 = code[i++];
                displace.Byte2 = code[i++];
                displace.Byte1 = code[i++];
                displace.Byte0 = code[i++];

                Console.Write("[");
                printRegisterName(reg1);
                Console.WriteLine(" + " + displace.INT + "]");
            }
            else if (mod == AddressingMode.LITERALINTEGER)
            {
                Value toPush = new Value(TYPES.INT);
                toPush.Byte3 = code[i++];
                toPush.Byte2 = code[i++];
                toPush.Byte1 = code[i++];
                toPush.Byte0 = code[i++];

                Console.Write(toPush.INT);
                Console.WriteLine();
            }
            else
            {

                Value toPush = new Value(TYPES.FLOAT);
                toPush.Byte3 = code[i++];
                toPush.Byte2 = code[i++];
                toPush.Byte1 = code[i++];
                toPush.Byte0 = code[i++];

                Console.Write(toPush.FLOAT);
                Console.WriteLine();
            }
        }
        
        public void printRegisterName(byte regCode)
        {
            switch (regCode)
            {
                case 0: Console.Write("%IP"); break;
                case 1: Console.Write("%FP"); break;
                case 2: Console.Write("%SP"); break;
                case 3: Console.Write("%STA"); break;
                case 4: Console.Write("%STB"); break;
                case 5: Console.Write("%STC"); break;
                case 6: Console.Write("%STD"); break;
                default: Console.Write("%???"); break;
            }
        }

        public Value getRegister(byte regCode)
        {
            switch (regCode)
            {
                case 0: return new Value(ip);
                case 1: return new Value(fp);
                case 2: return new Value(sp);
                case 3: return sta;
                case 4: return stb;
                case 5: return stc;
                case 6: return std;
                default: throw new Exception("Unknown register code in byte at ip " + ip);
            }
        }

        public void setRegister(byte regCode, Value value)
        {
            switch (regCode)
            {
                case 0: ip = value.INT; break;
                case 1: fp = value.INT; break;
                case 2: sp = value.INT; break;
                case 3:
                    {//sta
                        Memory[sp] = value;
                        break;
                    }
                case 4:
                    {//stb
                        if (sp > 0)
                        {
                            Memory[sp - 1] = value;
                        }
                        break;
                    }
                case 5:
                    {//stc
                        if (sp > 1)
                        {
                            Memory[sp - 2] = value;
                        }
                        break;
                    }
                case 6:
                    {//std
                        if (sp > 2)
                        {
                            Memory[sp - 3] = value;
                        }
                        break;
                    }
            }
        }

        AddressingMode DecodeAddressing(byte LDFRM, byte Reg1, byte Reg2)//Returns the data retrieved from the addressing and operand
        {
            if (LDFRM == 0)
            {
                return AddressingMode.REGISTER;
            }
            else if (LDFRM == 1)
            {
                //I think in this case, if reg1 is 0, do the direct addressing, otherwise if reg1 = 1, it's an integer literal, and if it's 2, it's a floating point literal
                if (Reg1 == 0)
                {
                    return AddressingMode.DIRECT;
                }
                else if (Reg1 == 1)
                {
                    return AddressingMode.LITERALINTEGER;
                }
                else if (Reg1 == 2)
                {
                    return AddressingMode.LITERALFLOAT;
                }
                else
                {
                    throw new Exception("Uknown type code at instruction " + ip);
                }
                
            }
            else if (LDFRM == 2)
            {
                return AddressingMode.INDIRECT;
            }
            else
            {
                if (Reg1 == Reg2) { return AddressingMode.INDIRECTDISPLACED; }
                return AddressingMode.INDIRECTSCALED;
            }
        }

        int AddressingJmp(AddressingMode mod, byte reg1, byte reg2, byte Scale)
        {
            if (mod == AddressingMode.REGISTER)
            {
                //Branch to the code address in whatever register
                return getRegister(reg1).INT;
            }
            else if (mod == AddressingMode.DIRECT)
            {
                //Branch to the code address specified at this point in memory
                int addr = code[ip++] << 24;
                addr = addr | (code[ip++] << 16);
                addr = addr | (code[ip++] << 8);
                addr = addr | code[ip++];

                return Memory[addr].INT;
            }
            else if (mod == AddressingMode.INDIRECT)
            {
                return Memory[getRegister(reg1).INT].INT;
            }
            else if (mod == AddressingMode.INDIRECTSCALED)
            {
                return Memory[getRegister(reg1).INT + Scale * getRegister(reg2).INT].INT;
            }
            else if (mod == AddressingMode.INDIRECTDISPLACED)
            {
                int displacement = code[ip++] << 24;
                displacement = displacement | (code[ip++] << 16);
                displacement = displacement | (code[ip++] << 8);
                displacement = displacement | code[ip++];

                return Memory[getRegister(reg1).INT + displacement].INT;
            }
            else if (mod == AddressingMode.LITERALINTEGER)
            {
                //Jump to this address in the code memory

                int addr = code[ip++] << 24;
                addr = addr | (code[ip++] << 16);
                addr = addr | (code[ip++] << 8);
                addr = addr | code[ip++];

                return addr;
            }
            else
            {
                throw new Exception("Invalid addressing mode for branching instruction on instruction " + ip);
            }
        }

        public void exec()
        {
            while (ip < code.Length)
            {
                byte opcode = (byte)(code[ip] & 0x3F);
                byte ldfrom = (byte)((code[ip] & 0xC0) >> 6);
                ip++;
                //printStack();

                switch (opcode)
                {
                    case HALT:
                        {
                            Console.WriteLine("Stop");
                            return;//Stop the program from running
                        }
                    case LOAD:
                        {
                            byte Addressing = code[ip++];
                            byte reg1 = (byte)((Addressing & 0b11100000) >> 5);
                            byte reg2 = (byte)((Addressing & 0b00011100) >> 2);
                            byte Scale = (byte)Math.Pow(2, (Addressing & 0b00000011));
                            AddressingMode mod = DecodeAddressing(ldfrom, reg1, reg2);

                            if (mod == AddressingMode.REGISTER)
                            {
                                //Use reg1
                                push(getRegister(reg1));
                            }
                            else if (mod == AddressingMode.DIRECT)
                            {
                                Value addr = new Value(TYPES.INT);
                                addr.Byte3 = code[ip++];
                                addr.Byte2 = code[ip++];
                                addr.Byte1 = code[ip++];
                                addr.Byte0 = code[ip++];
                                push(Memory[addr.INT]);
                            }
                            else if (mod == AddressingMode.INDIRECT)
                            {
                                //use the value in reg1 as an address 
                                push(Memory[getRegister(reg1).INT]);
                            }
                            else if (mod == AddressingMode.INDIRECTSCALED)
                            { 
                                push(Memory[getRegister(reg1).INT + Scale * getRegister(reg2).INT]);
                            }
                            else if (mod == AddressingMode.INDIRECTDISPLACED)
                            {
                                Value displace = new Value(TYPES.INT);
                                displace.Byte3 = code[ip++];
                                displace.Byte2 = code[ip++];
                                displace.Byte1 = code[ip++];
                                displace.Byte0 = code[ip++];

                                push(Memory[displace.INT + getRegister(reg1).INT]);
                            }
                            else if (mod == AddressingMode.LITERALINTEGER)
                            {
                                Value toPush = new Value(TYPES.INT);
                                toPush.Byte3 = code[ip++];
                                toPush.Byte2 = code[ip++];
                                toPush.Byte1 = code[ip++];
                                toPush.Byte0 = code[ip++];

                                push(toPush);
                            }
                            else
                            {
                                Value toPush = new Value(TYPES.FLOAT);
                                toPush.Byte3 = code[ip++];
                                toPush.Byte2 = code[ip++];
                                toPush.Byte1 = code[ip++];
                                toPush.Byte0 = code[ip++];

                                push(toPush);
                            }

                            break;
                        }
                    case STORE:
                        {
                            byte Addressing = code[ip++];
                            byte reg1 = (byte)((Addressing & 0b11100000) >> 5);
                            byte reg2 = (byte)((Addressing & 0b00011100) >> 2);
                            byte Scale = (byte)Math.Pow(2, (Addressing & 0b00000011));
                            AddressingMode mod = DecodeAddressing(ldfrom, reg1, reg2);
                            Value val = pop();

                            if (mod == AddressingMode.REGISTER)
                            {
                                setRegister(reg1, val);
                            }
                            else if (mod == AddressingMode.DIRECT)
                            {
                                Value addr = new Value(TYPES.INT);
                                addr.Byte3 = code[ip++];
                                addr.Byte2 = code[ip++];
                                addr.Byte1 = code[ip++];
                                addr.Byte0 = code[ip++];

                                Memory[addr.INT] = val;
                            }
                            else if (mod == AddressingMode.INDIRECT)
                            {
                                Memory[getRegister(reg1).INT] = val;
                            }
                            else if (mod == AddressingMode.INDIRECTSCALED)
                            {
                                Memory[getRegister(reg1).INT + Scale * getRegister(reg2).INT] = val;
                            }
                            else if (mod == AddressingMode.INDIRECTDISPLACED)
                            {
                                Value displace = new Value(TYPES.INT);
                                displace.Byte3 = code[ip++];
                                displace.Byte2 = code[ip++];
                                displace.Byte1 = code[ip++];
                                displace.Byte0 = code[ip++];

                                Memory[displace.INT + getRegister(reg1).INT] = val;
                            }
                            else if (mod == AddressingMode.LITERALINTEGER || mod == AddressingMode.LITERALFLOAT)
                            {
                                throw new Exception("Invalid addressing mode for STORE instruction at instruction " + ip);
                            }

                            break;
                        }
                    case POP:
                        {
                            pop();
                            break;
                        }
                    case CALL:
                        {
                            //Instruction structure example: CALL, nargs, addressingbyte, [0 or 4 byte argument]
                            //Takes a 1 byte argument list
                            int nargs = code[ip++];
                            Value wrappedArgs = new Value(TYPES.INT);

                            byte Addressing = code[ip++];
                            byte reg1 = (byte)((Addressing & 0b11100000) >> 5);
                            byte reg2 = (byte)((Addressing & 0b00011100) >> 2);
                            byte Scale = (byte)Math.Pow(2, (Addressing & 0b00000011));
                            AddressingMode mod = DecodeAddressing(ldfrom, reg1, reg2);

                            wrappedArgs.INT = nargs;
                            Value wrappedFp = new Value(TYPES.INT);
                            wrappedFp.INT = fp;
                            Value wrappedIp = new Value(TYPES.INT);
                            wrappedIp.INT = ip;

                            push(wrappedArgs);
                            push(wrappedFp);
                            push(wrappedIp);
                            fp = sp;

                            ip = AddressingJmp(mod, reg1, reg2, Scale);
                            break;
                        }
                    case RET:
                        {
                            Value returnVal = pop();
                            sp = fp;
                            ip = pop().INT;
                            fp = pop().INT;
                            int nargs = pop().INT;
                            sp -= nargs;
                            push(returnVal);

                            break;
                        }
                    case BR:
                        {
                            byte Addressing = code[ip++];
                            byte reg1 = (byte)((Addressing & 0b11100000) >> 5);
                            byte reg2 = (byte)((Addressing & 0b00011100) >> 2);
                            byte Scale = (byte)Math.Pow(2, (Addressing & 0b00000011));
                            AddressingMode mod = DecodeAddressing(ldfrom, reg1, reg2);

                            ip = AddressingJmp(mod, reg1, reg2, Scale);

                            break;
                        }
                    case BRT:
                        {
                            //Take the next two bytes as an address (I should make this a function, probably)
                            byte Addressing = code[ip++];
                            byte reg1 = (byte)((Addressing & 0b11100000) >> 5);
                            byte reg2 = (byte)((Addressing & 0b00011100) >> 2);
                            byte Scale = (byte)Math.Pow(2, (Addressing & 0b00000011));
                            AddressingMode mod = DecodeAddressing(ldfrom, reg1, reg2);

                            if (pop().INT != 0)//I need to change these to look at a register
                            {
                                ip = AddressingJmp(mod, reg1, reg2, Scale);
                            }
                            break;
                        }
                    case BRF:
                        {
                            byte Addressing = code[ip++];
                            byte reg1 = (byte)((Addressing & 0b11100000) >> 5);
                            byte reg2 = (byte)((Addressing & 0b00011100) >> 2);
                            byte Scale = (byte)Math.Pow(2, (Addressing & 0b00000011));
                            AddressingMode mod = DecodeAddressing(ldfrom, reg1, reg2);

                            if (pop().INT == 0)
                            {
                                ip = AddressingJmp(mod, reg1, reg2, Scale);
                            }
                            break;
                        }
                    case PRINT:
                        {
                            Value toOut = peek();
                            Console.WriteLine(toOut.type == TYPES.INT ? toOut.INT : toOut.FLOAT);
                            break;
                        }
                    case ADD:
                        {
                            Value a = pop();
                            Value b = pop();

                            float result = (a.type == TYPES.INT ? a.INT : a.FLOAT) + (b.type == TYPES.INT ? b.INT : b.FLOAT);
                            Value toPush;

                            if (a.type == TYPES.FLOAT || b.type == TYPES.FLOAT)
                            {
                                toPush = new Value(TYPES.FLOAT);
                                toPush.FLOAT = result;
                            }
                            else
                            {
                                toPush = new Value(TYPES.INT);
                                toPush.INT = (int)result;
                            }

                            push(toPush);

                            break;
                        }
                    case SUB:
                        {
                            Value a = pop();
                            Value b = pop();

                            float result = (b.type == TYPES.INT ? b.INT : b.FLOAT) - (a.type == TYPES.INT ? a.INT : a.FLOAT);
                            Value toPush;

                            if (a.type == TYPES.FLOAT || b.type == TYPES.FLOAT)
                            {
                                toPush = new Value(TYPES.FLOAT);
                                toPush.FLOAT = result;
                            }
                            else
                            {
                                toPush = new Value(TYPES.INT);
                                toPush.INT = (int)result;
                            }

                            push(toPush);

                            break;
                        }
                    case MUL:
                        {
                            Value a = pop();
                            Value b = pop();

                            float result = (b.type == TYPES.INT ? b.INT : b.FLOAT) * (a.type == TYPES.INT ? a.INT : a.FLOAT);
                            Value toPush;

                            if (a.type == TYPES.FLOAT || b.type == TYPES.FLOAT)
                            {
                                toPush = new Value(TYPES.FLOAT);
                                toPush.FLOAT = result;
                            }
                            else
                            {
                                toPush = new Value(TYPES.INT);
                                toPush.INT = (int)result;
                            }

                            push(toPush);

                            break;
                        }
                    case DIV:
                        {
                            Value a = pop();
                            Value b = pop();

                            float result = (a.type == TYPES.INT ? a.INT : a.FLOAT) / (b.type == TYPES.INT ? b.INT : b.FLOAT);
                            Value toPush;

                            if (a.type == TYPES.FLOAT || b.type == TYPES.FLOAT)
                            {
                                toPush = new Value(TYPES.FLOAT);
                                toPush.FLOAT = result;
                            }
                            else
                            {
                                toPush = new Value(TYPES.INT);
                                toPush.INT = (int)result;
                            }

                            push(toPush);

                            break;
                        }
                    case LT:
                        {
                            Value toPush = new Value(TYPES.INT);//This result needs to get stored inside a register
                            Value a = peek();
                            Value b = stb;

                            if ((a.type == TYPES.INT ? a.INT : a.FLOAT) > (b.type == TYPES.INT ? b.INT : b.FLOAT))
                            {
                                toPush.INT = 1;
                            }
                            else
                            {
                                toPush.INT = 0;
                            }

                            push(toPush);

                            break;
                        }
                    case EQ:
                        {
                            Value toPush = new Value(TYPES.INT);//This result needs to get stored inside a register
                            Value a = pop();
                            Value b = pop();

                            if ((a.type == TYPES.INT ? a.INT : a.FLOAT) == (b.type == TYPES.INT ? b.INT : b.FLOAT))
                            {
                                toPush.INT = 1;
                            }
                            else
                            {
                                toPush.INT = 0;
                            }
                            push(toPush);

                            break;
                        }

                }
            }
        }
    }
}