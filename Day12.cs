namespace AdventOfCode
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Collections.Generic;

    public class Day12
    {
        private static Dictionary<char, int> Registers = new Dictionary<char, int>
            { {'a', 0},
              { 'b', 0 },
              {'c', 0},
              {'d', 0} };
        private static int CurrentInstruction = 0;

        public static int SolveProblem1()
        {
            var instructions = ProblemInput.SplitToLines().ToArray();
            // var instructions = TestProblemInput1.SplitToLines().ToArray();

            while (CurrentInstruction < instructions.Length)
            {
                CurrentInstruction += Execute(instructions[CurrentInstruction]);
            }

            return Registers['a'];
        }

        private static int Execute(string instruction)
        {
            if (instruction.StartsWith("inc"))
            {
                Registers[instruction[4]] = Registers[instruction[4]] + 1;
                return 1;
            }
            else if (instruction.StartsWith("dec"))
            {
                Registers[instruction[4]] = Registers[instruction[4]] - 1;
                return 1;
            }
            else if (instruction.StartsWith("cpy"))
            {
                Copy(instruction.Substring(4));
                return 1;
            }
            else
            {
                return Jump(instruction.Substring(4));
            }
        }

        private static int Jump(string values)
        {
            var parser = new Regex(@"(\w|\d+) (-?\d+)");
            var match = parser.Match(values);

            int firstValue = 0;
            if (!int.TryParse(match.Groups[1].Value, out firstValue))
            {
                firstValue = Registers[match.Groups[1].Value[0]];
            }

            if (firstValue == 0)
            {
                return 1;
            }

            return int.Parse(match.Groups[2].Value);
        }

        private static void Copy(string values)
        {
            var parser = new Regex(@"(\w|\d+) (\w)");
            var match = parser.Match(values);

            int firstValue = 0;
            if (!int.TryParse(match.Groups[1].Value, out firstValue))
            {
                firstValue = Registers[match.Groups[1].Value[0]];
            }

            Registers[match.Groups[2].Value[0]] = firstValue;
        }

        public static int SolveProblem2()
        {
            Registers['c'] = 1;

            var instructions = ProblemInput.SplitToLines().ToArray();
            // var instructions = TestProblemInput1.SplitToLines().ToArray();

            while (CurrentInstruction < instructions.Length)
            {
                CurrentInstruction += Execute(instructions[CurrentInstruction]);
            }

            return Registers['a'];
        }

//         private static readonly string TestProblemInput1 = @"cpy 41 a
// inc a
// inc a
// dec a
// jnz a 2
// dec a";
        private static readonly string ProblemInput = @"cpy 1 a
cpy 1 b
cpy 26 d
jnz c 2
jnz 1 5
cpy 7 c
inc d
dec c
jnz c -2
cpy a c
inc a
dec b
jnz b -2
cpy c b
dec d
jnz d -6
cpy 16 c
cpy 12 d
inc a
dec d
jnz d -2
dec c
jnz c -5";
    }
}