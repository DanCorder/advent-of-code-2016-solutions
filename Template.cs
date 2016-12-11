namespace AdventOfCode
{
    using System.Linq;

    public class Template
    {
        public static string SolveProblem1()
        {
            var instructions = TestProblemInput1.SplitToLines();
            return TestProblemInput1;
        }

        public static string SolveProblem2()
        {
            var instructions = TestProblemInput2.SplitToLines();
            return TestProblemInput2;
        }

        private static readonly string TestProblemInput1 = @"qq";
        private static readonly string TestProblemInput2 = @"qq";
        // private static readonly string ProblemInput = @"qq";
    }
}