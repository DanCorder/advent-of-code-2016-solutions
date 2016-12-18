namespace AdventOfCode
{
    using System;
    using System.Collections.Generic;

    public class Day15
    {
        public static int SolveProblem1()
        {
            Console.WriteLine("Start: " + DateTime.Now);
            // var discs = GetTestDiscs();
            var discs = GetProblem1Discs();

            return SolveProblem(discs);
        }

        public static int SolveProblem2()
        {
            Console.WriteLine("Start: " + DateTime.Now);
            // var discs = GetTestDiscs();
            var discs = GetProblem2Discs();

            return SolveProblem(discs);
        }

        private static int SolveProblem(List<Disc> discs)
        {
            var releaseTime = 0;
            var bounced = false;

            while(true)
            {
                bounced = false;
                for (var discIndex = 0; discIndex < discs.Count; discIndex++)
                {
                    if (!discs[discIndex].CanPassAtTime(releaseTime + discIndex + 1))
                    {
                        bounced = true;
                        break;
                    }
                }

                if (!bounced)
                {
                    Console.WriteLine("End: " + DateTime.Now);
                    return releaseTime;
                }
                releaseTime++;
            }
        }

        // Disc #1 has 5 positions; at time=0, it is at position 4.
        // Disc #2 has 2 positions; at time=0, it is at position 1.
        private static List<Disc> GetTestDiscs()
        {
            return new List<Disc> {
                new Disc(5, 4),
                new Disc(2, 1)
            };
        }

        // Disc #1 has 17 positions; at time=0, it is at position 1.
        // Disc #2 has 7 positions; at time=0, it is at position 0.
        // Disc #3 has 19 positions; at time=0, it is at position 2.
        // Disc #4 has 5 positions; at time=0, it is at position 0.
        // Disc #5 has 3 positions; at time=0, it is at position 0.
        // Disc #6 has 13 positions; at time=0, it is at position 5.
        private static List<Disc> GetProblem1Discs()
        {
            return new List<Disc> {
                new Disc(17, 1),
                new Disc(7, 0),
                new Disc(19, 2),
                new Disc(5, 0),
                new Disc(3, 0),
                new Disc(13, 5),
            };
        }

        // Disc #1 has 17 positions; at time=0, it is at position 1.
        // Disc #2 has 7 positions; at time=0, it is at position 0.
        // Disc #3 has 19 positions; at time=0, it is at position 2.
        // Disc #4 has 5 positions; at time=0, it is at position 0.
        // Disc #5 has 3 positions; at time=0, it is at position 0.
        // Disc #6 has 13 positions; at time=0, it is at position 5.
        // Disc #6 has 11 positions; at time=0, it is at position 0.
        private static List<Disc> GetProblem2Discs()
        {
            return new List<Disc> {
                new Disc(17, 1),
                new Disc(7, 0),
                new Disc(19, 2),
                new Disc(5, 0),
                new Disc(3, 0),
                new Disc(13, 5),
                new Disc(11, 0)
            };
        }

        private class Disc
        {
            public Disc(int numberOfPositions, int startingPosition)
            {
                this.startingPosition = startingPosition;
                this.numberOfPositions = numberOfPositions;
            }

            public bool CanPassAtTime(int time)
            {
                var currentPosition = (startingPosition + time) % numberOfPositions;
                return currentPosition == 0;
            }

            private readonly int startingPosition;
            private readonly int numberOfPositions;
        }
    }
}