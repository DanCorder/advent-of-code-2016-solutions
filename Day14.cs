namespace AdventOfCode
{
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Security.Cryptography;
    using System.Collections.Generic;

    public class Day14
    {
        public static int SolveProblem1()
        {
            //24846 too high
            var key = @"cuanljph";
            // var key = "abc";
            var index = 1; // Test 0 manually it's not relevant
            var foundTriples = new Dictionary<int, char>();
            var foundKeys = new List<int>();

            // for (var i = 1; i < 10000; i++)
            // {
            //     var hash = CalculateMD5Hash(key + i);
            //     var tripleChar = GetTripleChar(hash);

            //     if (tripleChar != null)
            //     {
            //         System.Console.WriteLine(hash + " " + tripleChar);
            //     }
            // }


            while (true)
            {
                if (foundTriples.ContainsKey(index - 1001))
                {
                    foundTriples.Remove(index - 1001);
                }

                var hash = CalculateMD5Hash(key + index);

                var tripleChar = GetTripleChar(hash);
                var quintupleChars = GetQuintupleChars(hash).ToArray();

                foreach (var triple in foundTriples.OrderBy(p => p.Key))
                {
                    if (quintupleChars.Contains(triple.Value))
                    {
                        System.Console.WriteLine("Found key for char " + triple.Value + " at " + triple.Key);
                        foundKeys.Add(triple.Key);
                        foundKeys.Sort();
                        if (foundKeys.Count >= 64 && index > foundKeys[63] + 1000)
                        {
                            return foundKeys[63];
                        }
                        foundTriples.Remove(triple.Key);
                    }
                }
                if (tripleChar != null)
                {
                    System.Console.WriteLine("Found triple " + tripleChar.Value + " at " + index);
                    foundTriples.Add(index, tripleChar.Value);
                }

                index++;
            }
            // return -1;
        }

        private static IEnumerable<char> GetQuintupleChars(string hash)
        {
            // var chars = new List<char>();
            // var match = QuintupleMatcher.Match(hash);
            // if (match.Success)
            // {
            //     for (var i = 1; i < match.Groups.Count; i++)

            //     chars.Add(match.Groups[i].Value[0]);
            // }
            // return chars;
            return hash.Where((c, i) => i >= 4 && hash.Substring(i - 4, 4).All(ssc => ssc == c)).Distinct();
        }

        private static readonly Regex TripleMatcher =
            new Regex(@"(.)\1{2,2}", RegexOptions.Compiled);

        private static readonly Regex QuintupleMatcher =
            new Regex(@"(.)\1{4,4}", RegexOptions.Compiled);

        private static char? GetTripleChar(string hash)
        {
            var triples = hash.Where((c, i) => i >= 2 && hash.Substring(i - 2, 2).All(ssc => ssc == c)).Distinct().ToList();
            return triples.Count > 0 ? (char?)triples[0] : null;


            // var match = TripleMatcher.Match(hash);
            // if (match.Success)
            // {
            //     return match.Groups[1].Value[0];
            // }
            // return null;
        }

        public static string SolveProblem2()
        {
            var instructions = TestProblemInput2.SplitToLines();
            return TestProblemInput2;
        }

        private static readonly MD5 md5 = System.Security.Cryptography.MD5.Create();

        private static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private static readonly string TestProblemInput1 = @"abc";
        private static readonly string TestProblemInput2 = @"qq";
        private static readonly string ProblemInput = @"cuanljph";
    }
}