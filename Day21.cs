namespace AdventOfCode
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class Day21
    {
        private static Regex SwapPostionParser = new Regex(@"swap position (\d+) with position (\d+)");
        private static Regex SwapLetterParser = new Regex(@"swap letter (\w) with letter (\w)");
        private static Regex RotateParser = new Regex(@"rotate (left|right) (\d+) step");
        private static Regex RotateLetterParser = new Regex(@"rotate based on position of letter (\w)");
        private static Regex ReverseParser = new Regex(@"reverse positions (\d+) through (\d+)");
        private static Regex MoveParser = new Regex(@"move position (\d+) to position (\d+)");

        public static string SolveProblem1()
        {
            Console.WriteLine("Start: " + DateTime.Now);
            // var instructions = TestProblemInput1.SplitToLines();
            var instructions = ProblemInput.SplitToLines();

            // var password = "abcde";
            var password = "abcdefgh";

            foreach (var instruction in instructions)
            {
                password = DoInstruction(instruction, password);
                Console.WriteLine(password);
            }

            Console.WriteLine("End: " + DateTime.Now);
            return password;
        }

        public static string SolveProblem2()
        {
            Console.WriteLine("Start: " + DateTime.Now);
            // var instructions = TestProblemInput1.SplitToLines();
            var instructions = ProblemInput.SplitToLines().Reverse();

            // var password = "abcde";
            var password = "fbgdceah";

            foreach (var instruction in instructions)
            {
                password = DoReverseInstruction(instruction, password);
                Console.WriteLine(password);
            }

            Console.WriteLine("End: " + DateTime.Now);
            return password;
        }

        private static string DoInstruction(string instruction, string password)
        {
            if (instruction.StartsWith("swap position"))
            {
                var match = SwapPostionParser.Match(instruction);
                var index1 = int.Parse(match.Groups[1].Value);
                var index2 = int.Parse(match.Groups[2].Value);
                return swapCharsAt(password, index1, index2);
            }
            else if (instruction.StartsWith("swap letter"))
            {
                var match = SwapLetterParser.Match(instruction);
                var letter1 = match.Groups[1].Value[0];
                var letter2 = match.Groups[2].Value[0];
                var index1 = password.IndexOf(letter1);
                var index2 = password.IndexOf(letter2);
                return swapCharsAt(password, index1, index2);
            }
            else if (instruction.StartsWith("rotate based"))
            {
                var match = RotateLetterParser.Match(instruction);
                var letter = match.Groups[1].Value[0];
                var index = password.IndexOf(letter);
                var rotations = index >= 4 ? index + 2 : index + 1;
                return Rotate(password, true, rotations);
            }
            else if (instruction.StartsWith("rotate"))
            {
                var match = RotateParser.Match(instruction);
                var direction = match.Groups[1].Value;
                var steps = int.Parse(match.Groups[2].Value);
                return Rotate(password, direction == "right", steps);
            }
            else if (instruction.StartsWith("reverse"))
            {
                var match = ReverseParser.Match(instruction);
                var index1 = int.Parse(match.Groups[1].Value);
                var index2 = int.Parse(match.Groups[2].Value);
                return password.Substring(0, index1) + new string(password.Substring(index1, index2 - index1 + 1).Reverse().ToArray()) + password.Substring(index2 + 1);
            }
            else if (instruction.StartsWith("move"))
            {
                var match = MoveParser.Match(instruction);
                var index1 = int.Parse(match.Groups[1].Value);
                var index2 = int.Parse(match.Groups[2].Value);
                var letter = password[index1];
                var chars = password.ToList();
                chars.Remove(letter);
                chars.Insert(index2, letter);
                return new string(chars.ToArray());
            }

            throw new Exception("Bad instruction: " + instruction);
        }

        private static string DoReverseInstruction(string instruction, string password)
        {
            if (instruction.StartsWith("swap position"))
            {
                var match = SwapPostionParser.Match(instruction);
                var index1 = int.Parse(match.Groups[1].Value);
                var index2 = int.Parse(match.Groups[2].Value);
                return swapCharsAt(password, index1, index2);
            }
            else if (instruction.StartsWith("swap letter"))
            {
                var match = SwapLetterParser.Match(instruction);
                var letter1 = match.Groups[1].Value[0];
                var letter2 = match.Groups[2].Value[0];
                var index1 = password.IndexOf(letter1);
                var index2 = password.IndexOf(letter2);
                return swapCharsAt(password, index1, index2);
            }
            else if (instruction.StartsWith("rotate based"))
            {
                // 01234567
                // 0 => 70123456
                // 1 => 67012345
                // 2 => 56701234
                // 3 => 45670123
                // 4 => 23456701
                // 5 => 12345670
                // 6 => 01234567
                // 7 => 70123456
                var match = RotateLetterParser.Match(instruction);
                var letter = match.Groups[1].Value[0];
                var resultingIndex = password.IndexOf(letter);

                for (int i = 7; i < password.Length; i--)
                {
                    var rotations = i >= 4 ? i + 2 : i + 1;
                    var result = Rotate(password, false, rotations);
                    if (result[i] == letter)
                        return result;
                }
            }
            else if (instruction.StartsWith("rotate"))
            {
                var match = RotateParser.Match(instruction);
                var direction = match.Groups[1].Value;
                var steps = int.Parse(match.Groups[2].Value);
                return Rotate(password, direction != "right", steps);
            }
            else if (instruction.StartsWith("reverse"))
            {
                var match = ReverseParser.Match(instruction);
                var index1 = int.Parse(match.Groups[1].Value);
                var index2 = int.Parse(match.Groups[2].Value);
                return password.Substring(0, index1) + new string(password.Substring(index1, index2 - index1 + 1).Reverse().ToArray()) + password.Substring(index2 + 1);
            }
            else if (instruction.StartsWith("move"))
            {
                var match = MoveParser.Match(instruction);
                var index2 = int.Parse(match.Groups[1].Value);
                var index1 = int.Parse(match.Groups[2].Value);
                var letter = password[index1];
                var chars = password.ToList();
                chars.Remove(letter);
                chars.Insert(index2, letter);
                return new string(chars.ToArray());
            }

            throw new Exception("Bad instruction: " + instruction);
        }

        private static string Rotate(string password, bool rotateRight, int steps)
        {
            // abcde => eabcd
            // abcde => bcdea
            steps = steps % password.Length;

            if (rotateRight)
            {
                var index = password.Length - steps;
                return password.Substring(index) + password.Substring(0, index);
            }
            else
            {
                var index = steps;
                return password.Substring(index) + password.Substring(0, index);
            }
        }

        private static string swapCharsAt(string value, int index1, int index2)
        {
            var chars = value.ToArray();
            var temp = chars[index1];
            chars[index1] = chars[index2];
            chars[index2] = temp;
            return new string(chars);
        }

        private static readonly string TestProblemInput1 = @"swap position 4 with position 0
swap letter d with letter b
reverse positions 0 through 4
rotate left 1 step
move position 1 to position 4
move position 3 to position 0
rotate based on position of letter b
rotate based on position of letter d";
        private static readonly string TestProblemInput2 = @"qq";
        private static readonly string ProblemInput = @"rotate based on position of letter d
move position 1 to position 6
swap position 3 with position 6
rotate based on position of letter c
swap position 0 with position 1
rotate right 5 steps
rotate left 3 steps
rotate based on position of letter b
swap position 0 with position 2
rotate based on position of letter g
rotate left 0 steps
reverse positions 0 through 3
rotate based on position of letter a
rotate based on position of letter h
rotate based on position of letter a
rotate based on position of letter g
rotate left 5 steps
move position 3 to position 7
rotate right 5 steps
rotate based on position of letter f
rotate right 7 steps
rotate based on position of letter a
rotate right 6 steps
rotate based on position of letter a
swap letter c with letter f
reverse positions 2 through 6
rotate left 1 step
reverse positions 3 through 5
rotate based on position of letter f
swap position 6 with position 5
swap letter h with letter e
move position 1 to position 3
swap letter c with letter h
reverse positions 4 through 7
swap letter f with letter h
rotate based on position of letter f
rotate based on position of letter g
reverse positions 3 through 4
rotate left 7 steps
swap letter h with letter a
rotate based on position of letter e
rotate based on position of letter f
rotate based on position of letter g
move position 5 to position 0
rotate based on position of letter c
reverse positions 3 through 6
rotate right 4 steps
move position 1 to position 2
reverse positions 3 through 6
swap letter g with letter a
rotate based on position of letter d
rotate based on position of letter a
swap position 0 with position 7
rotate left 7 steps
rotate right 2 steps
rotate right 6 steps
rotate based on position of letter b
rotate right 2 steps
swap position 7 with position 4
rotate left 4 steps
rotate left 3 steps
swap position 2 with position 7
move position 5 to position 4
rotate right 3 steps
rotate based on position of letter g
move position 1 to position 2
swap position 7 with position 0
move position 4 to position 6
move position 3 to position 0
rotate based on position of letter f
swap letter g with letter d
swap position 1 with position 5
reverse positions 0 through 2
swap position 7 with position 3
rotate based on position of letter g
swap letter c with letter a
rotate based on position of letter g
reverse positions 3 through 5
move position 6 to position 3
swap letter b with letter e
reverse positions 5 through 6
move position 6 to position 7
swap letter a with letter e
swap position 6 with position 2
move position 4 to position 5
rotate left 5 steps
swap letter a with letter d
swap letter e with letter g
swap position 3 with position 7
reverse positions 0 through 5
swap position 5 with position 7
swap position 1 with position 7
swap position 1 with position 7
rotate right 7 steps
swap letter f with letter a
reverse positions 0 through 7
rotate based on position of letter d
reverse positions 2 through 4
swap position 7 with position 1
swap letter a with letter h";
    }
}