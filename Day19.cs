namespace AdventOfCode
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class Day19
    {
        public static int SolveProblem1() // 1816277
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

                // if (elves.Count % 10000 == 0)
                // {
                //     Console.WriteLine("Time: " + DateTime.Now);
                //     Console.WriteLine("Elves left: " + elves.Count);
                // }
            }


            Console.WriteLine("End: " + DateTime.Now);
            return elves.First.Value;
        }

        public static int SolveProblem2() // 1410967
        {
            Console.WriteLine("Start: " + DateTime.Now);
            var elvesList = Enumerable.Range(1, 3005290).ToList();
            // var elvesList = Enumerable.Range(1, 5).ToList();
            var elves = new LinkedList<int>(elvesList);

            var currentElf = elves.First;
            var currentSteal = GetElfToStealFrom(elves, currentElf);

            while (elves.Count > 1)
            {
                var nextSteal = currentSteal.Next;
                if (nextSteal == null)
                    nextSteal = elves.First;
                if (elves.Count % 2 == 1)
                {
                    nextSteal = nextSteal.Next;
                    if (nextSteal == null)
                        nextSteal = elves.First;
                }

                elves.Remove(currentSteal);

                currentElf = currentElf.Next;
                if (currentElf == null)
                    currentElf = elves.First;

                currentSteal = nextSteal;

                // if (elves.Count % 10000 == 0)
                // {
                //     Console.WriteLine("Time: " + DateTime.Now);
                //     Console.WriteLine("Elves left: " + elves.Count);
                // }
            }


            Console.WriteLine("End: " + DateTime.Now);
            return elves.First.Value;
        }

        private static LinkedListNode<int> GetElfToStealFrom(LinkedList<int> elves, LinkedListNode<int> currentElf)
        {
            int offset = elves.Count / 2;
            var ret = currentElf;

            for (var i = 0; i < offset; i++)
            {
                ret = ret.Next;
                if (ret == null)
                    ret = elves.First;
            }
            return ret;
        }
    }
}