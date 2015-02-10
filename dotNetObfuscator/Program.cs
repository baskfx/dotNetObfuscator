using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeTools;

namespace dotNetObfuscator
{
    class Program
    {
        static void Main(string[] args)
        {
            //<script>alert(String.fromCharCode('A'.charCodeAt(0) + 'B'.charCodeAt(0) - 'A'.charCodeAt(0)));</script>
            if (true)
            {
                string text = args.Length > 0 ? args[0] : "test";
                Obfuscator o = new Obfuscator();
                string res = o.Obfuscate(text, "7fkc83Rhjas61dfBc0xZgfspPs1nNf2EeЁйжпyyКйзЗХяЯбИпРдээвУЖ");
                Console.WriteLine(res);
            }
            string s = "" + (char)((((33 | 68) + (('W') - 0x59)) < (0x47 + ((0x49 + ((0x66 + (((char)0x5E) - 0x47)) - (35 | 35))) - 0x53)) ? ((32 | 66) + (('_') - 0x4D)) : (0x50 + (((44 | 44) + ((109 + (((char)0x4D) - 0x50)) - 83)) - 0x5C)))) + (char)(((0x59 + (93 + (('S') - 0x45))) < ((0x4D + (((49 | 50) + ((0x45 + ((84 + ((0x63 + ((92 + (('S') - 0x4B)) - 80)) - 0x49)) - (36 | 37))) - 0x59)) - 0x4C)) - 0x5A) ? (0x4A + ((95 + (('T') - 73)) - 84)) : (0x69 + (((char)78) - (27 | 55))))) + (char)((((0x74 + (((char)0x5B) - 72)) - 0x47) > (0x60 + ((0x63 + ((0x51 + (((33 | 66) + (((char)0x4C) - (26 | 52))) - (22 | 46))) - (28 | 58))) - 0x4F)) ? ((111 + ((107 + (('C') - 74)) - (26 | 53))) - (46 | 46)) : (0x5D + ((72 + ((91 + ((83 + ((68 + ((0x57 + ((0x61 + (((char)(25 | 50)) - (34 | 34))) - 0x5C)) - 0x46)) - 0x4D)) - 0x4B)) - (30 | 61))) - 0x4E)))) + (char)((((char)0x5B) < (98 + ((char)0x4D)) ? ((36 | 74) + (((char)0x5F) - 0x59)) : ((char)70)));


            //BinHelper bin = new BinHelper();
            //Console.WriteLine( BinHelper.int2bin(1027));
            //List<int> pos = BinHelper.findPos(3, 1027);
            //string a = BinHelper.MakeXfromY(22, 709);

            Console.ReadLine();
        }
    }
}
