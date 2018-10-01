//A note for future me: I need to think of a way to use pointers. They're going to be essential. But for now, I won't worry about them too much. 
//Also, this may or may not be worth writing an assembler for this
//I may end up creating a single array for globals and for the stack, so as to make memory addressing easier

namespace Stack_Machine
{
    public enum TYPES
    {
        INT = 0x00,
        FLOAT = 0x01
    }

    enum OpCode
    {
        HALT,
        LOAD,
        STORE,
        POP,
        CALL,
        RET,
        BR,
        BRT,
        BRF,
        PRINT,
        ADD,
        SUB, 
        MUL,
        DIV,
        LT,
        EQ
    }

    public static class OpCodes
    {

        public const byte HALT = (byte)OpCode.HALT;//Halts the program expects zero bytes after opcode
        public const byte LOAD = (byte)OpCode.LOAD;//Loads a value onto the stack
        public const byte STORE = (byte)OpCode.STORE;//Stores an integer in a memory location, either stack or global
        public const byte POP = (byte)OpCode.POP;//Pops the top element off of the stack
        public const byte CALL = (byte)OpCode.CALL;//Calls a function at the specified address
        //0 operand instruction
        public const byte RET = (byte)OpCode.RET;//Returns from a function call
        public const byte BR = (byte)OpCode.BR;//Branch to a specified address
        public const byte BRT = (byte)OpCode.BRT;//Branches to a specifed address if the element on the top of the stack is not zero
        public const byte BRF = (byte)OpCode.BRF;//Branches to a specified address if the element on the top of the stack is zero

        //These instructions are 0 operand instructions
        public const byte PRINT = (byte)OpCode.PRINT;//Outputs the top of the stack to the console window
        public const byte ADD = (byte)OpCode.ADD;//Pops the top two elements on the stack and adds them, then pushes the result
        public const byte SUB = (byte)OpCode.SUB;//Pops the top two elements on the stack and subtracts them, then pushes the result
        public const byte MUL = (byte)OpCode.MUL;//Pops the top two elements on the stack and multiplies them, then pushes the result
        public const byte DIV = (byte)OpCode.DIV;//Pops the top two elements on the stack and divides them, then pushes the result
        public const byte LT = (byte)OpCode.LT;//Checks if the top element is greater than the second element, and pushes either a zero or a one
        public const byte EQ = (byte)OpCode.EQ;//Checks if the top element is equal to the second element and pushes either a zero or a one
    }
}