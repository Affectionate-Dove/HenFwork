// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Worlds.Chunks.Simulation;
using HenFwork.Worlds.Functional.Chunks;
using HenFwork.Worlds.Functional.Chunks.Simulation;
using HenFwork.Worlds.Functional.Nodes;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace HenFwork.Tests.Worlds.Functional.Chunks.Simulation
{
    public class ChunksSimulationManagerTests
    {
        [Test]
        public void AllChunksStrategyTest()
        {
            var testSuite = new TestSuite();
            testSuite.SimulationManager.Strategy = new ChunksSimulationStrategy(ChunksSimulationStrategyType.All, null);
            var synchronizedTime = 1;
            testSuite.SimulationManager.Simulate(synchronizedTime);
            Assert.AreEqual(synchronizedTime, testSuite.SimulationManager.SynchronizedTime);
            foreach (var node in testSuite.Nodes.SelectMany(row => row))
                Assert.AreEqual(synchronizedTime, node.SynchronizedTime, node.Position.ToString());
        }

        [Test]
        public void SquareStrategyTest()
        {
            var testSuite = new TestSuite();
            testSuite.SimulationManager.Strategy = new ChunksSimulationStrategy(ChunksSimulationStrategyType.Square, new HashSet<ChunksSimulationRingConfiguration>
            {
                new ChunksSimulationRingConfiguration(1, 0)
            });

            var synchronizedTime = 1;
            testSuite.SimulationManager.Simulate(synchronizedTime, new Vector2(1));
            Assert.AreEqual(synchronizedTime, testSuite.SimulationManager.SynchronizedTime);

            foreach (var node in testSuite.Nodes.SelectMany(node => node))
                if (node.Position.X is <= 2 && node.Position.Z is <= 2)
                    Assert.AreEqual(synchronizedTime, node.SynchronizedTime, node.Position.ToString());
                else
                    Assert.AreEqual(0, node.SynchronizedTime, node.Position.ToString());
        }

        [Test]
        public void CircleStrategyTest()
        {
            var testSuite = new TestSuite();
            testSuite.SimulationManager.Strategy = new ChunksSimulationStrategy(ChunksSimulationStrategyType.Circle, new HashSet<ChunksSimulationRingConfiguration>
            {
                new ChunksSimulationRingConfiguration(1f, 0)
            });

            var synchronizedTime = 1;
            testSuite.SimulationManager.Simulate(synchronizedTime, new Vector2(1.5f));
            Assert.AreEqual(synchronizedTime, testSuite.SimulationManager.SynchronizedTime);

            var expectedSimulatedNodes = new List<TestNode>
            {
                testSuite.Nodes[0][1],
                testSuite.Nodes[1][0],
                testSuite.Nodes[1][1],
                testSuite.Nodes[1][2],
                testSuite.Nodes[2][1],
            };
            foreach (var node in testSuite.Nodes.SelectMany(node => node))
                if (expectedSimulatedNodes.Contains(node))
                    Assert.AreEqual(synchronizedTime, node.SynchronizedTime, node.Position.ToString());
                else
                    Assert.AreEqual(0, node.SynchronizedTime, node.Position.ToString());
        }

        [Test]
        public void SimulationFrequencyTest()
        {
            var testSuite = new TestSuite();
            testSuite.SimulationManager.Strategy = new ChunksSimulationStrategy(ChunksSimulationStrategyType.Square, new HashSet<ChunksSimulationRingConfiguration>
            {
                new ChunksSimulationRingConfiguration(1f, 1),
                new ChunksSimulationRingConfiguration(2f, 2),
                new ChunksSimulationRingConfiguration(3f, 3),
            });

            var synchronizedTime = 0d;
            for (var i = 1; i <= 30; i++)
            {
                synchronizedTime = i / 10d;
                testSuite.SimulationManager.Simulate(synchronizedTime, new Vector2(0));
            }

            Assert.AreEqual(synchronizedTime, testSuite.SimulationManager.SynchronizedTime);

            foreach (var node in testSuite.Nodes.SelectMany(node => node))
            {
                Assert.AreEqual(synchronizedTime, node.SynchronizedTime, node.Position.ToString());

                if (node.Position.X <= 1 && node.Position.Z <= 1)
                    Assert.AreEqual(3, node.TimesSimulated, node.Position.ToString());
                else if (node.Position.X <= 2 && node.Position.Z <= 2)
                    Assert.AreEqual(2, node.TimesSimulated, node.Position.ToString());
                else
                    Assert.AreEqual(1, node.TimesSimulated, node.Position.ToString());
            }
        }

        private class TestNode : Node
        {
            public int TimesSimulated { get; private set; }

            protected override void Simulation(double duration)
            {
                base.Simulation(duration);
                TimesSimulated++;
            }
        }

        private record TestSuite
        {
            public TestNode[][] Nodes;

            public TestSuite()
            {
                ChunksManager = new ChunksManager(new Vector2(4), 1);
                SimulationManager = new ChunksSimulationManager(ChunksManager);
                Nodes = new TestNode[4][];
                for (var y = 0; y <= 3; y++)
                {
                    Nodes[y] = new TestNode[4];
                    for (var x = 0; x <= 3; x++)
                        ChunksManager.AddNode(Nodes[y][x] = new TestNode { Position = new Vector3(x, 0, y) });
                }
            }

            public ChunksManager ChunksManager { get; }
            public ChunksSimulationManager SimulationManager { get; }
        }
    }
}