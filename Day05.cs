namespace AdventOfCode
{
    using System.Security.Cryptography;
    using System.Text;

    public class Day05
    {
        public static string SolveProblem1()
        {
            var code = "";
            var index = 0;

            while (code.Length < 8)
            {
                var hash = CalculateMD5Hash(ProblemInput + index);

                if (hash.StartsWith("00000"))
                {
                    code += hash[5];
                    System.Console.WriteLine("Found char: " + hash[5]);
                }

                index++;
            }

            return code;
        }

        public static string SolveProblem2()
        {
            var code = new StringBuilder("________");
            var index = 0;

            while (code.ToString().Contains("_"))
            {
                var hash = CalculateMD5Hash(ProblemInput + index);

                if (hash.StartsWith("00000"))
                {
                    var position = 0;

                    if (int.TryParse(hash[5].ToString(), out position))
                    {
                        if (position >= 0 && position < 8 && code[position] == '_')
                        {
                            code[position] = hash[6];
                            System.Console.WriteLine("Found char: " + hash[6] + " at position: " + hash[5]);
                        }
                    }
                }

                index++;
            }

            return code.ToString();
        }

        private static readonly string ProblemInput = "wtnhxymk";
        // private static readonly string ProblemInput = "abc";

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
    }
}