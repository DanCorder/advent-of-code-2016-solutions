namespace AdventOfCode
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Collections.Generic;

    public class Day16
    {
        public static string SolveProblem1()
        {
            Console.WriteLine("Start: " + DateTime.Now);
            // var discLength = 20;
            // var initialState = "10000";
            var discLength = 272;
            var initialState = "10111100110001111";

            var data = GenerateData(discLength, initialState);
            Console.WriteLine("Data: " + data);
            var checksum = GenerateFinalChecksum(data);

            Console.WriteLine("End: " + DateTime.Now);
            return checksum;
        }

        private static string GenerateFinalChecksum(string data)
        {
            IList<char> checksum = data.ToList();
            do
            {
                checksum = GenerateChecksum(checksum);
            } while (checksum.Count % 2 == 0);

            return new string(checksum.ToArray());
        }

        private static IList<char> GenerateChecksum(IList<char> data)
        {
            var ret = new List<char>();
            for (var i = 0; i < data.Count; i += 2)
            {
                ret.Add(data[i] == data[i+1] ? '1' : '0');
            }
            return ret;
        }

        private static string GenerateData(int discLength, string initialState)
        {
            var data = new StringBuilder(initialState);
            while (data.Length < discLength)
            {
                var b = new string(data.ToString().Reverse().Select(c => c == '0' ? '1' : '0').ToArray());
                data.Append('0');
                data.Append(b);
            }

            return data.ToString().Substring(0, discLength);
        }

        public static string SolveProblem2()
        {
            Console.WriteLine("Start: " + DateTime.Now);
            var instructions = TestProblemInput2.SplitToLines();

            Console.WriteLine("End: " + DateTime.Now);
            return TestProblemInput2;
        }

        private static readonly string TestProblemInput1 = @"qq";
        private static readonly string TestProblemInput2 = @"qq";
        // private static readonly string ProblemInput = @"10111100110001111";
    }
}