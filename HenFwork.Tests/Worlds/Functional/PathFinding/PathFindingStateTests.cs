// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Worlds.Functional.PathFinding;
using NUnit.Framework;

namespace HenFwork.Tests.Worlds.Functional.PathFinding
{
    public class PathfindingStateTests
    {
        [Test]
        public void NotFinishedTest()
        {
            Assert.IsTrue(PathfindingState.Successful.HasFlag(PathfindingState.Finished));
            Assert.IsTrue(PathfindingState.Failed.HasFlag(PathfindingState.Finished));
            Assert.IsFalse(PathfindingState.NotStarted.HasFlag(PathfindingState.Finished));
            Assert.IsFalse(PathfindingState.InProgress.HasFlag(PathfindingState.Finished));
        }
    }
}