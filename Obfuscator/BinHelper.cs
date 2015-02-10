using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTools
{
    class BinHelper
    {
        public static Random rnd = new Random();
        public static string int2bin(long x)
        {
            string res = Convert.ToString(x, 2);
            return res;
        }

        /// <summary>
        /// Ищем вхождения x в y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static List<int> findPos(long x, long y)
        {
            List<int> res = new List<int>();
            string sX = int2bin(x);
            string sY = int2bin(y);
            for (int i = 0; i <= sY.Length - sX.Length; i++)
            {
                if (sY.Substring(i, sX.Length) == sX)
                    res.Add(i);
            }
            return res;
        }

        public static string makeANDmask(string x, string y)
        {
            string res = "";
            for (int i = 0; i < y.Length - x.Length; i++)
                x = "0" + x;

            //if (x.Length != y.Length) throw new Exception("wrong length");
            for (int i = 0; i < x.Length; i++)
                if (x[i] == '1')
                    res += "1";
                else
                    res += rnd.Next(2).ToString();
            return res;
        }

        public static string clearLeft(string x)
        {
            return x;
        }

        public static string clearRight(string x)
        {
            return x;
        }

        public static string MakeXfromY(long x, long y)
        {
            //10110 из 1011000101
            string res = "";
            string sX = int2bin(x);
            string sY = "" + int2bin(y);
            int pos = rnd.Next(sY.Length) + 1;
            pos = 6;
            //>>
            if (pos >= sX.Length)
            {
                long x2 = x >> (pos - sX.Length);
                //string y2 = sY.Substring(0, sY.Length - (pos - sX.Length));//101100010
                //string maskAND = makeANDmask(sX, y2);//1110111

                res = "";
                for (int i = 0; i < sY.Length; i++)
                {
                    if ((i >= sY.Length - pos) && (i < sY.Length - (pos - sX.Length)))
                    {
                        int xpos = i - (sY.Length - pos);
                        if (sX[xpos] == '1')
                            res += "1";
                        else
                            res += rnd.Next(2).ToString();
                    }
                    else res += "1";
                }
            }

            //y:  1011000101 |
            //res:0000101100
            // >> 6
            //x:10110
            return res;
        }
    }
}
