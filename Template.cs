namespace AdventOfCode
{
    using System;
    using System.Linq;

    public class Template
    {
        public static string SolveProblem1()
        {
            Console.WriteLine("Start: " + DateTime.Now);
            var instructions = TestProblemInput1.SplitToLines();

            Console.WriteLine("End: " + DateTime.Now);
            return TestProblemInput1;
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
        // private static readonly string ProblemInput = @"qq";
    }
}