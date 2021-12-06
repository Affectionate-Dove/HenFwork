// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Worlds.Functional.PathFinding;
using NUnit.Framework;
using System.Linq;

namespace HenFwork.Tests.Worlds.Functional.PathFinding
{
    public class PathfindingManagerTests
    {
        [Test]
        public void Test()
        {
            var nodeA = new PathNode();
            var nodeB = new PathNode();
            var nodeC = new PathNode();
            var nodeD = new PathNode();
            nodeA.ConnectAsymmetrically(nodeB);
            nodeB.ConnectAsymmetrically(nodeC);
            nodeC.ConnectAsymmetrically(nodeD);
            // A -> B -> C -> D

            var pathfinder1 = new Pathfinder(new PathRequest(nodeA, nodeB));
            var pathfinder2 = new Pathfinder(new PathRequest(nodeA, nodeC));
            var pathfinder3 = new Pathfinder(new PathRequest(nodeA, nodeD));
            var pathfinder4 = new Pathfinder(new PathRequest(nodeC, nodeA));

            var pathfindingManager = new PathfindingManager();
            pathfindingManager.Pathfinders.AddRange(new[] { pathfinder1, pathfinder2, pathfinder3, pathfinder4 });

            for (var i = 0; i < 10; i++)
            {
                pathfindingManager.Update();
                if (pathfindingManager.Pathfinders.Count >= 4)
                    Assert.Contains(pathfinder1, pathfindingManager.Pathfinders);
                if (pathfindingManager.Pathfinders.Count >= 3)
                    Assert.Contains(pathfinder2, pathfindingManager.Pathfinders);
                if (pathfindingManager.Pathfinders.Count >= 2)
                    Assert.Contains(pathfinder3, pathfindingManager.Pathfinders);
            }
            Assert.Zero(pathfindingManager.Pathfinders.Count);
            Assert.IsTrue(new[] { pathfinder1, pathfinder2, pathfinder3 }.All(pf => pf.State == PathfindingState.Successful));
            Assert.IsTrue(pathfinder4.State == PathfindingState.Failed);
        }
    }
}