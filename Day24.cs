namespace AdventOfCode
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class Day24
    {
        private static ISet<node> AllNodes = new HashSet<node>();

        public static int SolveProblem1()
        {
            Console.WriteLine("Start: " + DateTime.Now);
            // var instructions = TestProblemInput1.SplitToLines().ToList();
            var instructions = ProblemInput.SplitToLines().ToList();
            var width = instructions[0].Length;
            var height = instructions.Count;

            var maze = instructions.Select(i => i.ToArray()).ToArray();

            for(var x = 0; x < width; x++)
            {
                for(var y=0; y<height; y++)
                {
                    char currentSpace = maze[y][x];
                    if (currentSpace == '#')
                        continue;

                    var newNode = new node { X = x, Y = y};
                    if (currentSpace == '0')
                    {
                        newNode.IsStartSquare = true;
                        newNode.IsTargetSquare = true;
                    }
                    else if (currentSpace != '.')
                    {
                        newNode.IsTargetSquare = true;
                    }

                    var neighbours = AllNodes.Where(n =>
                        (n.X == newNode.X-1 && n.Y == newNode.Y) ||
                        (n.X == newNode.X+1 && n.Y == newNode.Y) ||
                        (n.X == newNode.X && n.Y == newNode.Y-1) ||
                        (n.X == newNode.X && n.Y == newNode.Y+1));

                    foreach (var neighbour in neighbours)
                    {
                        newNode.neighbours.Add(neighbour);
                        neighbour.neighbours.Add(newNode);
                    }

                    AllNodes.Add(newNode);
                }
            }

            Console.WriteLine("built nodes");

            var targets = AllNodes.Where(n => n.IsTargetSquare).ToList();

            var distances = targets.Select(t => findTargetDistances(t, targets.Where(t2=>t2 != t).ToList())).ToList();
            var distancesDictionary = distances.ToDictionary(l => l[0].target1, l => l);
            Console.WriteLine("found distances");

            var shortestTotal = findShortestTotal(distancesDictionary, targets.Where(t => !t.IsStartSquare).ToList(), AllNodes.Single(n =>n.IsStartSquare));

            Console.WriteLine("End: " + DateTime.Now);
            return shortestTotal;
        }

        public static int SolveProblem2()
        {
            Console.WriteLine("Start: " + DateTime.Now);
            // var instructions = TestProblemInput1.SplitToLines().ToList();
            var instructions = ProblemInput.SplitToLines().ToList();
            var width = instructions[0].Length;
            var height = instructions.Count;

            var maze = instructions.Select(i => i.ToArray()).ToArray();

            for(var x = 0; x < width; x++)
            {
                for(var y=0; y<height; y++)
                {
                    char currentSpace = maze[y][x];
                    if (currentSpace == '#')
                        continue;

                    var newNode = new node { X = x, Y = y};
                    if (currentSpace == '0')
                    {
                        newNode.IsStartSquare = true;
                        newNode.IsTargetSquare = true;
                    }
                    else if (currentSpace != '.')
                    {
                        newNode.IsTargetSquare = true;
                    }

                    var neighbours = AllNodes.Where(n =>
                        (n.X == newNode.X-1 && n.Y == newNode.Y) ||
                        (n.X == newNode.X+1 && n.Y == newNode.Y) ||
                        (n.X == newNode.X && n.Y == newNode.Y-1) ||
                        (n.X == newNode.X && n.Y == newNode.Y+1));

                    foreach (var neighbour in neighbours)
                    {
                        newNode.neighbours.Add(neighbour);
                        neighbour.neighbours.Add(newNode);
                    }

                    AllNodes.Add(newNode);
                }
            }

            Console.WriteLine("built nodes");

            var targets = AllNodes.Where(n => n.IsTargetSquare).ToList();

            var distances = targets.Select(t => findTargetDistances(t, targets.Where(t2=>t2 != t).ToList())).ToList();
            var distancesDictionary = distances.ToDictionary(l => l[0].target1, l => l);
            Console.WriteLine("found distances");

            var shortestTotal = findShortestTotal2(distancesDictionary, targets.Where(t => !t.IsStartSquare).ToList(), AllNodes.Single(n =>n.IsStartSquare));

            Console.WriteLine("End: " + DateTime.Now);
            return shortestTotal;
        }

        private static int findShortestTotal2(Dictionary<node, IList<TargetDistance>> distancesDictionary, List<node> targets, node start)
        {
            return findShortestTotal2(distancesDictionary, targets, start, 0, int.MaxValue, start);
        }

        private static int findShortestTotal2(
            Dictionary<node, IList<TargetDistance>> distancesDictionary,
            List<node> remainingTargets,
            node currentNode,
            int currentDistance,
            int shortestDistance,
            node endNode)
        {
            if (currentDistance >= shortestDistance)
                return shortestDistance;

            var distances = distancesDictionary[currentNode];

            if (remainingTargets.Count == 0)
                return currentDistance + distances.Single(d => d.target2 == endNode).distance;

            foreach(var target in remainingTargets)
            {
                var candidateDistance = findShortestTotal2(
                    distancesDictionary,
                    remainingTargets.Where(t => t != target).ToList(),
                    target,
                    currentDistance + distances.Single(d => d.target2 == target).distance,
                    shortestDistance,
                    endNode);

                shortestDistance = Math.Min(candidateDistance, shortestDistance);
            }

            return shortestDistance;
        }

        private static int findShortestTotal(Dictionary<node, IList<TargetDistance>> distancesDictionary, List<node> targets, node start)
        {
            return findShortestTotal(distancesDictionary, targets, start, 0, int.MaxValue);
        }

        private static int findShortestTotal(
            Dictionary<node, IList<TargetDistance>> distancesDictionary,
            List<node> remainingTargets,
            node currentNode,
            int currentDistance,
            int shortestDistance)
        {
            if (currentDistance >= shortestDistance)
                return shortestDistance;

            if (remainingTargets.Count == 0)
                return currentDistance;

            var distances = distancesDictionary[currentNode];

            foreach(var target in remainingTargets)
            {
                var candidateDistance = findShortestTotal(
                    distancesDictionary,
                    remainingTargets.Where(t => t != target).ToList(),
                    target,
                    currentDistance + distances.Single(d => d.target2 == target).distance,
                    shortestDistance);

                shortestDistance = Math.Min(candidateDistance, shortestDistance);
            }

            return shortestDistance;
        }

        private static IList<TargetDistance> findTargetDistances(node currentNode, IList<node> targets)
        {
            var targetDistances = new List<TargetDistance>();

            return targets.Select(t => new TargetDistance { target1 = currentNode, target2 = t, distance = findShortestDistance(currentNode, t)}).ToList();
        }

        private static int findShortestDistance(node start, node end)
        {
            Console.WriteLine(string.Format("finding shortest distance between {0},{1} and {2},{3}", start.X, start.Y, end.X, end.Y));
            return findShortestDistance(start, end, 1, int.MaxValue, new Dictionary<node, int>() { { start, 0}});
        }

        private static int findShortestDistance(node start, node end, int currentDistance, int shortestDistance, IDictionary<node, int> visitedNodes)
        {
            if (currentDistance >= shortestDistance)
                return shortestDistance;

            foreach(var neighbour in start.neighbours)
            {
                if (visitedNodes.ContainsKey(neighbour))
                {
                    if (visitedNodes[neighbour] <= currentDistance)
                    {
                        // We've been here before, and faster
                        continue;
                    }
                    else
                    {
                        visitedNodes[neighbour] = currentDistance;
                    }
                }
                else
                {
                    visitedNodes[neighbour] = currentDistance;
                }
                if (neighbour == end)
                {
                    return currentDistance;
                }
                else
                {
                    shortestDistance = Math.Min(shortestDistance, findShortestDistance(neighbour, end, currentDistance+1, shortestDistance, visitedNodes));
                }
            }

            return shortestDistance;
        }

        class TargetDistance
        {
            public node target1;
            public node target2;
            public int distance;
        }

        class node
        {
            public int X;
            public int Y;

            public List<node> neighbours = new List<node>();

            public bool IsTargetSquare;
            public bool IsStartSquare;
        }

        private static readonly string TestProblemInput1 = @"###########
#0.1.....2#
#.#######.#
#4.......3#
###########";
        private static readonly string TestProblemInput2 = @"qq";
        private static readonly string ProblemInput = @"#########################################################################################################################################################################################
#...#...............#.#...#.#.......#.....#.....#.....#...............#.....#.#.....#.....#.......#.........#.#.....#.....#.#.#...#...........#.................#.#...#.....#.....#...#.#
#.###.#.###.###.###.#.###.#.###.#####.###.#.#.#.#.#.#.#.#.###.#.#####.#.#.#.#.#.#.###.###.#.#.#.#.###.###.#.#.#.#.###.###.#.#.#.#.#####.#####.#.#.###########.###.#.#.#.###.#.###.#.#.#.#
#.#...#.....#.....#.#.#...#.#.....#.#.#...#.....#.#.#.....#.............#...#.....#...#.#.........#.#.....#.......#...#.#.#.#.#.........#.....#.#.#...#...#...#...#...#.....#.....#...#.#
#.#.###.#.#.#.#.###.#.#.###.#.#####.#.#.#.#.#####.#.#.#.###.#.#.#.#.###.#.#.###.#.#.#.#.###.###.#.#.###.#.#.#.###.###.#.#.#.#.###########.#.###.#.###.#.#.#.#.#.#.#.#.###.#.#.#.###.#.#.#
#...........#.#.#...#.#.....#.....#...........#...........#.....#.............#.......#.#...............#.....#.......#.....#...............#1....#.......#.#.....#.......#...#.....#...#
#.###.#.#.#.#.#.#.#.#########.#.#.#.#.#.###.#.#.#####.#.#.#.###.#.###.#########.#######.#.#.#.#.#############.#####.#.###.#.#.#.#.#####.#.#######.#####.#.#.#.#.#.#####.#.#.#.#.###.#.###
#...#.....#.....#.#.#.#.................#.#.#...#.......#...#...#.#...........#.#...#.......#.#.#...#...#.......#...#.#.#.#.#.....#.....#.....#.......#...#...#...#.......#...#.....#...#
#.#.#####.#.#####.#.#.#.#.###.###.#.#.###.#.#.#.#.#######.#.#.#.#.###.#.###.#####.#.#.#.#.#.#.#.#.#.#.#.#.#.#.#.#######.#.#####.#.#.#.#######.#.#.#.#.#.#######.#.###.#.#.#.###.###.#.#.#
#...#.....#.......#.#...#.#...#...........#...#...#.#.....#...#.......#.......#.#.#.#...#.#.....#.#...#...#.....#.........#...#.........#.#.....#.....#...#.#.#...#...#.....#.#3#.#.....#
#.#.###.#.#.###.#######.#.#.#.#.###.###.#####.#.#.#.#.#.#.#.#.#.#####.#.###.###.###.#.#.#.#.#.#.#.###.#.#######.###.#.###.#.#.#.###.#.#.#.#.#.#.#.#.###.###.#.#.###.#.#.###.#.#.#.###.#.#
#.......#.#...#.#.#.....#...#...#0..#.#.....#.#.....#.#.#...#...#.....#.....#.#.....#...#.#...#.#...#.#.#...#...#...#...#.#.#.............#.#...#.......#...#...#...#.#.#.....#...#.#...#
#####.#####.#.###.#.#.#.#.#.#.#.###.#.#.#.#.#.#.###.#.###.#.#.#.#.#.#.###.###.###.#.#####.#####.#.#.#.#.#.#.#.#.#.#.#.###.#.###.###########.#.#.#########.#.#.#.#.###.#.###.#.###.#.#.#.#
#...#...#.......#.#.#...#.......#...#...#.#...#.#...#.....#.....#.....#.#.#.........#.#.......#...#.#...#.#.........#...#.........#...#...#.#.....#.....#...#.#.......#.#.....#.........#
#.#.###.#.#####.###.###.#.#.###.###.#.###.#.#.#.#.#.#.#####.#.###.#.#.#.###.###.#####.#.#.#.#.#.###.###.#.#########.#.###.#.#.#.#.#.#.#.#.#.#####.###.#.#.#.#.#.#.#.#.#####.#.###.#.###.#
#.....#.#.#...#.#...#.#.....#.#...........#.....#...#.#...#.#.....#.#.#.....#.......#...#.#.#.......#.......#.........#.........#.#.#.#...#...#.#...#...#.......#.....#.....#.......#...#
###.#.#.#####.#.#.#.#.#.#.#.#.#.#.#.#.#.#######.###.#.#####.#####.###.#.#.#.###.#.#.###.#.#.#.#.#####.#.###.#.#.#.#########.#.###.#.#.#.#.#####.###.#.#.#.###.#####.#.#.#.#.#.#.#.#.#.###
#.#...#.#.......#.#.#...#.......#.#.....#.#.........#...#.....#.................#.........#...#...#.#...#.....#...#...#.#...#...#...#.....#...#.......#.......#.....#...#...#...#...#.#.#
#.#.#####.#.#####.###.#.###.#.###.#.#.#.#.#######.#.###.###.#.#.#########.#.###.#########.###.###.#.###.#.#####.#.###.#.#.#.#.#.#.#.#.###.#.#.#.#########.#####.#.#.###.#.#.#.#.#####.#.#
#.............#2#.#.....#.#.#...#...#.#...........#.........#...#.#.......#.#.........#.#.#.....#.#.......#...#.....#...#.#.#.......#...#.#.......#.......#...#.........#...#...#.....#.#
#.#.###.#.#.#.#.#.#.#.###.#.###.#.#####.###.###.#.#.#.#.#.#.#####.#.#.#.#.###.#.#####.#.#.#.#.#####.#.#####.###.###.#.#.#.#.#.#.#.#.#.#.#.#.###.#.###.#####.#######.#.#.#####.###.###.#.#
#...#...#...#...#.#.#.#...#.#.........#.#.#...............#.#...#...#.....#...#.#...#...#...........#.#.....#.....#...#.#.#...#...#.#.....#.....#...........#...#.....#.....#.#6#.....#.#
#####.#.#####.###.#.#.#.#.#.###.###.#.###.#.#########.#.###.#.#.#.###.###.#####.#####.###.###.#.#.#.###.#.#.#.#.###.#.###.#.#.#.#.###.#.#.#.###.#.#.#.#.#.###.#.#####.#.###.#.#.#.###.#.#
#...#...#.....#.....#...#...#...#.....#.....................#...#.....#.........#.....#.#.#...#.#.........#.....................#...#...#.....#...#...#.....#...#...#.....#.#.......#...#
#.###.#.#.#.#.###.#.###.#.#####.#.###.#.#####.#.#######.#.#.#.###.#.#.#.#.#.#.#.#.#.#.#.#.#.#.###.#####.###.#####.#######.###.#.#.#######.###.#.###.###.#.#.###.#.#.#.#####.#.#.#######.#
#.#...#.........#...#.....#.......#.....#.#.....#.#...#.#...#.#...#.........#.....#.......#.......#.......#.......#...#...#.......#.....#.#...........#.........#.#.#...#.#.#.....#...#.#
#.#.#.#.#.#.#.#.#.#.#########.#####.#.###.#.#####.#.###.#.#.#.#.#########.#.#.#.#.###.###.#####.###.###.#.#.#.#####.#.#.#########.###.#.###.###.###.#.#######.#.###.###.#.###.###.#.###.#
#.....#.....#...#.#.........#...#.....#...#.............#.....#.#.........#...#.......#.................#...#.........#...#...........#.........#.....#.......#.....#.....#.#.#.#...#.#.#
#.###.#.###.#.#.#.#.#.#.#.###.###.#.#######.#.###.#####.#.###.#.#.#.#.#.#.#.#.#######.###.###.#######.#.###.###.#####.#.#.#.#.#.#.#.###.#.###.###.###.#.###.#.###.###.#####.#.#.###.#.###
#.....#...#.#.....#.#...#.....#.....#.#...#...#...........#.#.#...........#.#.#.....#.....#...#...#...#...#.....#.......#.......#.#.........#.....#.....#.#...#.......#.........#.......#
#.#.#.###.#.#.#.###.###.#.###.#####.#.#.#.#.#.#######.###.#.#.#.#####.###.#.#.###.###.#.#.#.#.#.###.#####.###.###.#######.###.#.#.#.#.#.#.#####.#########.#.#.#.#.#.###.#######.###.#.#.#
#.......#.#.#.....#.....#.#...#.....#...#.#.....#...#.#...#.....#.......#...#.#.#...#.....#.......#.......#...#.#.......#...#...#.#.#...#.......#.....#.#...#.#.#...#...#.......#.......#
#.###.###.#.#.###.#.#.#.#.#.#.#.#.#.###.###.#.#.###.#.###.###.#.#.#.#.#.#####.#.#.#.#.#.#########.#.###.#.#.#.#.#.#.#.#.#.#####.###.###.#.#.#.#.#.#.#.#.#.#.###.#.###.#.#####.#######.#.#
#.....#.#5......#.......#...#.#.#...#.#.#.#.....#.....#.#.....#.......#.#...#...#.....#.....#...........#...#.......#.#...#.........#.........#.........#...........#.#.#.....#7....#.#.#
#.#.#.#.###.#####.#####.#.###.#.#.#.#.###.#.#.#.#####.#.#.#.#.#.###.#.###.###.###.#.#.###.#.#.#.#.#######.#.#.#####.#.#.###.#.#####.#.#.#.#.#.#.#.#####.#.#.#.#.#####.#.#.#.#.#####.#.#.#
#...........#.#...#.........#...#.#.#.#.......#.#.....#...#...#.#.....#...#...#.......#.................................#...#...#.#.....#.....#...#...#.#...#...#.#.......#.........#.#.#
###.###.#.###.#.###.#.#.#.#.#.#.#.#.#.#.###.#.#.#.#.#.#.###.###.###.###.#.###.#.###.#####.#.#.###.###.#.#####.#.#.#.#####.#.###.#.#.#.#.###.#.###.###.#.#######.#.#.#.#.###.###.#.#.#.#.#
#.......#.#.........#...#.#.#.................#.......#...#.....#...............#...#.#...#...#.......#.#.#.......#.......#...............#4#.......#.....#...#...#.#...#.......#.....#.#
#########################################################################################################################################################################################";
    }
}