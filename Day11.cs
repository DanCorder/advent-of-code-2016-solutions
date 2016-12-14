namespace AdventOfCode
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class Day11
    {
        private static HashSet<State> seenStates = new HashSet<State>();
        // private const int NUMBER_OF_ELEMENTS = 2;
        // private const int NUMBER_OF_ELEMENTS = 5;
        private const int NUMBER_OF_ELEMENTS = 7;


        public static int SolveProblem1()
        {
            var ProblemInput = new State {
                CurrentFloor = 0,
                Positions = 0
            };

            ProblemInput.SetPositions(new Position[] {
                    new Position() { GeneratorFloor = 0, ChipFloor = 1 }, // Polonium
                    new Position() { GeneratorFloor = 0, ChipFloor = 1 }, // Prometheum
                    new Position() { GeneratorFloor = 0, ChipFloor = 0 }, // Thulium
                    new Position() { GeneratorFloor = 0, ChipFloor = 0 }, // Ruthenium
                    new Position() { GeneratorFloor = 0, ChipFloor = 0 } // Cobalt
                }
            );
            // ProblemInput.SetPositions(new Position[] {
            //         new Position() { GeneratorFloor = 2, ChipFloor = 0 }, // Lithium
            //         new Position() { GeneratorFloor = 1, ChipFloor = 0 } // Hydrogen
            //     }
            // );

            return BreadthFirstSearch(ProblemInput, GetNextStates, IsEndState);
        }

        public static int SolveProblem2()
        {
            var ProblemInput = new State {
                CurrentFloor = 0,
                Positions = 0
            };

            ProblemInput.SetPositions(new Position[] {
                    new Position() { GeneratorFloor = 0, ChipFloor = 1 }, // Polonium
                    new Position() { GeneratorFloor = 0, ChipFloor = 1 }, // Prometheum
                    new Position() { GeneratorFloor = 0, ChipFloor = 0 }, // Thulium
                    new Position() { GeneratorFloor = 0, ChipFloor = 0 }, // Ruthenium
                    new Position() { GeneratorFloor = 0, ChipFloor = 0 }, // Cobalt
                    new Position() { GeneratorFloor = 0, ChipFloor = 0 }, // Elerium
                    new Position() { GeneratorFloor = 0, ChipFloor = 0 }  // Dilithium
                }
            );

            return BreadthFirstSearch(ProblemInput, GetNextStates, IsEndState);
        }

        private static int BreadthFirstSearch(
            State startingState,
            Func<State, IEnumerable<State>> getNextStates,
            Func<State, bool> isEndState)
        {
            State finalState = startingState;
            var nextStateQueue = new List<State>();
            nextStateQueue.Add(startingState);
            seenStates.Add(startingState);

            var currentDepth = 0;
            while (nextStateQueue.Count > 0)
            {
                Console.WriteLine("Depth: " + currentDepth + " Number of moves to check: " + nextStateQueue.Count + " " + DateTime.Now);

                var stateQueue = nextStateQueue;
                nextStateQueue = new List<State>();
                currentDepth++;

                foreach (var currentState in stateQueue)
                {
                    var nextStates = getNextStates(currentState);

                    if (nextStates.Any(IsEndState))
                        return currentDepth;

                    foreach(var nextState in nextStates)
                    {
                        seenStates.Add(nextState);
                        nextStateQueue.Add(nextState);
                    }
                }
            }

            return -1;
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
                .Where(s => !seenStates.Contains(s));
        }

        private static bool IsValidState(State state)
        {
            var positions = state.DecompressPositions();
            return positions.All(p =>
                p.ChipFloor == p.GeneratorFloor ||
                positions.All(p2 => p2.GeneratorFloor != p.ChipFloor));
        }

        private static IEnumerable<State> GetAllPossibleNextStates(State currentState)
        {
            var nextStates = new List<State>();
            if (currentState.CurrentFloor != 0)
            {
                nextStates.AddRange(GetStatesToFloor(currentState.CurrentFloor - 1, currentState));
            }
            if (currentState.CurrentFloor != 3)
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
            var positions = currentState.DecompressPositions();

            for (var i = 0; i < positions.Count; i++)
            {
                if (positions[i].ChipFloor == currentState.CurrentFloor)
                {
                    var nextState = currentState.Clone();
                    nextState.CurrentFloor = nextFloor;
                    var nextPositions = positions.Select(p => p.Clone()).ToArray();
                    nextPositions[i].ChipFloor = nextFloor;
                    nextState.SetPositions(nextPositions);
                    nextStates.Add(nextState);
                }
                if (positions[i].GeneratorFloor == currentState.CurrentFloor)
                {
                    var nextState = currentState.Clone();
                    nextState.CurrentFloor = nextFloor;
                    var nextPositions = positions.Select(p => p.Clone()).ToArray();
                    nextPositions[i].GeneratorFloor = nextFloor;
                    nextState.SetPositions(nextPositions);
                    nextStates.Add(nextState);
                }
            }
            return nextStates;
        }

        private static IEnumerable<State> GetDoubleMoveStatesToFloor(int nextFloor, State currentState)
        {
            var positions = currentState.DecompressPositions();
            var nextStates = new List<State>();

            for (var i = 0; i < positions.Count * 2; i++)
            {
                if ((i % 2 == 0 && positions[i/2].GeneratorFloor == currentState.CurrentFloor) ||
                    (i % 2 == 1 && positions[i/2].ChipFloor == currentState.CurrentFloor))
                {
                    for (var j = i + 1; j < positions.Count * 2; j++)
                    {
                        if ((j % 2 == 0 && positions[j/2].GeneratorFloor == currentState.CurrentFloor) ||
                            (j % 2 == 1 && positions[j/2].ChipFloor == currentState.CurrentFloor))
                        {
                            var nextState = currentState.Clone();
                            nextState.CurrentFloor = nextFloor;
                            var nextPositions = positions.Select(p => p.Clone()).ToArray();

                            if (i%2 == 0)
                            {
                                nextPositions[i/2].GeneratorFloor = nextFloor;
                            }
                            else
                            {
                                nextPositions[i/2].ChipFloor = nextFloor;
                            }
                            if (j%2 == 0)
                            {
                                nextPositions[j/2].GeneratorFloor = nextFloor;
                            }
                            else
                            {
                                nextPositions[j/2].ChipFloor = nextFloor;
                            }

                            nextState.SetPositions(nextPositions);
                            nextStates.Add(nextState);
                        }
                    }
                }
            }
            return nextStates;
        }

        private static bool IsEndState(State state)
        {
            return state.DecompressPositions().All(t => t.GeneratorFloor == 3 && t.ChipFloor == 3);
        }

        private struct State
        {
            public int CurrentFloor;
            public int Positions;

            public IList<Position> DecompressedPositions => DecompressPositions();

            public IList<Position> DecompressPositions()
            {
                var ret = new List<Position>();

                for (int i = 0; i < NUMBER_OF_ELEMENTS; i++)
                {
                    ret.Add(new Position() { GeneratorFloor = GetFloorAt(2*i), ChipFloor = GetFloorAt(2*i + 1)});
                }

                return ret;
            }

            public void SetPositions(Position[] positions)
            {
                Array.Sort(positions);

                for (int i = 0; i < NUMBER_OF_ELEMENTS; i++)
                {
                    SetFloorAt(2*i, positions[i].GeneratorFloor);
                    SetFloorAt(2*i + 1, positions[i].ChipFloor);
                }
            }

            private int GetFloorAt(int index)
            {
                int mask = 3; // 11 in binary
                mask = mask << (index * 2);
                var maskedPostions = Positions & mask;
                return maskedPostions >> (index * 2);
            }

            private void SetFloorAt(int index, int floor)
            {
                var floorInPosition = floor << (index * 2);
                int mask = 3; // 11 in binary
                mask = ~(mask << (index * 2));
                Positions = Positions & mask;
                Positions = Positions | floorInPosition;
            }

            public State Clone()
            {
                return new State() {
                    CurrentFloor = this.CurrentFloor,
                    Positions = this.Positions
                };
            }
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

            public Position Clone()
            {
                return new Position() {
                    GeneratorFloor = this.GeneratorFloor,
                    ChipFloor = this.ChipFloor
                };
            }
        }
    }
}