using NUnit.Framework;
using Pathfinding.Models;

namespace Pathfinding.Logic.Tests
{
    [TestFixture]
    public class Tests
    {
        [TestCase(1, 1, 2, 2, 2, 1)]
        [TestCase(2, 2, 1, 1, 1, 2)]
        public void GetPath_ReturnsPathWithThreeNodes(int a, int b, int c, int d, int e, int f)
        {
            //Arrange
            var pathFinder = new Pathfinder(10, 10);
            var startNode = new PathNode()
            {
                X = a,
                Y = b,
            };
            var endNode = new PathNode()
            {
                X = c,
                Y = d,
            };
            var newNode = new PathNode()
            {
                X = e,
                Y = f,
            };
            //Act
            var result = pathFinder.GetPath(startNode, endNode);
            //Assert
            Assert.Contains(newNode, result);
        }

        [TestCase(0, 0, 0, 1, 1, 0)]
        [TestCase(9, 9, 9, 8, 8, 9)]
        [TestCase(0, 9, 1, 9, 0, 8)]
        [TestCase(9, 0, 9, 1, 8, 0)]
        public void GetAvailableNeighbours_OnEndOfPathFinder_ReturnsTwoNeigbours(int a, int b, int c, int d, int e, int f)
        {
            //Arrange
            var pathFinder = new Pathfinder(10, 10);
            var mainNode = new PathNode()
            {
                X = a,
                Y = b,
            };
            var neigbour1 = new PathNode()
            {
                X = c,
                Y = d,
            };
            var neigbour2 = new PathNode()
            {
                X = e,
                Y = f,
            };
            //Act
            var result = pathFinder.GetAvailableNeighbours(mainNode);
            //Assert
            Assert.Contains(neigbour1, result);
            Assert.Contains(neigbour2, result);
        }

        [TestCase(1, 1, 1, 1, 2, 2, 0, 2)]
        [TestCase(2, 2, 1, 1, 2, 2, 2, 0)]
        public void CalculateCosts_ChangesStartAndEndCostsOfNode(int a, int b, int c, int d, int e, int f, int costToStart, int costToEnd)
        {
            //Arrange
            var pathFinder = new Pathfinder(10, 10);
            var evaluatedNode = new PathNode()
            {
                X = a,
                Y = b,
            };
            var startNode = new PathNode()
            {
                X = c,
                Y = d,
            };
            var endNode = new PathNode()
            {
                X = e,
                Y = f,
            };
            //Act
            pathFinder.CalculateCosts(evaluatedNode, startNode, endNode);
            //Assert
            Assert.AreEqual(costToStart, evaluatedNode.CostToStartNode);
            Assert.AreEqual(costToEnd, evaluatedNode.CostToEndNode);
            Assert.AreEqual(costToStart + costToEnd, evaluatedNode.OverallCost);
        }
        
        [TestCase(1, 1, 2, 2, 2)]
        [TestCase(1, 1, 1, 2, 1)]
        [TestCase(2, 2, 1, 2, 1)]
        [TestCase(0, 0, 3, 3, 6)]
        public void CalculateFastestPathCost(int a, int b, int c, int d, int cost)
        {
            //Arrange
            var pathFinder = new Pathfinder(10, 10);
            var startNode = new PathNode()
            {
                X = a,
                Y = b,
            };
            var endNode = new PathNode()
            {
                X = c,
                Y = d,
            };
            //Act
            var result = pathFinder.CalculateFastestPathCost(startNode, endNode);
            //Assert
            Assert.AreEqual(cost, result);
        }
    }
}