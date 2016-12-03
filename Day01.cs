namespace AdventOfCode
{
    using System;
    using System.Linq;
    public class Day01
    {
        private const string Problem1Input = "L5, R1, R3, L4, R3, R1, L3, L2, R3, L5, L1, L2, R5, L1, R5, R1, L4, R1, R3, L4, L1, R2, R5, R3, R1, R1, L1, R1, L1, L2, L1, R2, L5, L188, L4, R1, R4, L3, R47, R1, L1, R77, R5, L2, R1, L2, R4, L5, L1, R3, R187, L4, L3, L3, R2, L3, L5, L4, L4, R1, R5, L4, L3, L3, L3, L2, L5, R1, L2, R5, L3, L4, R4, L5, R3, R4, L2, L1, L4, R1, L3, R1, R3, L2, R1, R4, R5, L3, R5, R3, L3, R4, L2, L5, L1, L1, R3, R1, L4, R3, R3, L2, R5, R4, R1, R3, L4, R3, R3, L2, L4, L5, R1, L4, L5, R4, L2, L1, L3, L3, L5, R3, L4, L3, R5, R4, R2, L4, R2, R3, L3, R4, L1, L3, R2, R1, R5, L4, L5, L5, R4, L5, L2, L4, R4, R4, R1, L3, L2, L4, R3";

        private enum Heading
        {
            N = 0,
            E = 1,
            S = 2,
            W = 3
        }

        private class Position
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        public static int SolveProblem1()
        {
            var directions = Problem1Input.Split(',')
                                          .Select(d => d.Trim())
                                          .Select(d => new { turn = d[0], move = int.Parse(d.Substring(1)) });
            var position = new Position { X = 0, Y = 0};
            var heading = Heading.N;

            foreach(var direction in directions)
            {
                var newHeading = turn(heading, direction.turn);
                var newPosition = move(position, newHeading, direction.move);
                heading = newHeading;
                position = newPosition;
            }

            return Math.Abs(position.X) + Math.Abs(position.Y);
        }

        private static Position move(Position position, Heading heading, int move)
        {
            if (heading == Heading.N)
            {
                return new Position { X = position.X, Y = position.Y + move };
            }
            if (heading == Heading.S)
            {
                return new Position { X = position.X, Y = position.Y - move };
            }
            if (heading == Heading.E)
            {
                return new Position { X = position.X + move, Y = position.Y };
            }
            return new Position { X = position.X - move, Y = position.Y };
        }

        private static Heading turn(Heading currentHeading, char turn)
        {
            if (currentHeading == Heading.N && turn == 'L')
            {
                return Heading.W;
            }
            if (currentHeading == Heading.W && turn == 'R')
            {
                return Heading.N;
            }
            if (turn == 'L')
            {
                return currentHeading - 1;
            }
            return currentHeading + 1;
        }
    }
}