namespace AdventOfCode
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Collections.Generic;

    public class Day23
    {
        private static Dictionary<char, int> Registers = new Dictionary<char, int>
            { {'a', 7},
              {'b', 0 },
              {'c', 0},
              {'d', 0} };
        private static int CurrentInstruction = 0;

        public static int SolveProblem1()
        {
            var instructions = ProblemInput.SplitToLines().ToArray();
            // var instructions = TestProblemInput1.SplitToLines().ToArray();

            while (CurrentInstruction < instructions.Length)
            {
                // Console.WriteLine("Registers");
                // Console.WriteLine("a=" + Registers['a']);
                // Console.WriteLine("b=" + Registers['b']);
                // Console.WriteLine("c=" + Registers['c']);
                // Console.WriteLine("d=" + Registers['d']);
                // Console.WriteLine("Executing: " + instructions[CurrentInstruction] + " at " + CurrentInstruction);

                CurrentInstruction += Execute(instructions[CurrentInstruction], CurrentInstruction, instructions);
            }

            return Registers['a'];
        }

        private static int Execute(string instruction, int currentInstruction, string[] instructions)
        {
            if (currentInstruction == 4)
            {
                if (instructions[4] == "cpy b c" &&
                  instructions[5] == "inc a" &&
                instructions[6] == "dec c" &&
                instructions[7] == "jnz c -2" &&
                instructions[8] == "dec d" &&
                instructions[9] == "jnz d -5")
                {
                    Registers['a'] += (Registers['b'] * Registers['d']);
                    Registers['c'] = 0;
                    Registers['d'] = 0;
                    return 6;
                }
                else
                {
                    Console.WriteLine("Mulitplier changed");
                }
            }

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
            else if (instruction.StartsWith("tgl"))
            {
                Toggle(instruction[4], currentInstruction, instructions);
                return 1;
            }
            else
            {
                return Jump(instruction.Substring(4), currentInstruction, instructions);
            }
        }

        private static int Jump(string values, int currentInstruction, string[] instructions)
        {
            // General case
            var parser = new Regex(@"(\w|\d+) (-?)([\w|\d]+)");
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

            int secondValue = 0;
            if (!int.TryParse(match.Groups[3].Value, out secondValue))
            {
                secondValue = Registers[match.Groups[3].Value[0]];
            }
            if (match.Groups[2].Value.Length > 0)
            {
                secondValue *= -1;
            }

            return secondValue;
        }

        private static void Copy(string values)
        {
            var parser = new Regex(@"(\w|-?\d+) (\w)");
            var match = parser.Match(values);

            int firstValue = 0;
            if (!int.TryParse(match.Groups[1].Value, out firstValue))
            {
                firstValue = Registers[match.Groups[1].Value[0]];
            }

            if (Registers.ContainsKey(match.Groups[2].Value[0]))
            {
                Registers[match.Groups[2].Value[0]] = firstValue;
            }
        }

        private static void Toggle(char register, int currentInstruction, string[] instructions)
        {
            // Console.WriteLine(string.Format("Toggle register {0} ({1}), current index {2}", register, Registers[register], currentInstruction));
            // Console.WriteLine("Current instructions:");
            // foreach(var instruction in instructions)
            // {
            //     Console.WriteLine(instruction);
            // }

            int jumpValue = Registers[register];
            int index = currentInstruction + jumpValue;

            if (index < 0 || index >= instructions.Length)
            {
                // Console.WriteLine("Skipping as index=" + index);
                return;
            }

            var instructionToChange = instructions[index];
            string newInstruction = "";

            if (instructionToChange.StartsWith("inc"))
            {
                newInstruction = "dec" + instructionToChange.Substring(3);
            }
            else if (instructionToChange.StartsWith("dec"))
            {
                newInstruction = "inc" + instructionToChange.Substring(3);
            }
            else if (instructionToChange.StartsWith("cpy"))
            {
                newInstruction = "jnz" + instructionToChange.Substring(3);
            }
            else if (instructionToChange.StartsWith("tgl"))
            {
                newInstruction = "inc" + instructionToChange.Substring(3);
            }
            else
            {
                newInstruction = "cpy" + instructionToChange.Substring(3);
            }

            // Console.WriteLine("New instructions:");
            // foreach(var instruction in instructions)
            // {
            //     Console.WriteLine(instruction);
            // }

            instructions[index] = newInstruction;
        }

        public static int SolveProblem2()
        {
            Registers['a'] = 12;

            var instructions = ProblemInput.SplitToLines().ToArray();
            // var instructions = TestProblemInput1.SplitToLines().ToArray();

            while (CurrentInstruction < instructions.Length)
            {
                // Console.WriteLine("Registers");
                // Console.WriteLine("a=" + Registers['a']);
                // Console.WriteLine("b=" + Registers['b']);
                // Console.WriteLine("c=" + Registers['c']);
                // Console.WriteLine("d=" + Registers['d']);
                Console.WriteLine("Executing: " + instructions[CurrentInstruction] + " at " + CurrentInstruction);

                CurrentInstruction += Execute(instructions[CurrentInstruction], CurrentInstruction, instructions);
            }

            return Registers['a'];
        }

//         private static readonly string TestProblemInput1 = @"cpy 2 a
// tgl a
// tgl a
// tgl a
// cpy 1 a
// dec a
// dec a";
        private static readonly string ProblemInput = @"cpy a b
dec b
cpy a d
cpy 0 a
cpy b c
inc a
dec c
jnz c -2
dec d
jnz d -5
dec b
cpy b c
cpy c d
dec d
inc c
jnz d -2
tgl c
cpy -16 c
jnz 1 c
cpy 78 c
jnz 99 d
inc a
inc d
jnz d -2
inc c
jnz c -5";
    }
}