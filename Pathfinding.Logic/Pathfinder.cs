using Pathfinding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinding.Logic
{
    /// <summary>
    /// To calculate path i've used A* pathfinding algorithm 
    /// https://www.youtube.com/watch?v=-L-WgKMFuhE&ab_channel=SebastianLague
    /// </summary>
    public class Pathfinder
    {
        /// <summary>
        /// List of nodes that are obstacles
        /// </summary>
        private bool[,] blockedNodes;

        public Pathfinder(int sizeX, int sizeY)
        {
            blockedNodes = new bool[sizeX, sizeY];
        }

        /// <summary>
        /// Get path in a list of nodes from startNode to endNode
        /// </summary>
        /// <param name="startNode">Node that starts path</param>
        /// <param name="endNode">Node that ends path</param>
        /// <returns>List of nodes between points</returns>
        /// <exception cref="Exception">Unable to calculate path (path blocked)</exception>
        public List<PathNode> GetPath(PathNode startNode, PathNode endNode)
        {
            var path = new List<PathNode>();
            var availableNodes = new List<PathNode>();
            var triedNodes = new List<PathNode>();

            startNode.Parent = null;
            availableNodes.Add(startNode);
            CalculateCosts(startNode, startNode, endNode);
            PathNode? currentNode;

            while (true)
            {
                if (availableNodes.Count == 0)
                    throw new Exception("Unable to create path");

                var minOverallCost = availableNodes.Min(node => node.OverallCost);
                var nodes = availableNodes.Where(node => node.OverallCost == minOverallCost);
                currentNode = nodes.OrderBy(node => node.CostToEndNode).First();
                var a = availableNodes.Remove(currentNode);
                triedNodes.Add(currentNode);

                if (currentNode.Equals(endNode))
                    break;

                var neighbours = GetAvailableNeighbours(currentNode);

                foreach (var neighbour in neighbours)
                {
                    if (triedNodes.Count(node => node.Equals(neighbour)) > 0)
                    {
                        if(currentNode.CostToStartNode + 1 < neighbour.CostToStartNode)
                        {
                            neighbour.Parent = currentNode;
                            neighbour.CostToStartNode = neighbour.CostToStartNode + 1;
                        }
                        continue;
                    }
                    var neighbourFromList = availableNodes.FirstOrDefault(node => node.Equals(neighbour));
                    if(neighbourFromList is not null)
                    {
                        if(currentNode.CostToStartNode + 1 < neighbourFromList.CostToStartNode)
                        {
                            neighbourFromList.Parent = currentNode;
                            neighbourFromList.CostToStartNode = currentNode.CostToStartNode + 1;
                        }
                    }
                    else
                    {
                        neighbour.CostToStartNode = currentNode.CostToStartNode + 1;
                        neighbour.CostToEndNode = CalculateFastestPathCost(neighbour, endNode);
                        neighbour.Parent = currentNode;
                        availableNodes.Add(neighbour);
                    }
                }
            }

            while(currentNode is not null)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            foreach(var node in path)
            {
                blockedNodes[node.X, node.Y] = true;
            }
            return path;
        }

        /// <summary>
        /// Get possible moves from provided node (no diagonals so paths wont be crossing)
        /// </summary>
        /// <param name="currentNode">Node for which neighbours will be evaluated</param>
        /// <returns>List of available neighbours(not blocked)</returns>
        public List<PathNode> GetAvailableNeighbours(PathNode currentNode)
        {
            var neighbours = new List<PathNode>();

            if (currentNode.X - 1 >= 0 && !blockedNodes[currentNode.X - 1, currentNode.Y])
            {
                neighbours.Add(new PathNode()
                {
                    X = currentNode.X - 1,
                    Y = currentNode.Y
                });
            }

            if (currentNode.X + 1 < blockedNodes.GetLength(0) && !blockedNodes[currentNode.X + 1, currentNode.Y])
            {
                neighbours.Add(new PathNode()
                {
                    X = currentNode.X + 1,
                    Y = currentNode.Y
                });
            }

            if (currentNode.Y - 1 >= 0 && !blockedNodes[currentNode.X, currentNode.Y - 1])
            {
                neighbours.Add(new PathNode()
                {
                    X = currentNode.X,
                    Y = currentNode.Y - 1
                });
            }

            if (currentNode.Y + 1 < blockedNodes.GetLength(1) && !blockedNodes[currentNode.X, currentNode.Y + 1])
            {
                neighbours.Add(new PathNode()
                {
                    X = currentNode.X,
                    Y = currentNode.Y + 1
                });
            }

            return neighbours;
        }

        /// <summary>
        /// Calculates cost from evaluated node to end and start nodes (saves changes to evaluated node)
        /// </summary>
        /// <param name="evaluatedNode">Node for which costs will be calculated</param>
        /// <param name="startNode">Start node of evaluatedNode</param>
        /// <param name="endNode">End node of evaluatedNode</param>
        public void CalculateCosts(PathNode evaluatedNode, PathNode startNode, PathNode endNode)
        {
            evaluatedNode.CostToStartNode = CalculateFastestPathCost(evaluatedNode, startNode);   
            evaluatedNode.CostToEndNode = CalculateFastestPathCost(evaluatedNode, endNode);
        }

        /// <summary>
        /// Calculate fastest path between two nodes
        /// </summary>
        /// <param name="startNode">First node</param>
        /// <param name="endNode">Second node</param>
        /// <returns></returns>
        public int CalculateFastestPathCost(PathNode startNode, PathNode endNode)
        {
            PathNode stepNode = new PathNode();
            stepNode.X = startNode.X;
            stepNode.Y = startNode.Y;

            var cost = 0;
            while(stepNode.X != endNode.X || stepNode.Y != endNode.Y)
            {
                if (Math.Abs(stepNode.X - endNode.X) > Math.Abs(stepNode.Y - endNode.Y))
                {
                    if (stepNode.X > endNode.X)
                    {
                        stepNode.X--;
                        cost++;
                        continue;
                    }
                    else if (stepNode.X < endNode.X)
                    {
                        stepNode.X++;
                        cost++;
                        continue;
                    }
                }
                else
                {
                    if (stepNode.Y > endNode.Y)
                    {
                        stepNode.Y--;
                        cost++;
                        continue;
                    }
                    else if (stepNode.Y < endNode.Y)
                    {
                        stepNode.Y++;
                        cost++;
                        continue;
                    }
                }
            }

            return cost;
        }
    }
}
