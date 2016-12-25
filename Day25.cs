namespace AdventOfCode
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Collections.Generic;

    public class Day25
    {
        private static Dictionary<char, int> Registers = new Dictionary<char, int>
            { {'a', 7},
              {'b', 0 },
              {'c', 0},
              {'d', 0} };
        private static int CurrentInstruction = 0;
        private static List<int> output = new List<int>();

        public static int SolveProblem1()
        {
            // return alternativeSolve();


            var instructions = ProblemInput.SplitToLines().ToArray();
            // var instructions = TestProblemInput1.SplitToLines().ToArray();
            var startValue = 0;
            var instructionsExecuted = 0;

            while (true)
            {
                if (startValue % 1000 == 0)
                {
                    Console.WriteLine("Trying: " + startValue);
                }
                Registers['a'] = startValue;
                Registers['b'] = 0;
                Registers['c'] = 0;
                Registers['d'] = 0;
                instructionsExecuted = 0;
                CurrentInstruction = 0;
                output = new List<int>();

                if (startValue == 196)
                {
                    var x = 1;
                }

                while (CurrentInstruction < instructions.Length && !InvalidOutput() && instructionsExecuted < 10000)
                {
                    CurrentInstruction += Execute(instructions[CurrentInstruction], CurrentInstruction, instructions);
                    instructionsExecuted++;
                    //Console.WriteLine("Executed: " + instructionsExecuted);
                }

                if (CurrentInstruction < instructions.Length && !InvalidOutput())
                {
                    break;
                }

                startValue++;
            }

            var outputString = string.Join(",", output.Select(v => v.ToString()));
            Console.WriteLine(outputString);
            return startValue;
        }

        // private static bool OutputIsValid()
        // {
        //     return output.Count == 6 &&
        //         output[0] == 0 &&
        //         output[2] == 0 &&
        //         output[4] == 0 &&
        //         output[1] == 1 &&
        //         output[3] == 1 &&
        //         output[5] == 1;
        // }
        private static bool InvalidOutput()
        {
            var ret = output.Count > 1 &&
                (output.Where((v, i) => i %2 == 0).Any(v => v != 0) ||
                output.Where((v, i) => i %2 == 1).Any(v => v != 1));

            return ret;
        }
        private static int Execute(string instruction, int currentInstruction, string[] instructions)
        {
            // Overall this program does:
            // Copy start value into d
            // Increment d by (7*362) = 2534
            // Copy d into a
            // Divide a by 2 and output remainder.
            // If a != 0 then repeat
            // else  a = d and start dividing again
            if (currentInstruction == 2)
            {
                    Registers['d'] += (Registers['c'] * 362);
                    Registers['c'] = 0;
                    Registers['b'] = 0;
                    return 6;
            }
            else if (currentInstruction == 10)
            {
                // cpy a b
                // cpy 0 a  b = a, a =0
                // cpy 2 c  c = 2 : YY
                // jnz b 2  if (b == 0) break : XX
                // jnz 1 6
                // dec b    dec b and c
                // dec c
                // jnz c -4 if (c != 0) loop to XX
                // inc a    inc a
                // jnz 1 -7 loop to YY
                //
                // At break: b = 0, c = 1 if a is odd or 2 if a is even, a = a/2
                //
                // cpy 2 b   b = 2, c = 1 or 2
                // jnz c 2   if c == 0 break
                // jnz 1 4
                // dec b
                // dec c
                // jnz 1 -4
                //
                // at break c = 0; if c was 1, b =1 if c was 2, b = 0

                // So divide a by 2 and put the remainder in b
                var a = Registers['a'];

                Registers['b'] = a%2;
                Registers['c'] = 0;
                Registers['a'] = a/2;
                return 16;
            }
            else if (instruction.StartsWith("inc"))
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
            else if (instruction.StartsWith("out"))
            {
                Output(instruction.Substring(4));
                return 1;
            }
            else
            {
                return Jump(instruction.Substring(4), currentInstruction, instructions);
            }
        }

        private static void Output(string values)
        {
            // General case
            var parser = new Regex(@"(-?)([\w|\d+])");
            var match = parser.Match(values);

            int secondValue = 0;
            if (!int.TryParse(match.Groups[2].Value, out secondValue))
            {
                secondValue = Registers[match.Groups[2].Value[0]];
            }
            if (match.Groups[1].Value.Length > 0)
            {
                secondValue *= -1;
            }
            output.Add(secondValue);
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

        // Did this when I couldn't find the issue with the longer solution (regex issue in the end!)
        // It does produce the correct answer, see comments in longer solution for why it does what it
        // does.
        private static int alternativeSolve()
        {
            var seed = 2534;
            var testValue = seed;

            while (true)
            {
                if (IsAnswer(testValue))
                    break;

                testValue++;
            }

            return testValue - seed;
        }

        private static bool IsAnswer(int seed)
        {
            List<int> outputValues = new List<int>();
            while (seed > 0)
            {
                outputValues.Add(seed%2);
                seed /= 2;
            }

            var evens = outputValues.Where((v, i) => i %2 == 0).ToArray();

            return outputValues.Count % 2 == 0 &&
                outputValues.Where((v, i) => i %2 == 0).All(v => v == 0) &&
                outputValues.Where((v, i) => i %2 == 1).All(v => v == 1);
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
        private static readonly string ProblemInput = @"cpy a d
cpy 7 c
cpy 362 b
inc d
dec b
jnz b -2
dec c
jnz c -5
cpy d a
jnz 0 0
cpy a b
cpy 0 a
cpy 2 c
jnz b 2
jnz 1 6
dec b
dec c
jnz c -4
inc a
jnz 1 -7
cpy 2 b
jnz c 2
jnz 1 4
dec b
dec c
jnz 1 -4
jnz 0 0
out b
jnz a -19
jnz 1 -21";
    }
}