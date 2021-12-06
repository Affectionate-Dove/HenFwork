// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenBstractions.Numerics;
using HenFwork.Graphics3d.Shapes;
using HenFwork.Worlds.Functional;
using HenFwork.Worlds.Functional.Chunks;
using HenFwork.Worlds.Functional.Mediums;
using HenFwork.Worlds.Functional.Nodes;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace HenFwork.Graphics3d
{
    /// <summary>
    ///     Manages a <see cref="Graphics3d.Scene"/> that
    ///     represents a <see cref="World"/>.
    /// </summary>
    /// <remarks>
    ///     For any <see cref="Node"/>s or <see cref="Medium"/>s roughly within the <see cref="ViewDistance"/>
    ///     from <see cref="ViewPoint"/>, a <see cref="Spatial"/> is generated
    ///     using <see cref="NodeSpatialCreator"/> or <see cref="MediumSpatialCreator"/>
    ///     respectively, and on each <see cref="Update"/> call,
    ///     a function returned by <see cref="NodeHandlerCreator"/> is called for
    ///     the <see cref="Node"/> and the <see cref="Spatial"/>.
    /// </remarks>
    public class WorldSceneManager
    {
        private readonly Dictionary<Node, Action<Node, Spatial>> nodeHandlers = new();
        private readonly Dictionary<Node, Spatial> nodeSpatials = new();
        private readonly Dictionary<Medium, Spatial> mediumSpatials = new();
        private float viewDistance;

        // TODO: implement an LOD system
        // TODO: different distances for loading and unloading
        /// <summary>
        ///     The distance from <see cref="ViewPoint"/>
        ///     inside of which <see cref="Node"/>s
        ///     are considered for visual representation.
        /// </summary>
        public float ViewDistance
        {
            get => viewDistance;
            set
            {
                if (value is < 0)
                    throw new ArgumentOutOfRangeException(nameof(ViewDistance), "Cannot be less than 0");

                viewDistance = value;
            }
        }

        public Vector2 ViewPoint { get; set; }

        public Scene Scene { get; set; }
        public NodeWorld World { get; }

        public ChunksAreaObserver Observer { get; }

        public Func<Node, Spatial> NodeSpatialCreator { get; set; } = DefaultNodeSpatialCreator;
        public Func<Node, Spatial, Action<Node, Spatial>> NodeHandlerCreator { get; set; } = DefaultNodeHandlerCreator;
        public Func<Medium, Spatial> MediumSpatialCreator { get; set; } = DefaultMediumSpatialCreator;

        public WorldSceneManager(NodeWorld world)
        {
            World = world;
            Scene = new Scene();

            Observer = new ChunksAreaObserver(world.ChunksManager);
            Observer.NodeEnteredChunksArea += OnNodeEntrance;
            Observer.NodeLeftChunksArea += OnNodeExit;
            Observer.MediumEnteredChunksArea += OnMediumEntrance;
            Observer.MediumLeftChunksArea += OnMediumExit;
        }

        public static Action<Node, Spatial> DefaultNodeHandlerCreator(Node node, Spatial spatial) => DefaultNodeHandler;

        public static void DefaultNodeHandler(Node node, Spatial spatial) => spatial.Position = node.Position;

        public static Spatial DefaultNodeSpatialCreator(Node node) => new BoxSpatial
        {
            Box = Box.FromPositionAndSize(Vector3.Zero, Vector3.One, new(0.5f)),
            Color = ColorInfo.RED,
            Position = node.Position
        };

        public static Spatial DefaultMediumSpatialCreator(Medium medium) => new TriangleSpatial
        {
            Triangle = medium.Triangle,
            Color = new(medium.Color.r, medium.Color.g, medium.Color.b, 30),
            WireColor = medium.Color
        };

        public void Update()
        {
            Observer.ObservedArea = RectangleF.FromPositionAndSize(ViewPoint, new(ViewDistance * 2), new(0.5f), CoordinateSystem2d.YUp);
            foreach (var (node, nodeHandler) in nodeHandlers)
                nodeHandler(node, nodeSpatials[node]);
        }

        private void OnNodeEntrance(Node node)
        {
            var spatial = NodeSpatialCreator(node);
            nodeSpatials.Add(node, spatial);
            Scene.Spatials.Add(spatial);
            var nodeHandler = NodeHandlerCreator(node, spatial);
            nodeHandlers.Add(node, nodeHandler);
        }

        private void OnNodeExit(Node node)
        {
            Scene.Spatials.Remove(nodeSpatials[node]);
            nodeSpatials.Remove(node);
            nodeHandlers.Remove(node);
        }

        private void OnMediumEntrance(Medium medium)
        {
            var spatial = MediumSpatialCreator(medium);
            mediumSpatials.Add(medium, spatial);
            Scene.Spatials.Add(spatial);
        }

        private void OnMediumExit(Medium medium)
        {
            Scene.Spatials.Remove(mediumSpatials[medium]);
            mediumSpatials.Remove(medium);
        }
    }
}