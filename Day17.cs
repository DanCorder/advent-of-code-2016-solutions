namespace AdventOfCode
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Security.Cryptography;
    using System.Collections.Generic;

    public class Day17
    {
        private static string PASSCODE = "gdjjyniy";

        public static string SolveProblem1()
        {
            Console.WriteLine("Start: " + DateTime.Now);
            // PASSCODE = "ihgpwlah"; //test 1
            // PASSCODE = "kglvqrro"; //test 2
            // PASSCODE = "ulqzkmiv"; //test 3

            var result = BreadthFirstSearch(new State(), GetNextStates, IsEndState);

            Console.WriteLine("End: " + DateTime.Now);
            return result.path;
        }

        public static int SolveProblem2()
        {
            Console.WriteLine("Start: " + DateTime.Now);

            var result = BreadthFirstSearch2(new State(), GetNextStates, IsEndState);

            Console.WriteLine("End: " + DateTime.Now);
            return result;
        }

        private static State BreadthFirstSearch(
            State startingState,
            Func<State, IEnumerable<State>> getNextStates,
            Func<State, bool> isEndState)
        {
            var nextStateQueue = new List<State>();
            nextStateQueue.Add(startingState);

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

                    foreach(var nextState in nextStates)
                    {
                        if (IsEndState(nextState))
                        {
                            return nextState;
                        }
                        nextStateQueue.Add(nextState);
                    }
                }
            }

            return startingState;
        }

        // depth first would be quicker, but I already have this code and it's still very fast.
        private static int BreadthFirstSearch2(
            State startingState,
            Func<State, IEnumerable<State>> getNextStates,
            Func<State, bool> isEndState)
        {
            int deepestSolution = 0;
            var nextStateQueue = new List<State>();
            nextStateQueue.Add(startingState);

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

                    foreach(var nextState in nextStates)
                    {
                        if (IsEndState(nextState))
                        {
                            deepestSolution = currentDepth;
                        }
                        else
                            nextStateQueue.Add(nextState);
                    }
                }
            }

            return deepestSolution;
        }

        private static IEnumerable<State> GetNextStates(State currentState)
        {
            var nextStates = new List<State>();
            var validDirections = GetOpenDoors(currentState);

            if (validDirections[0])
            {
                nextStates.Add(new State { xPos = currentState.xPos, yPos = currentState.yPos - 1, path = currentState.path + "U" });
            }
            if (validDirections[1])
            {
                nextStates.Add(new State { xPos = currentState.xPos, yPos = currentState.yPos + 1, path = currentState.path + "D" });
            }
            if (validDirections[2])
            {
                nextStates.Add(new State { xPos = currentState.xPos - 1, yPos = currentState.yPos, path = currentState.path + "L" });
            }
            if (validDirections[3])
            {
                nextStates.Add(new State { xPos = currentState.xPos + 1, yPos = currentState.yPos, path = currentState.path + "R" });
            }

            return nextStates;
        }

        private static bool[] GetOpenDoors(State state)
        {
            var doors = GetDoorStates(state);
            var walls = GetWalls(state);
            return doors.Zip(walls, (d,w) => w ? false : d).ToArray();
        }

        // U D L R
        private static bool[] GetWalls(State state)
        {
            var walls = new bool[4];
            if (state.xPos == 0)
            {
                walls[2] = true;
            }
            if (state.xPos == 3)
            {
                walls[3] = true;
            }
            if (state.yPos == 0)
            {
                walls[0] = true;
            }
            if (state.yPos == 3)
            {
                walls[1] = true;
            }
            return walls;
        }

        private static bool[] GetDoorStates(State state)
        {
            var hash = CalculateMD5Hash(PASSCODE + state.path);
            return hash.Take(4).Select(c => c == 'B' || c == 'C' || c == 'D' || c == 'E' || c == 'F').ToArray();
        }

        private static bool IsEndState(State state)
        {
            return state.xPos == 3 && state.yPos == 3;
        }

        private class State
        {
            public int xPos = 0;
            public int yPos = 0;

            public string path = "";
        }

        private static readonly MD5 md5 = System.Security.Cryptography.MD5.Create();

        private static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}