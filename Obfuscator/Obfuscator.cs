using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CodeTools
{
    public interface IObfuscate
    {
    }

    public class Obfuscator
    {
        public static Random rnd = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
        public static string signStr;

        public class CharObf
        {
            private string result;

            public string int2char(int i)
            {
                return "";
            }

            private string ShowHex(int x)
            {
                return String.Format("0x{0:X2}", x);
            }

            private string ShowDec(int x)
            {
                int x1 = x / rnd.Next(2, 4);
                int x2 = x - x1;
                return
                    rnd.NextDouble() < .5 ?
                    x.ToString() :
                    string.Format("({0} + {1})", x1, x2);
            }

            /// <summary>
            /// Представляет число x в виде десятичного, либо 16-ричного числа
            /// </summary>
            /// <param name="x"></param>
            /// <returns></returns>
            private string ShowNumber(int x)
            {
                return
                    rnd.NextDouble() < .5 ? ShowHex(x) : ShowDec(x);
                //int decAgain = int.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
            }

            private string ShowChar1(int c)
            {
                return "'" + (char)c + "'";
            }

            private string ShowChar2(int c)
            {
                return "(char)" + ShowNumber(c);
            }

            private string ShowChar3(int c)
            {
                for (int i = 0; i < signStr.Length; i++)
                    if ((char)c == signStr[i])
                        return "{VARIABLE}[" + i + "]";
                return "";
            }

            /// <summary>
            /// Возвращает c в виде комбинации чисел в диапазоне от 65 до 90
            /// Если c лежит в этом диапазоне, то оно и возвращается
            /// </summary>
            /// <param name="c"></param>
            /// <returns></returns>
            private string Make65_90(int c)
            {
                //return c.ToString();

                string res = " (";
                if (c < 65)
                {
                    int d = rnd.Next(65, 92) + 1;
                    res += Make65_90(c + d).ToString() + " - " + ShowNumber(d);
                }
                else
                    if (c > 65 + 30)
                    {
                        int d = rnd.Next(65, 90 + 27) + 1;
                        res += ShowNumber(d) + " + " + Make65_90(c - d);
                    }
                    else
                    {
                        string c3 = ShowChar3(c);
                        if (c3 != "") 
                            res += c3;
                        else
                            res += rnd.Next(2) < 1 ? ShowChar1(c) : ShowChar2(c);
                    }
                return res + ") ";
            }

            /// <summary>
            /// создаём булевое выражение
            /// </summary>
            /// <param name="MakeTrue"></param>
            /// <returns></returns>
            protected string MakeBoolExpression(bool MakeTrue)
            {
                int n1 = rnd.Next(0, 100);
                int n2 = rnd.Next(n1 + 1, 200);
                if (MakeTrue)
                    return
                        rnd.NextDouble() < .5 ?
                        string.Format(" {1} > {0} ", Make65_90(n1), Make65_90(n2)) :
                        string.Format(" {0} < {1} ", Make65_90(n1), Make65_90(n2));
                else
                    return
                        rnd.NextDouble() < .5 ?
                        string.Format(" {0} > {1} ", Make65_90(n1), Make65_90(n2)) :
                        string.Format(" {1} < {0} ", Make65_90(n1), Make65_90(n2));
            }

            protected string Show(int c)
            {
                int r = rnd.Next(0, 10);
                string pattern = "";
                if (r < 0) pattern = "(char)({0})";
                else pattern = "({0} ? {1} : {2})";
                string s = "";

                if (rnd.NextDouble() < 0.5)
                    s = string.Format(pattern, MakeBoolExpression(true), Make65_90(c), Make65_90(rnd.Next(100)));
                else
                    s = string.Format(pattern, MakeBoolExpression(false), Make65_90(rnd.Next(100)), Make65_90(c));

                return s;
            }
            
            /// <summary>
            /// Кодируем символ c последовательностью выражений
            /// </summary>
            /// <param name="c">кодируемый символ</param>
            /// <param name="p1">вероятность того, что символ остаётся неизменным</param>
            /// <param name="p2">[p1..p2] - вероятность знака "-", p > p2 - знак "+" в выражении</param>
            /// <returns></returns>
            private int ObfuscateChar(int c, double p1, double p2)
            {
                double r = rnd.NextDouble();
                result += " (";
                if (r < p1)
                {
                    result += Show(c) + ") ";
                    return c;
                }
                else if (r < p2)
                {
                    int R = rnd.Next(65, 90) + 1;
                    result += (Show(R) + "-");
                    c = R - ObfuscateChar(R - c, p1, p2);
                    result += ") ";
                    return c;
                }
                else
                {
                    int R = rnd.Next(65, 90) + 1;
                    result += Show(R) + "+";
                    c = R + ObfuscateChar(c - R, p1, p2);
                    result += ") ";
                    return c;
                }
            }

            public string Obfuscate2str(int c, double p1, double p2)
            {
                result = "";
                ObfuscateChar(c, p1, p2);
                return result;
            }
        }

        protected int ObfuscateChar2(int c)
        {
            return c;
        }

        //plasticclub
        /*static string o(string s)
        {
            string result = "";
            int Ox7c1f = 1;
            bool _______ = ((1 << 4 | 0xff) & (382 + 647)) == 5;
            bool aаааaааaа = 178 < (short)(0x7c1f) << 0x3c4;
            bool xinhuanet = 178 < (short)(0x7c1f) << 0x3c4;
            bool pdbqpqbdp = 'c' < (short)(0x7c1f) << 0x31c;
            bool Ill1I1lO0 = 'и' < (short)(0x7c1f) << 0x34c;
            bool EЕEEЕЕEЕE = 'и' < (long)(0x7c1f) << 0x34c;
            bool ______ = _______ == (("3" + (0xff - '1')).Length != _______.ToString().Length);
            string host = string.Format("\0{0}[\0{3}\0[{6}i-{2}{1}[\0{5}{4}\0{6}[{2}h{0}", (char)(65), (char)(0x41)
                           , (char)(61 + (!______).ToString().Length +
                           2), (char)('k' + ((______).ToString().Length - (_______).ToString().Length) * (1 << 2 | 1)), (char)0
                           , (char)('o' + ((______ && _______) ? 17 << 3 | 6 : 298 >> 8)), !_______ ? '\0' : (char)(90 - 0x6)).Replace("\0", "").Replace("[", "");
            string ext = (char)(0x41) + string.Format("s{0}{1}", (char)('l' +
                (!______).ToString().Length), ("" + (char)(90 - 2)).ToLower());
            return host;

            return result;
        }
        */

        public static int[] RandomShuffle(int[] list)
        {
            Random r = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
            int[] shuffle = new int[list.Length];
            list.CopyTo(shuffle, 0);
            for (int i = 2; i < list.Length; ++i)
            {
                int temp = shuffle[i];
                int nextRandom = r.Next(i - 1);
                shuffle[i] = shuffle[nextRandom];
                shuffle[nextRandom] = temp;
            }
            return shuffle;
        }

        public string Obfuscate(string text, string ss)
        {
            signStr = ss;
            string _ = "";

            using (StreamReader rd = new StreamReader("data.txt"))
            _ = rd.ReadLine();
            string s = String.Format("{4}{6}{0}{5}{2}{1}{7}{3}"
                , (char)((((0x4F + ((0x6C + (((char)0x53) - 0x9)) - (25 + 51))) - 0x58) < (99 + ('K')) ? (0x4B + ((85 + ((0x65 + (((37 + 76) + (((0x47 + ((char)(36 + 36))) - 83) - 77)) - 0x4C)) - 91)) - 92)) : ((char)0x53)))
                , (char)(((0x53 + (((28 + 57) + ((0x70 + (((34 + 34) + (((46 + 46) + (('_') - (41 + 42))) - 0x44)) - (25 + 50))) - 0x52)) - 82)) < ((char)0x59) ? (('[') - 0x57) : (_[6])) - ((((0x45 + ('I')) - 0x5A) > ((36 + 36) + ((28 + 58) + ((0x5F + (('Y') - 81)) - (29 + 60)))) ? ((char)92) : ((((42 + 42) + (((32 + 65) + (((34 + 34) + ((0x6D + ((0x64 + ((_[19]) - (39 + 40))) - 0x52)) - 0x4F)) - (22 + 46))) - 0x5C)) - (45 + 45)) - 0x5A))))
                , (char)(((0x52 + (((char)91) - 76)) > (((30 + 61) + ((101 + (('G') - 75)) - 91)) - 0x48) ? ('W') : (((char)0x53) - (22 + 46))) + (((0x64 + ((char)0x56)) > ((char)0x5E) ? ((char)74) : (((char)0x5B) - 0x59)) - ((('H') < (((50 + 50) + ((_[19]) - 0x47)) - 0x48) ? ('S') : (((49 + 50) + ((0x58 + ((0x4D + ((69 + ((70 + ((74 + (((52 + 52) + ((79 + ((70 + ((92 + (((38 + 38) + ((0x4F + ((0x6D + (('O') - (24 + 48))) - 0x52)) - 0x5C)) - 0x4E)) - 0x48)) - 0x53)) - 0x5A)) - (22 + 44))) - 0x54)) - 76)) - 0x4E)) - 0x53)) - 0x54)) - 0x50)))))
                , (char)((((0x42 + ((char)(22 + 46))) - 0x57) < ((112 + ((']') - 69)) - 0x4D) ? ((char)0x4F) : (((42 + 43) + ((70 + ((0x51 + ((117 + (('J') - 0x48)) - 0x56)) - 67)) - 0x47)) - 0x55)) - ((((26 + 54) + ('W')) > ((0x68 + (('Y') - (41 + 41))) - 0x49) ? ((char)0x59) : ((89 + (((33 + 34) + (((55 + 56) + ((_[19]) - 0x4F)) - (34 + 35))) - 0x51)) - 0x42)) + (((0x74 + ((char)74)) > ((0x74 + ((0x47 + ((0x75 + (('_') - 0x5A)) - (30 + 61))) - 0x59)) - (37 + 38)) ? (_[31]) : ((106 + (((35 + 36) + (((33 + 33) + (((58 + 59) + (((char)(43 + 44)) - (25 + 51))) - 79)) - (43 + 44))) - 0x42)) - 0x53)) - (((78 + ((40 + 40) + ((100 + ((0x4C + ((107 + (('Q') - (27 + 54))) - 86)) - 0x45)) - (43 + 44)))) < (((char)0x5F) - 80) ? (((33 + 67) + (('U') - (41 + 41))) - 0x55) : ((29 + 60) + (70 + ((90 + (('W') - 0x4A)) - (22 + 46)))))))))
                , (char)((((char)83) < (0x5D + ((108 + (((char)(29 + 58)) - 0x43)) - (38 + 39))) ? ('K') : ((0x69 + ((91 + (('T') - (24 + 49))) - 0x57)) - 0x4C)) - ((((char)0x5F) > ((24 + 48) + (((34 + 69) + (((26 + 53) + ((0x63 + (('D') - 0x45)) - 0x50)) - 0x4F)) - 0x48)) ? ('V') : ('C')) + ((('O') > (((50 + 51) + (((char)0x51) - 0x46)) - (46 + 46)) ? ((char)0x56) : ('C')) - (((((char)(47 + 48)) - 85) < (((51 + 52) + (((char)76) - 0x4D)) - 0x5A) ? ((char)0x44) : ('G')) - (((_[15]) < (((37 + 74) + ((0x4B + ((0x68 + ((112 + ((_[28]) - 84)) - 90)) - (36 + 37))) - 91)) - 89) ? ((0x6D + (((113 + ((0x50 + (((36 + 37) + (((36 + 73) + (((35 + 71) + (((0x51 + (((26 + 53) + ((char)(33 + 34))) - 0x5B)) - 0x50) - (22 + 44))) - (40 + 41))) - 78)) - 0x5A)) - 85)) - 75) - 0x4B)) - (37 + 37)) : (((((41 + 42) + ((_[19]) - 0x43)) - (36 + 37)) - 68) - 0x48)))))))
                , (char)((((90 + (((34 + 34) + ((0x73 + (((char)0x54) - 74)) - 0x56)) - 84)) - 0x4A) > ((0x4A + ((0x75 + (((char)0x4B) - (25 + 51))) - 0x5C)) - 80) ? (89 + ((107 + ((((50 + 51) + ((72 + ((0x59 + ((103 + ((_[6]) - 87)) - 67)) - 0x48)) - 89)) - (22 + 46)) - 74)) - 0x59)) : ((0x75 + (((35 + 35) + (('_') - 66)) - 0x5B)) - (37 + 38))))
                , (char)((((0x71 + ((_[6]) - 69)) - (27 + 54)) > (((char)(35 + 36)) - (22 + 45)) ? ((char)71) : (((char)94) - 0x52)) + ((((0x73 + (((char)83) - 0x4D)) - 0x51) > (((char)(44 + 44)) - 0x57) ? ('T') : (((54 + 54) + ((_[24]) - 0x46)) - 0x56)) - (((((35 + 36) + ((0x63 + ((116 + (((0x6E + (((34 + 69) + (('[') - 0x55)) - 72)) - 0x56) - (34 + 35))) - (25 + 50))) - 0x45)) - (36 + 37)) > ((0x47 + ((0x4F + ((117 + (((91 + ((97 + (('_') - 0x50)) - (25 + 51))) - (35 + 36)) - 0x4A)) - 0x42)) - 0x45)) - 68) ? ((0x70 + ((0x5F + (((char)0x49) - 0x48)) - 0x58)) - (24 + 49)) : (_[6])))))
                , (char)(((108 + ((char)0x4B)) > (((39 + 40) + ((0x6F + ((0x5D + ((0x6A + (((char)0x57) - 0x58)) - 84)) - 85)) - 0x53)) - 77) ? ('S') : (((29 + 59) + ((_[6]) - 0x45)) - 75)) - (((0x6F + ('A')) < (_[19]) ? ((0x55 + (((39 + 78) + ((79 + ((88 + (((char)(47 + 48)) - (22 + 44))) - 81)) - 0x50)) - 0x5A)) - 92) : (((66 + ((char)0x47)) - 0x5C) - 0x49)))));

            string l = "7fkc83Rhjas61dfBc0xZgfspPs1nNfrpwo.x,zn,DHSGFAME;GLDFJKW7U271=5-3946ZMJCNVB.N,CNXGDJFKSTЫОЛЦН72Г527E5e3ЁйЙжпХyЪy.КЮйИзЭАДЖВЗХЁяЯбИпвРпдрэолэвйУуЖяьбюитмсмшхнгеук";
            s = String.Format("{2}{6}{3}{7}{5}{1}{4}{0}", (char)(((0x6C + (l[15])) > ((l[28]) - (35 + 36)) ? (l[55]) : ('^')) + ((((107 + ((l[28]) - (26 + 53))) - 72) < (0x44 + (((33 + 68) + ((91 + (((char)(31 + 64)) - (29 + 59))) - 0x4B)) - 72)) ? ((l[24]) - 66) : (l[15])))), (char)((((25 + 50) + ((103 + (('\\') - 0x59)) - 0x55)) < (l[54]) ? ((l[24]) - 0x4D) : (l[55])) + ((((33 + 66) + ((70 + ((0x4E + ((0x48 + ((0x5D + ((82 + ((83 + ((0x6D + ((115 + (('I') - (27 + 54))) - 0x5C)) - 0x59)) - (27 + 54))) - 73)) - 0x54)) - 0x48)) - 0x4A)) - 0x4D)) < (((char)0x5D) - 92) ? ((0x69 + (((30 + 61) + ((86 + (('Y') - 76)) - 0x58)) - 0x47)) - 78) : ('I')) + (((71 + (l[6])) < (l[19]) ? ((l[6]) - 0x49) : (l[24])) - (((l[41]) > ((0x62 + ((0x5B + ((0x74 + ((l[43]) - 0x5A)) - 0x45)) - (25 + 51))) - 0x54) ? (l[73]) : (l[55])) + (((l[44]) > (74 + ('_')) ? ((l[73]) - 82) : (l[41])) - (((0x50 + ((0x6E + ((l[50]) - (35 + 36))) - 0x5C)) > ((0x59 + (((char)91) - 70)) - 0x45) ? ('Y') : ((0x47 + ((114 + ((86 + ((89 + ((0x62 + ((l[53]) - (38 + 38))) - 0x57)) - (44 + 44))) - (29 + 60))) - 0x54)) - 0x50)) + ((((l[24]) - 0x48) < ((37 + 75) + (l[28])) ? ((char)81) : (l[41])) - ((((0x6A + ((l[50]) - (28 + 56))) - 88) < (((43 + 43) + (((56 + 56) + (((0x58 + (((32 + 64) + ((90 + ((l[73]) - 0x48)) - 67)) - 0x51)) - 80) - 0x46)) - 83)) - (29 + 59)) ? (l[50]) : (((char)94) - 0x47)) - (((0x72 + (l[50])) > (0x69 + ((l[42]) - (30 + 60))) ? (l[40]) : ((0x70 + ((l[47]) - 0x43)) - 68)) + (((l[24]) < ((38 + 78) + ((l[55]) - 0x5A)) ? ((((0x74 + (((char)(27 + 54)) - 0x51)) - (27 + 56)) - 91) - 0x46) : ((']') - 0x47)))))))))))), (char)(((l[45]) < (0x4A + (0x4D + ((0x45 + ((0x6B + ((l[24]) - 0x4D)) - 0x43)) - 77))) ? (l[80]) : ((0x59 + ((0x68 + ((l[54]) - 76)) - (29 + 59))) - (40 + 40))) - ((((0x73 + ((l[42]) - 73)) - (33 + 34)) < (((char)0x5F) - 0x50) ? ((l[73]) - 0x4D) : (l[40])) + (((((39 + 39) + ((0x55 + (((54 + 55) + ((l[50]) - 0x57)) - 74)) - (26 + 54))) - 0x4B) > ((char)0x5D) ? ((char)(27 + 54)) : (l[6])) + ((((0x54 + ((0x56 + ((0x50 + (((36 + 72) + ((l[87]) - 68)) - 0x5B)) - 0x5B)) - 86)) - 0x57) < (((36 + 74) + ((0x5C + ((l[57]) - 81)) - 0x5A)) - 0x58) ? ((((l[71]) - 0x57) - 79) - 82) : (((26 + 53) + (((30 + 62) + ((0x70 + (('[') - 0x4B)) - (40 + 40))) - 91)) - 0x59)))))), (char)((((0x66 + ((l[73]) - (46 + 46))) - 0x48) > (117 + (((23 + 47) + (l[71])) - 0x56)) ? ('[') : ('I')) + ((((91 + (((char)(30 + 61)) - (36 + 37))) - 0x4D) < (0x5C + (77 + ((82 + (('\\') - 0x4D)) - (23 + 48)))) ? (l[44]) : ((0x45 + (((39 + 40) + ((0x6D + ((115 + (('I') - (37 + 38))) - 92)) - 80)) - 79)) - 0x46)) + (((l[41]) > (0x6A + ((char)92)) ? (('_') - 0x54) : (l[73])) + ((((28 + 56) + ((l[42]) - 0x45)) < (0x54 + (l[55])) ? (l[15]) : ((0x51 + ((117 + ((l[28]) - (38 + 38))) - 0x5B)) - 0x4D)) + (((l[55]) > (0x4C + (0x71 + ((l[24]) - 76))) ? ((0x70 + ((l[55]) - 0x55)) - (27 + 56)) : ((((l[50]) - (30 + 62)) - 0x4F) - 0x51))))))), (char)(((((57 + 58) + ((l[41]) - 0x4A)) - 0x42) < (82 + (l[19])) ? (l[73]) : (((44 + 45) + ((0x60 + ((l[54]) - (25 + 50))) - 0x50)) - 0x4D)) + (((((51 + 52) + (((0x6A + (((47 + 48) + ((89 + ((77 + ((81 + ((70 + ((102 + ((72 + ((114 + ((l[47]) - (27 + 55))) - 0x44)) - 90)) - 0x4F)) - 0x4D)) - 72)) - (30 + 61))) - 0x55)) - 0x5C)) - 77) - (33 + 33))) - 0x56) < ((87 + ((0x6B + (((0x59 + (((41 + 41) + (((48 + 48) + ((0x42 + ((104 + ((l[73]) - 88)) - 0x44)) - 0x45)) - 71)) - 86)) - 0x4F) - 0x42)) - 0x4A)) - (43 + 44)) ? ((0x44 + (((32 + 64) + ((l[55]) - 73)) - 80)) - (23 + 47)) : (((38 + 78) + ((l[19]) - 90)) - (26 + 52))))), (char)(((((55 + 55) + (((char)0x5C) - 0x59)) - (24 + 48)) > (l[44]) ? ((0x70 + (((char)(31 + 63)) - 0x47)) - 80) : (l[15])) - ((((0x4D + (l[40])) - (42 + 43)) > (0x5C + (((char)0x5D) - 0x53)) ? ((0x42 + (l[46])) - (30 + 62)) : (((0x71 + ((l[28]) - 0x52)) - 0x5B) - 66)))), (char)(((l[47]) > (l[73]) ? (0x64 + ((l[15]) - 0x46)) : (l[19])) + ((((char)0x5C) > ((34 + 35) + ((0x6D + ((l[43]) - 84)) - 69)) ? (l[54]) : (l[6])) - (((111 + (((char)0x5F) - 0x57)) < (l[87]) ? (l[43]) : (((34 + 68) + ((0x48 + ((0x51 + ((72 + (((30 + 62) + ((0x55 + ((76 + (((34 + 34) + ((0x6A + ((0x5B + ((l[53]) - (33 + 33))) - 0x5B)) - (25 + 52))) - 0x4B)) - 77)) - (24 + 48))) - 86)) - (37 + 37))) - 81)) - 0x4C)) - 0x5B))))), (char)(((0x72 + ((0x62 + ((l[50]) - 70)) - 85)) < ((0x75 + (('\\') - 0x4C)) - (38 + 39)) ? (79 + ((0x62 + (('\\') - 83)) - 89)) : (l[47])) + ((((70 + (l[15])) - 89) > ((l[57]) - 0x52) ? (l[50]) : (((51 + 52) + ((l[71]) - 71)) - (39 + 39))) - (((l[46]) < (0x48 + ('O')) ? (((25 + 51) + ((0x44 + ('I')) - 0x57)) - 86) : ((0x60 + ((0x4F + ((0x48 + ((97 + (((char)91) - (25 + 52))) - 74)) - 0x5C)) - (26 + 53))) - 0x42))))));            
            //<-test

            string FormatStr = "";
            string FullStr = "";
            CharObf co = new CharObf();
            List<string> obfchars = new List<string>();
            Hashtable uniqchars = new Hashtable();
            for (int i = 0; i < text.Length; i++)
            {
                if (i > 0)
                    FullStr += "+";
                string obfstr = "" + "(char)" + co.Obfuscate2str((int)text[i], 0.4, 0.7);
                FullStr += obfstr;
                obfchars.Add(obfstr);
                if (!uniqchars.ContainsKey(text[i]))
                    uniqchars.Add(text[i], obfstr);
            }
            FullStr = "string s = \"\"+" + FullStr;

            //сначала надо перемешать символы
            int[] range = new int[obfchars.Count];
            for (int i = 0; i < obfchars.Count; i++)
                range[i] = i;
            int[] shuffle = RandomShuffle(range);
            int[] reshuffle = new int[shuffle.Length];
            for (int i = 0; i < shuffle.Length; i++)
                reshuffle[shuffle[i]] = i;

            string obfFormatStr = "";
            for (int i = 0; i < shuffle.Length; i++)
                obfFormatStr += "{" + reshuffle[i] + "}";

            //через format.string получается более оптимизированно 
            obfFormatStr = string.Format("String.Format(\"{0}\" ", obfFormatStr);
            for (int i = 0; i < obfchars.Count;i++ )
                obfFormatStr += string.Format(", {0}", obfchars[shuffle[i]], Environment.NewLine);
            obfFormatStr = string.Format("{0});", obfFormatStr);

            //string.Format("{2}+{0}+{1} > {4} ? {5} : {3}", var1, var2, var3, var5, var4, var7)
            StringBuilder sb = new StringBuilder();
            //sb.Append(@"using (StreamWriter wr = new StreamWriter(""data.txt""))\n");
            //sb.Append(string.Format(@"wr.WriteLine(""{0}"");\n", signStr));
            string varchars = "_lI";//O0AEKАЕОКТT";
            char varchar = varchars[rnd.Next(0, varchars.Length)];
            sb.Append(string.Format(@"string {2} = ""{1}"";{0}", Environment.NewLine, signStr, varchar));
            //sb.Append(@"using (StreamReader rd = new StreamReader(""data.txt""))\n");
            //sb.Append(@"_ = rd.ReadLine();\n");
            obfFormatStr = string.Format("string s = {1}",
                signStr,
                obfFormatStr.Replace("{VARIABLE}", varchar.ToString()).Replace("'\\'", "'\\\\'").Replace(" ", "").Replace("?", " ? ").Replace("<", " < ").Replace(">", " > ").Replace(":", " : ").Replace("-", " - ").Replace("+", " + "));
            sb.Append(obfFormatStr);
            return sb.ToString();
        }
    }
}
