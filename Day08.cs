namespace AdventOfCode
{
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Day08
    {
        private static bool[,] display = new bool[50,6];
        public static int SolveProblem1()
        {
            // var instructions = TestProblemInput1.Split('\r', '\n').Where(i => !string.IsNullOrEmpty(i));
            var instructions = ProblemInput.Split('\r', '\n').Where(i => !string.IsNullOrEmpty(i));
            foreach(var instruction in instructions)
            {
                ExecuteInstruction(instruction);
            }

            return display.Cast<bool>().Count(x => x);
        }

        private static void ExecuteInstruction(string instruction)
        {
            if (instruction.StartsWith("rect "))
            {
                DrawRect(instruction.Substring(5));
            }
            else if (instruction.StartsWith("rotate column x="))
            {
                RotateColumnX(instruction.Substring(16));
            }
            else if (instruction.StartsWith("rotate row y="))
            {
                RotateRowY(instruction.Substring(13));
            }
            PrintDisplay();
        }

        private static void RotateRowY(string details)
        {
            var parser = new Regex(@"(\d+) by (\d+)");
            var match = parser.Match(details);
            var row = int.Parse(match.Groups[1].Value);
            var amount = int.Parse(match.Groups[2].Value);

            var endOfRowValue = new bool[amount];
            for (int i = 0; i < amount; i++)
            {
                endOfRowValue[i] = display[50 - amount + i, row];
            }

            for(var x = 49; x >= amount; x--)
            {
                display[x, row] = display[x-amount, row];
            }

            for (int i = 0; i < amount; i++)
            {
                display[i, row] = endOfRowValue[i];
            }
        }

        private static void RotateColumnX(string details)
        {
            var parser = new Regex(@"(\d+) by (\d+)");
            var match = parser.Match(details);
            var column = int.Parse(match.Groups[1].Value);
            var amount = int.Parse(match.Groups[2].Value);

            var endOfColumnValue = new bool[amount];
            for (int i = 0; i < amount; i++)
            {
                endOfColumnValue[i] = display[column, 6 - amount + i];
            }

            for(var y = 5; y >= amount; y--)
            {
                display[column, y] = display[column, y-amount];
            }

            for (int i = 0; i < amount; i++)
            {
                display[column, i] = endOfColumnValue[i];
            }

        }

        private static void DrawRect(string details)
        {
            var parser = new Regex(@"(\d+)x(\d+)");
            var match = parser.Match(details);
            var xMax = int.Parse(match.Groups[1].Value);
            var yMax = int.Parse(match.Groups[2].Value);

            for(var x = 0; x < xMax; x++)
            {
                for (var y = 0; y < yMax; y++)
                {
                    display[x, y] = true;
                }
            }
        }

        private static void PrintDisplay()
        {
            for (var y = 0; y < 6; y++)
            {
                for(var x = 0; x < 50; x++)
                {
                    System.Console.Write(display[x,y] ? '#' : '.');
                }
                System.Console.WriteLine("");
            }
            System.Console.WriteLine("");
        }

//         private static readonly string TestProblemInput1 = @"rect 3x2
// rotate column x=1 by 1
// rotate row y=0 by 4
// rotate column x=1 by 4";
        private static readonly string ProblemInput = @"rect 1x1
rotate row y=0 by 5
rect 1x1
rotate row y=0 by 5
rect 1x1
rotate row y=0 by 3
rect 1x1
rotate row y=0 by 2
rect 1x1
rotate row y=0 by 3
rect 1x1
rotate row y=0 by 2
rect 1x1
rotate row y=0 by 5
rect 1x1
rotate row y=0 by 5
rect 1x1
rotate row y=0 by 3
rect 1x1
rotate row y=0 by 2
rect 1x1
rotate row y=0 by 3
rect 2x1
rotate row y=0 by 2
rect 1x2
rotate row y=1 by 5
rotate row y=0 by 3
rect 1x2
rotate column x=30 by 1
rotate column x=25 by 1
rotate column x=10 by 1
rotate row y=1 by 5
rotate row y=0 by 2
rect 1x2
rotate row y=0 by 5
rotate column x=0 by 1
rect 4x1
rotate row y=2 by 18
rotate row y=0 by 5
rotate column x=0 by 1
rect 3x1
rotate row y=2 by 12
rotate row y=0 by 5
rotate column x=0 by 1
rect 4x1
rotate column x=20 by 1
rotate row y=2 by 5
rotate row y=0 by 5
rotate column x=0 by 1
rect 4x1
rotate row y=2 by 15
rotate row y=0 by 15
rotate column x=10 by 1
rotate column x=5 by 1
rotate column x=0 by 1
rect 14x1
rotate column x=37 by 1
rotate column x=23 by 1
rotate column x=7 by 2
rotate row y=3 by 20
rotate row y=0 by 5
rotate column x=0 by 1
rect 4x1
rotate row y=3 by 5
rotate row y=2 by 2
rotate row y=1 by 4
rotate row y=0 by 4
rect 1x4
rotate column x=35 by 3
rotate column x=18 by 3
rotate column x=13 by 3
rotate row y=3 by 5
rotate row y=2 by 3
rotate row y=1 by 1
rotate row y=0 by 1
rect 1x5
rotate row y=4 by 20
rotate row y=3 by 10
rotate row y=2 by 13
rotate row y=0 by 10
rotate column x=5 by 1
rotate column x=3 by 3
rotate column x=2 by 1
rotate column x=1 by 1
rotate column x=0 by 1
rect 9x1
rotate row y=4 by 10
rotate row y=3 by 10
rotate row y=1 by 10
rotate row y=0 by 10
rotate column x=7 by 2
rotate column x=5 by 1
rotate column x=2 by 1
rotate column x=1 by 1
rotate column x=0 by 1
rect 9x1
rotate row y=4 by 20
rotate row y=3 by 12
rotate row y=1 by 15
rotate row y=0 by 10
rotate column x=8 by 2
rotate column x=7 by 1
rotate column x=6 by 2
rotate column x=5 by 1
rotate column x=3 by 1
rotate column x=2 by 1
rotate column x=1 by 1
rotate column x=0 by 1
rect 9x1
rotate column x=46 by 2
rotate column x=43 by 2
rotate column x=24 by 2
rotate column x=14 by 3
rotate row y=5 by 15
rotate row y=4 by 10
rotate row y=3 by 3
rotate row y=2 by 37
rotate row y=1 by 10
rotate row y=0 by 5
rotate column x=0 by 3
rect 3x3
rotate row y=5 by 15
rotate row y=3 by 10
rotate row y=2 by 10
rotate row y=0 by 10
rotate column x=7 by 3
rotate column x=6 by 3
rotate column x=5 by 1
rotate column x=3 by 1
rotate column x=2 by 1
rotate column x=1 by 1
rotate column x=0 by 1
rect 9x1
rotate column x=19 by 1
rotate column x=10 by 3
rotate column x=5 by 4
rotate row y=5 by 5
rotate row y=4 by 5
rotate row y=3 by 40
rotate row y=2 by 35
rotate row y=1 by 15
rotate row y=0 by 30
rotate column x=48 by 4
rotate column x=47 by 3
rotate column x=46 by 3
rotate column x=45 by 1
rotate column x=43 by 1
rotate column x=42 by 5
rotate column x=41 by 5
rotate column x=40 by 1
rotate column x=33 by 2
rotate column x=32 by 3
rotate column x=31 by 2
rotate column x=28 by 1
rotate column x=27 by 5
rotate column x=26 by 5
rotate column x=25 by 1
rotate column x=23 by 5
rotate column x=22 by 5
rotate column x=21 by 5
rotate column x=18 by 5
rotate column x=17 by 5
rotate column x=16 by 5
rotate column x=13 by 5
rotate column x=12 by 5
rotate column x=11 by 5
rotate column x=3 by 1
rotate column x=2 by 5
rotate column x=1 by 5
rotate column x=0 by 1";
    }
}