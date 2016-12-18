namespace AdventOfCode
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class Day18
    {
        public static int SolveProblem1()
        {
            Console.WriteLine("Start: " + DateTime.Now);
            //var input = ".^^.^.^^^^";
            //var numberOfRows = 10;
            var input = ".^^^.^.^^^.^.......^^.^^^^.^^^^..^^^^^.^.^^^..^^.^.^^..^.^..^^...^.^^.^^^...^^.^.^^^..^^^^.....^....";
            var numberOfRows = 40;

            var firstRowTraps = input.Select(c => c == '^').ToArray();

            var floor = GetFloor(firstRowTraps, numberOfRows);

            printFloor(floor);

            var safeTiles = floor.Sum(r => r.Sum(t => t ? 0 : 1));

            Console.WriteLine("End: " + DateTime.Now);
            return safeTiles;
        }

        public static int SolveProblem2()
        {
            Console.WriteLine("Start: " + DateTime.Now);
            //var input = ".^^.^.^^^^";
            //var numberOfRows = 10;
            var input = ".^^^.^.^^^.^.......^^.^^^^.^^^^..^^^^^.^.^^^..^^.^.^^..^.^..^^...^.^^.^^^...^^.^.^^^..^^^^.....^....";
            var numberOfRows = 400000;

            var firstRowTraps = input.Select(c => c == '^').ToArray();

            var floor = GetFloor(firstRowTraps, numberOfRows);

            // printFloor(floor);

            var safeTiles = floor.Sum(r => r.Sum(t => t ? 0 : 1));

            Console.WriteLine("End: " + DateTime.Now);
            return safeTiles;
        }

        private static void printFloor(bool[][] floor)
        {
            foreach(var row in floor)
            {
                Console.WriteLine(new String(row.Select(t => t ? '^' : '.').ToArray()));
            }
        }

        private static bool[][] GetFloor(bool[] firstRowTraps, int numberOfRows)
        {
            var ret = new List<bool[]>() {firstRowTraps};

            while (ret.Count < numberOfRows)
            {
                ret.Add(GetNextRow(ret[ret.Count - 1]));
            }

            return ret.ToArray();
        }

        private static bool[] GetNextRow(bool[] previousRow)
        {
            var paddedRow = previousRow.Prepend(false).Append(false).ToArray();
            var paddedNextRow = paddedRow.Select((t,i) => (i > 0 && i < paddedRow.Length - 1) && (
                (paddedRow[i-1] && paddedRow[i] && !paddedRow[i+1]) ||
                (!paddedRow[i-1] && paddedRow[i] && paddedRow[i+1]) ||
                (paddedRow[i-1] && !paddedRow[i] && !paddedRow[i+1]) ||
            (!paddedRow[i-1] && !paddedRow[i] && paddedRow[i+1])));

            return paddedNextRow.Skip(1).Take(previousRow.Length).ToArray();
        }
    }
}