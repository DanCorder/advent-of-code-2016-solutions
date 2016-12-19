namespace AdventOfCode
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class Day19
    {
        public static int SolveProblem1()
        {
            Console.WriteLine("Start: " + DateTime.Now);
            var elvesList = Enumerable.Range(1, 3005290).ToList();
            var elves = new LinkedList<int>(elvesList);

            var currentElf = elves.First;

            while (elves.Count > 1)
            {
                var nextNode = currentElf.Next;
                if (nextNode == null)
                    nextNode = elves.First;

                elves.Remove(nextNode);

                currentElf = currentElf.Next;
                if (currentElf == null)
                    currentElf = elves.First;

                if (elves.Count % 10000 == 0)
                {
                    Console.WriteLine("Time: " + DateTime.Now);
                    Console.WriteLine("Elves left: " + elves.Count);
                }
            }


            Console.WriteLine("End: " + DateTime.Now);
            return elves.First.Value;
        }

        public static string SolveProblem2()
        {
            Console.WriteLine("Start: " + DateTime.Now);
            var instructions = TestProblemInput2.SplitToLines();

            Console.WriteLine("End: " + DateTime.Now);
            return TestProblemInput2;
        }
        private static readonly string TestProblemInput2 = @"qq";
    }
}