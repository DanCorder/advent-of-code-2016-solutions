namespace AdventOfCode
{
    using System;
    using System.Linq;

    public class Day02
    {
        private static readonly int[] leftMostColumn = { 1, 4, 7 };
        private static readonly int[] rightMostColumn = { 3, 6, 9 };

        // private static readonly string[] Problem1Input = new string[] {
        //     "ULL",
        //     "RRDDD",
        //     "LURDL",
        //     "UUUUD"
        // };
        private static readonly string[] Problem1Input = new string[] {
            "DLRURUDLULRDRUDDRLUUUDLDLDLRLRRDRRRLLLLLDDRRRDRRDRRRLRRURLRDUULRLRRDDLULRLLDUDLULURRLRLDUDLURURLDRDDULDRDRDLDLLULULLDDLRRUDULLUULRRLLLURDRLDDLDDLDRLRRLLRURRUURRRRLUDLRDDDDRDULRLLDDUURDUDRLUDULLUDLUDURRDRDUUUUDDUDLLLRLUULRUURDLRLLRRLRLLDLLRLLRRRURLRRLURRLDLLLUUDURUDDLLUURRDRDRRDLLDDLLRDRDRRLURLDLDRDLURLDULDRURRRUDLLULDUDRURULDUDLULULRRRUDLUURRDURRURRLRRLLRDDUUUUUDUULDRLDLLRRUDRRDULLLDUDDUDUURLRDLULUUDLDRDUUUDDDUDLDURRULUULUUULDRUDDLLLDLULLRLRLUDULLDLLRLDLDDDUUDURDDDLURDRRDDLDRLLRLRR",
            "RLDUDURDRLLLLDDRRRURLLLRUUDDLRDRDDDUDLLUDDLRDURLDRDLLDRULDDRLDDDRLDRDDDRLLULDURRRLULDRLRDRDURURRDUDRURLDRLURDRLUULLULLDLUDUDRDRDDLDDDDRDURDLUDRDRURUDDLLLRLDDRURLLUDULULDDLLLDLUDLDULUUDLRLURLDRLURURRDUUDLRDDDDDRLDULUDLDDURDLURLUURDLURLDRURRLDLLRRUDRUULLRLDUUDURRLDURRLRUULDDLDLDUUDDRLDLLRRRUURLLUURURRURRLLLUDLDRRDLUULULUDDULLUDRLDDRURDRDUDULUDRLRRRUULLDRDRLULLLDURURURLURDLRRLLLDRLDUDLLLLDUUURULDDLDLLRRUDDDURULRLLUDLRDLUUDDRDDLLLRLUURLDLRUURDURDDDLLLLLULRRRURRDLUDLUURRDRLRUDUUUURRURLRDRRLRDRDULLDRDRLDURDDUURLRUDDDDDLRLLRUDDDDDURURRLDRRUUUDLURUUDRRDLLULDRRLRRRLUUUD",
            "RDRURLLUUDURURDUUULLRDRLRRLRUDDUDRURLLDLUUDLRLLDDURRURLUDUDDURLURLRRURLLURRUDRUDLDRLLURLRUUURRUDDDURRRLULLLLURDLRLLDDRLDRLLRRDLURDLRDLDUDRUULLDUUUDLURRLLRUDDDUUURLURUUDRLRULUURLLRLUDDLLDURULLLDURDLULDLDDUDULUDDULLRDRURDRRLLDLDDDDRUDLDRRLLLRLLLRRULDLRLRLRLLDLRDRDLLUDRDRULDUURRDDDRLLRLDLDRDUDRULUDRDLDLDDLLRULURLLURDLRRDUDLULLDLULLUDRRDDRLRURRLDUDLRRUUDLDRLRLDRLRRDURRDRRDDULURUUDDUUULRLDRLLDURRDLUULLUDRDDDLRUDLRULLDDDLURLURLRDRLLURRRUDLRRLURDUUDRLRUUDUULLRUUUDUUDDUURULDLDLURLRURLRUDLULLULRULDRDRLLLRRDLU",
            "RRRRDRLUUULLLRLDDLULRUUURRDRDRURRUURUDUULRULULRDRLRRLURDRRRULUUULRRUUULULRDDLLUURRLLDUDRLRRLDDLDLLDURLLUDLDDRRURLDLULRDUULDRLRDLLDLRULLRULLUDUDUDDUULDLUUDDLUDDUULLLLLURRDRULURDUUUDULRUDLLRUUULLUULLLRUUDDRRLRDUDDRULRDLDLLLLRLDDRRRULULLLDLRLURRDULRDRDUDDRLRLDRRDLRRRLLDLLDULLUDDUDDRULLLUDDRLLRRRLDRRURUUURRDLDLURRDLURULULRDUURLLULDULDUDLLULDDUURRRLDURDLUDURLDDRDUDDLLUULDRRLDLLUDRDURLLDRLDDUDURDLUUUUURRUULULLURLDUUULLRURLLLUURDULLUULDRULLUULRDRUULLRUDLDDLRLURRUUDRLRRRULRUUULRULRRLDLUDRRLL",
            "ULRLDLLURDRRUULRDUDDURDDDLRRRURLDRUDDLUDDDLLLRDLRLLRRUUDRRDRUULLLULULUUDRRRDRDRUUUUULRURUULULLULDULURRLURUDRDRUDRURURUDLDURUDUDDDRLRLLLLURULUDLRLDDLRUDDUUDURUULRLLLDDLLLLRRRDDLRLUDDUULRRLLRDUDLLDLRRUUULRLRDLRDUDLLLDLRULDRURDLLULLLRRRURDLLUURUDDURLDUUDLLDDRUUDULDRDRDRDDUDURLRRRRUDURLRRUDUDUURDRDULRLRLLRLUDLURUDRUDLULLULRLLULRUDDURUURDLRUULDURDRRRLLLLLUUUULUULDLDULLRURLUDLDRLRLRLRDLDRUDULDDRRDURDDULRULDRLRULDRLDLLUDLDRLRLRUDRDDR"
            };

        public static string SolveProblem1()
        {
            var code = "";
            var currentDigit = 5;

            foreach (var instruction in Problem1Input)
            {
                var nextDigit = getNextDigit(currentDigit, instruction);
                currentDigit = nextDigit;
                code += nextDigit;
            }

            return code;
        }

        public static string SolveProblem2()
        {
            var code = "";
            var currentDigit = '5';

            foreach (var instruction in Problem1Input)
            {
                var nextDigit = getNextDigit2(currentDigit, instruction);
                currentDigit = nextDigit;
                code += nextDigit;
            }

            return code;
        }

        private static int getNextDigit(int currentDigit, string instruction)
        {
            for (int i = 0; i < instruction.Length; i++)
            {
                currentDigit = getNextDigit(currentDigit, instruction[i]);
            }
            return currentDigit;
        }

        private static int getNextDigit(int currentDigit, char instruction)
        {
            switch(instruction)
            {
                case 'U':
                    if (currentDigit <= 3)
                    {
                        return currentDigit;
                    }
                    return currentDigit - 3;
                case 'D':
                    if (currentDigit >= 7)
                    {
                        return currentDigit;
                    }
                    return currentDigit + 3;
                case 'L':
                    if (leftMostColumn.Contains(currentDigit))
                    {
                        return currentDigit;
                    }
                    return currentDigit - 1;
                case 'R':
                    if (rightMostColumn.Contains(currentDigit))
                    {
                        return currentDigit;
                    }
                    return currentDigit + 1;
                default:
                    throw new Exception("Unrecognised instruction: " + instruction);
            }
        }

        private static char getNextDigit2(char currentDigit, string instruction)
        {
            for (int i = 0; i < instruction.Length; i++)
            {
                currentDigit = getNextDigit2(currentDigit, instruction[i]);
            }
            return currentDigit;
        }

        //     1
        //   2 3 4
        // 5 6 7 8 9
        //   A B C
        //     D
        private static char getNextDigit2(char currentDigit, char instruction)
        {
            switch(instruction)
            {
                case 'U':
                    if (currentDigit == '3')
                        return '1';
                    if (currentDigit == '6')
                        return '2';
                    if (currentDigit == '7')
                        return '3';
                    if (currentDigit == '8')
                        return '4';
                    if (currentDigit == 'A')
                        return '6';
                    if (currentDigit == 'B')
                        return '7';
                    if (currentDigit == 'C')
                        return '8';
                    if (currentDigit == 'D')
                        return 'B';
                    return currentDigit;
                //     1
                //   2 3 4
                // 5 6 7 8 9
                //   A B C
                //     D
                case 'D':
                    if (currentDigit == '1')
                        return '3';
                    if (currentDigit == '2')
                        return '6';
                    if (currentDigit == '3')
                        return '7';
                    if (currentDigit == '4')
                        return '8';
                    if (currentDigit == '6')
                        return 'A';
                    if (currentDigit == '7')
                        return 'B';
                    if (currentDigit == '8')
                        return 'C';
                    if (currentDigit == 'B')
                        return 'D';
                    return currentDigit;
                //     1
                //   2 3 4
                // 5 6 7 8 9
                //   A B C
                //     D
                case 'L':
                    if (currentDigit == '9')
                        return '8';
                    if (currentDigit == '4')
                        return '3';
                    if (currentDigit == '8')
                        return '7';
                    if (currentDigit == 'C')
                        return 'B';
                    if (currentDigit == '3')
                        return '2';
                    if (currentDigit == '7')
                        return '6';
                    if (currentDigit == 'B')
                        return 'A';
                    if (currentDigit == '6')
                        return '5';
                    return currentDigit;
                //     1
                //   2 3 4
                // 5 6 7 8 9
                //   A B C
                //     D
                case 'R':
                    if (currentDigit == '5')
                        return '6';
                    if (currentDigit == '2')
                        return '3';
                    if (currentDigit == '6')
                        return '7';
                    if (currentDigit == 'A')
                        return 'B';
                    if (currentDigit == '3')
                        return '4';
                    if (currentDigit == '7')
                        return '8';
                    if (currentDigit == 'B')
                        return 'C';
                    if (currentDigit == '8')
                        return '9';
                    return currentDigit;
                default:
                    throw new Exception("Unrecognised instruction: " + instruction);
            }
        }
    }
}