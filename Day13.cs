namespace AdventOfCode
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class Day13
    {
        enum SquareState
        {
            unexplored = 0,
            wall,
            space
        }

        private static SquareState[,] Maze = new SquareState[1000,1000];
        private static bool[,] VisitedSpaces = new bool[1000,1000];

        public static int SolveProblem1()
        {
            var start = new State { XPos = 1, YPos = 1, Distance = 0};

            var result = BreadthFirstSearch(start, GetNextStates, IsEndState);

            return result.Distance;
        }

        private static IEnumerable<State> GetNextStates(State currentState)
        {
            var nextStates = new List<State>();
            TryAddState(currentState, nextStates, currentState.XPos - 1, currentState.YPos);
            TryAddState(currentState, nextStates, currentState.XPos, currentState.YPos - 1);
            TryAddState(currentState, nextStates, currentState.XPos + 1, currentState.YPos);
            TryAddState(currentState, nextStates, currentState.XPos, currentState.YPos + 1);
            return nextStates;
        }

        private static void TryAddState(State currentState, List<State> states, int xPos, int yPos)
        {
            if (xPos < 0 || yPos < 0)
                return;

            if (VisitedSpaces[xPos, yPos])
                return;

            VisitedSpaces[xPos,yPos] = true;

            if (IsWall(xPos, yPos))
                return;

            var nextState = new State {
                XPos = xPos,
                YPos = yPos,
                Distance = currentState.Distance + 1};
            states.Add(nextState);
        }

        private static bool IsWall(int xPos, int yPos)
        {
            if (Maze[xPos, yPos] == SquareState.unexplored)
            {
                Maze[xPos, yPos] = CalculateIsWall(xPos, yPos) ? SquareState.wall : SquareState.space;
            }

            return Maze[xPos, yPos] == SquareState.wall;
        }

        private static bool CalculateIsWall(int xPos, int yPos)
        {
            var number = (xPos * xPos) + (3*xPos) + (2*xPos*yPos) + yPos + (yPos*yPos) + ProblemInput;
            var numberOfOnes = CalculateOnes(number);
            return (numberOfOnes % 2) == 1;
        }

        private static int CalculateOnes(int number)
        {
            var ret = 0;

            while (number > 0)
            {
                var digit = number % 2;
                if (digit == 1)
                    ret++;
                number = number / 2;
            }

            return ret;
        }

        private static bool IsEndState(State state)
        {
            return state.XPos == 31 && state.YPos == 39;
            // return state.XPos == 7 && state.YPos == 4;
        }

        // public static string SolveProblem2()
        // {
        //     var instructions = TestProblemInput2.SplitToLines();
        //     return TestProblemInput2;
        // }

        private static State BreadthFirstSearch(
            State startingState,
            Func<State, IEnumerable<State>> getNextStates,
            Func<State, bool> isEndState)
        {
            State finalState = startingState;
            var stateQueue = new Queue<State>();
            stateQueue.Enqueue(startingState);
            var currentDepth = -1;
            while (stateQueue.Count > 0)
            {
                var currentState = stateQueue.Dequeue();
                VisitedSpaces[currentState.XPos, currentState.YPos] = true;

                if (currentDepth != currentState.Distance)
                {
                    currentDepth = currentState.Distance;
                    Console.WriteLine("Depth: " + currentDepth + " Number of moves to check: " + (stateQueue.Count + 1) + " " + DateTime.Now);
                }

                if (isEndState(currentState))
                {
                    finalState = currentState;
                    break;
                }

                var nextStates = getNextStates(currentState);

                foreach (var nextState in nextStates)
                    stateQueue.Enqueue(nextState);
            }

            return finalState;
        }

        private struct State
        {
            public int XPos;
            public int YPos;
            public int Distance;

            public State Clone()
            {
                var clone = new State() {
                    XPos = this.XPos,
                    YPos = this.YPos,
                    Distance = this.Distance,
                };
                return clone;
            }
        }

        // private static readonly int ProblemInput = 10;
        private static readonly int ProblemInput = 1358;
    }
}