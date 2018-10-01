using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Stack_Machine.OpCodes;

namespace Stack_Machine
{
    class Program
    {

        static byte[] test = new byte[]
        {
            0b01000000 | LOAD, 0x20, 0x0, 0x0, 0x0, 0x9,
            0b01000000 | LOAD, 0x20, 0x0, 0x0, 0x0, 0x0,
            0b01000000 | LOAD, 0x20, 0x0, 0x0, 0x0, 0x1,
            ADD,
            PRINT,
            LT,
            0b01000000 | BRF, 0x20, 0x0, 0x0, 0x0, 0xC,
            HALT
        };



        static void Main(string[] args)
        {

            Machine vm = new Machine(test, 0, 300);
            Console.WriteLine("Raw binary dump: ");
            for (int i = 0; i < test.Length; i++)
            {
                Console.Write(test[i] + " ");
            }
            Console.WriteLine("\n");

            vm.Disassemble();
            Console.WriteLine("Execution:\n");

            vm.exec();

            Console.Read();
        }
    }
}
