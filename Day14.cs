namespace AdventOfCode
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Security.Cryptography;
    using System.Collections.Generic;

    public class Day14
    {
        public static int SolveProblem1()
        {
            Console.WriteLine("Start: " + DateTime.Now);
            // 23769
            var key = @"cuanljph";
            // var key = "abc";
            var index = 1; // Tested 0 manually it's not relevant
            var foundTriples = new Dictionary<int, char>();
            var foundKeys = new List<int>();

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
                        Console.WriteLine("Found key for char " + triple.Value + " at " + triple.Key);
                        foundKeys.Add(triple.Key);
                        foundKeys.Sort();
                        if (foundKeys.Count >= 64 && index > foundKeys[63] + 1000)
                        {
                            Console.WriteLine("End: " + DateTime.Now);
                            return foundKeys[63];
                        }
                        foundTriples.Remove(triple.Key);
                    }
                }
                if (tripleChar != null)
                {
                    // Console.WriteLine("Found triple " + tripleChar.Value + " at " + index);
                    foundTriples.Add(index, tripleChar.Value);
                }

                index++;
            }
        }

        public static int SolveProblem2()
        {
            Console.WriteLine("Start: " + DateTime.Now);
            // 20606
            var key = @"cuanljph";
            // var key = "abc";

            var index = 1; // Tested 0 manually it's not relevant
            var foundTriples = new Dictionary<int, char>();
            var foundKeys = new List<int>();

            while (true)
            {
                if (foundTriples.ContainsKey(index - 1001))
                {
                    foundTriples.Remove(index - 1001);
                }

                var hash = CalculateMD5Hash2017(key + index);

                var tripleChar = GetTripleChar(hash);
                var quintupleChars = GetQuintupleChars(hash).ToArray();

                foreach (var triple in foundTriples.OrderBy(p => p.Key))
                {
                    if (quintupleChars.Contains(triple.Value))
                    {
                        Console.WriteLine("Found key for char " + triple.Value + " at " + triple.Key);
                        foundKeys.Add(triple.Key);
                        foundKeys.Sort();
                        if (foundKeys.Count >= 64 && index > foundKeys[63] + 1000)
                        {
                            Console.WriteLine("End: " + DateTime.Now);
                            return foundKeys[63];
                        }
                        foundTriples.Remove(triple.Key);
                    }
                }
                if (tripleChar != null)
                {
                    // Console.WriteLine("Found triple " + tripleChar.Value + " at " + index);
                    foundTriples.Add(index, tripleChar.Value);
                }

                index++;
            }
        }

        private static IEnumerable<char> GetQuintupleChars(string hash)
        {
            var chars = new List<char>();
            var match = QuintupleMatcher.Match(hash);
            if (match.Success)
            {
                for (var i = 1; i < match.Groups.Count; i++)
                    chars.Add(match.Groups[i].Value[0]);
            }
            return chars.Distinct();
        }

        private static readonly Regex TripleMatcher =
            new Regex(@"(.)\1{2,}", RegexOptions.Compiled);

        private static readonly Regex QuintupleMatcher =
            new Regex(@"(.)\1{4,}", RegexOptions.Compiled);

        private static char? GetTripleChar(string hash)
        {
            var match = TripleMatcher.Match(hash);
            if (match.Success)
            {
                return match.Groups[1].Value[0];
            }
            return null;
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

        private static string CalculateMD5Hash2017(string input)
        {
            var stretchedHash = input;
            for (var i = 0; i < 2017; i++)
            {
                // Console.WriteLine(stretchedHash);
                stretchedHash = CalculateMD5Hash(stretchedHash.ToLower());
            }
            return stretchedHash;
        }
    }
}