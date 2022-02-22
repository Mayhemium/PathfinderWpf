using System;
using System.Collections.Generic;
using System.Text;

namespace Pathfinding.Models
{
    public class PathNode : IEquatable<PathNode>
    {
        /// <summary>
        /// X coordinate of a node
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Y coordinate of a node
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// Previous node in a path to end node
        /// </summary>
        public PathNode? Parent { get; set; }
        /// <summary>
        /// Cost of moving from start node to this node
        /// </summary>
        public int CostToStartNode { get; set; }
        /// <summary>
        /// Cost of moving from this node to end node
        /// </summary>
        public int CostToEndNode { get; set; }
        /// <summary>
        /// Sum of cost to start node and cost to end node
        /// </summary>
        public int OverallCost {
            get
            {
                return CostToStartNode + CostToEndNode;
            }
        }
        /// <summary>
        /// Check if two nodes have the same coordinates
        /// </summary>
        /// <param name="other">Node to check with this</param>
        /// <returns></returns>
        public bool Equals(PathNode other)
        {
            return X == other.X && Y == other.Y;
        }
    }
}
