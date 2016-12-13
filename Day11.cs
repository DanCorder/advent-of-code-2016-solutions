namespace AdventOfCode
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class Day11
    {
        private static HashSet<State> PreviousStates = new HashSet<State>();

        public static int SolveProblem1()
        {
            // var initialState = ProblemInput;
            var initialState = TestProblemInput1;;

            Array.Sort(initialState.GeneratorChipPositions);

            var result = BreadthFirstSearch(initialState, GetNextStates, IsEndState);

            return result.Distance;
        }

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
                PreviousStates.Add(currentState);

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

        private static IEnumerable<State> GetNextStates(State currentState)
        {
            var allStates = GetAllPossibleNextStates(currentState);
            return PruneStates(allStates);
        }

        private static IEnumerable<State> PruneStates(IEnumerable<State> allStates)
        {
            return allStates
                .Distinct()
                .Where(IsValidState)
                .Where(s => !PreviousStates.Contains(s));
        }

        private static bool IsValidState(State state)
        {
            return state.GeneratorChipPositions.All(p =>
                p.ChipFloor == p.GeneratorFloor ||
                state.GeneratorChipPositions.All(p2 => p2.GeneratorFloor != p.ChipFloor));
        }

        private static IEnumerable<State> GetAllPossibleNextStates(State currentState)
        {
            var nextStates = new List<State>();
            if (currentState.CurrentFloor != 1)
            {
                nextStates.AddRange(GetStatesToFloor(currentState.CurrentFloor - 1, currentState));
            }
            if (currentState.CurrentFloor != 4)
            {
                nextStates.AddRange(GetStatesToFloor(currentState.CurrentFloor + 1, currentState));
            }
            return nextStates;
        }

        private static IEnumerable<State> GetStatesToFloor(int nextFloor, State currentState)
        {
            var nextStates = new List<State>();
            nextStates.AddRange(GetSingleMoveStatesToFloor(nextFloor, currentState));
            nextStates.AddRange(GetDoubleMoveStatesToFloor(nextFloor, currentState));
            return nextStates;
        }

        private static IEnumerable<State> GetSingleMoveStatesToFloor(int nextFloor, State currentState)
        {
            var nextStates = new List<State>();

            for (var i = 0; i < currentState.GeneratorChipPositions.Length; i++)
            {
                if (currentState.GeneratorChipPositions[i].ChipFloor == currentState.CurrentFloor)
                {
                    var nextState = currentState.Clone();
                    nextState.CurrentFloor = nextFloor;
                    nextState.GeneratorChipPositions[i].ChipFloor = nextFloor;
                    Array.Sort(nextState.GeneratorChipPositions);
                    nextState.Distance = currentState.Distance + 1;
                    nextStates.Add(nextState);
                }
                if (currentState.GeneratorChipPositions[i].GeneratorFloor == currentState.CurrentFloor)
                {
                    var nextState = currentState.Clone();
                    nextState.CurrentFloor = nextFloor;
                    nextState.GeneratorChipPositions[i].GeneratorFloor = nextFloor;
                    Array.Sort(nextState.GeneratorChipPositions);
                    nextState.Distance = currentState.Distance + 1;
                    nextStates.Add(nextState);
                }
            }
            return nextStates;
        }

        private static IEnumerable<State> GetDoubleMoveStatesToFloor(int nextFloor, State currentState)
        {
            var nextStates = new List<State>();

            for (var i = 0; i < currentState.GeneratorChipPositions.Length * 2; i++)
            {
                if ((i % 2 == 0 && currentState.GeneratorChipPositions[i/2].GeneratorFloor == currentState.CurrentFloor) ||
                    (i % 2 == 1 && currentState.GeneratorChipPositions[i/2].ChipFloor == currentState.CurrentFloor))
                {
                    for (var j = i + 1; j < currentState.GeneratorChipPositions.Length * 2; j++)
                    {
                        if ((j % 2 == 0 && currentState.GeneratorChipPositions[j/2].GeneratorFloor == currentState.CurrentFloor) ||
                            (j % 2 == 1 && currentState.GeneratorChipPositions[j/2].ChipFloor == currentState.CurrentFloor))
                        {
                            var nextState = currentState.Clone();
                            nextState.CurrentFloor = nextFloor;

                            if (i%2 == 0)
                            {
                                nextState.GeneratorChipPositions[i/2].GeneratorFloor = nextFloor;
                            }
                            else
                            {
                                nextState.GeneratorChipPositions[i/2].ChipFloor = nextFloor;
                            }
                            if (j%2 == 0)
                            {
                                nextState.GeneratorChipPositions[j/2].GeneratorFloor = nextFloor;
                            }
                            else
                            {
                                nextState.GeneratorChipPositions[j/2].ChipFloor = nextFloor;
                            }

                            Array.Sort(nextState.GeneratorChipPositions);
                            nextState.Distance = currentState.Distance + 1;
                            nextStates.Add(nextState);
                        }
                    }
                }
            }
            return nextStates;
        }

        private static bool IsEndState(State state)
        {
            return state.GeneratorChipPositions.All(t => t.GeneratorFloor == 4 && t.ChipFloor == 4);
        }

        public static string SolveProblem2()
        {
            var instructions = TestProblemInput2.SplitToLines();
            return TestProblemInput2;
        }

        private struct State
        {
            public int CurrentFloor;
            public Position[] GeneratorChipPositions;
            public int Distance;

            public State Clone()
            {
                var clone = new State() {
                    CurrentFloor = this.CurrentFloor,
                    Distance = this.Distance,
                    GeneratorChipPositions = new Position[this.GeneratorChipPositions.Length]
                };
                Array.Copy(this.GeneratorChipPositions,
                    clone.GeneratorChipPositions,
                    this.GeneratorChipPositions.Length);

                return clone;
            }

            // qq Implement gethashcode
        }

        private struct Position : IComparable<Position>
        {
            public int GeneratorFloor;
            public int ChipFloor;

            int IComparable<Position>.CompareTo(Position other)
            {
                if (other.GeneratorFloor != this.GeneratorFloor)
                {
                    return other.GeneratorFloor - this.GeneratorFloor;
                }
                return other.ChipFloor - this.ChipFloor;
            }
        }

        private static readonly State TestProblemInput1 = new State {
            CurrentFloor = 1,
            Distance = 0,
            GeneratorChipPositions = new Position[] {
                new Position() { GeneratorFloor = 3, ChipFloor = 1 }, // Lithium
                new Position() { GeneratorFloor = 2, ChipFloor = 1 } // Hydrogen
            }
        };
        private static readonly string TestProblemInput2 = @"qq";
        private static readonly State ProblemInput = new State {
            CurrentFloor = 1,
            Distance = 0,
            GeneratorChipPositions = new Position[] {
                new Position() { GeneratorFloor = 1, ChipFloor = 2 }, // Polonium
                new Position() { GeneratorFloor = 1, ChipFloor = 2 }, // Prometheum
                new Position() { GeneratorFloor = 1, ChipFloor = 1 }, // Thulium
                new Position() { GeneratorFloor = 1, ChipFloor = 1 }, // Ruthenium
                new Position() { GeneratorFloor = 1, ChipFloor = 1 } // Cobalt
            }
        };
    }
}