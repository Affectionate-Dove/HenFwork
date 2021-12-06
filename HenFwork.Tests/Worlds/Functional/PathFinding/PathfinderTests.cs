// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Worlds.Functional.PathFinding;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace HenFwork.Tests.Worlds.Functional.PathFinding
{
    public class PathfinderTests
    {
        public static IEnumerable<PathFindingTestCase> EnumerateTestCases()
        {
            var pointA = new PathNode { Position = new Vector3(-1, 0, 0) };
            var pointB = new PathNode { Position = new Vector3(1, 0, 0) };
            var pointC = new PathNode { Position = new Vector3(-2, 0, 4) };
            pointA.ConnectSymmetrically(pointB);
            pointB.ConnectSymmetrically(pointC);

            yield return new PathFindingTestCase(new PathRequest(pointA, pointB), PathfindingState.Successful, 2);
            yield return new PathFindingTestCase(new PathRequest(pointA, pointC), PathfindingState.Successful, 7);

            yield return ImpossiblePathTestCase();
        }

        [TestCaseSource(nameof(EnumerateTestCases)), Timeout(1000)]
        public void PathFindTest(PathFindingTestCase testCase)
        {
            var pathFinder = new Pathfinder(testCase.PathRequest);
            while (!pathFinder.State.HasFlag(PathfindingState.Finished))
                try
                {
                    pathFinder.Update();
                }
                catch (NotImplementedException)
                {
                    Assert.Ignore("Not yet implemented");
                }

            Assert.AreEqual(testCase.ResultState, pathFinder.State);

            if (testCase.ResultState == PathfindingState.Successful)
                Assert.AreEqual(testCase.Distance, PathLength(pathFinder.Result));
        }

        private static PathFindingTestCase ImpossiblePathTestCase()
        {
            var a = new PathNode();
            var b = new PathNode();
            var c = new PathNode();

            a.ConnectSymmetrically(b);
            b.ConnectSymmetrically(c);
            b.DisconnectAsymmetrically(c);
            return new PathFindingTestCase(new PathRequest(a, c), PathfindingState.Failed, null);
        }

        private static float PathLength(IReadOnlyList<PathNode> pathPoints)
        {
            var length = 0f;
            for (var i = 0; i < pathPoints.Count - 1; i++)
                length += (pathPoints[i + 1].Position - pathPoints[i].Position).Length();
            return length;
        }

        public record PathFindingTestCase(PathRequest PathRequest, PathfindingState ResultState, float? Distance);
    }
}