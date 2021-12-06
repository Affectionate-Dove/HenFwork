// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using System;
using System.Collections.Generic;

namespace HenFwork.Worlds.Functional.PathFinding
{
    /// <summary>
    ///     Finds a path to
    ///     the given <see cref="Destination"/> by visiting nodes
    ///     using a naive algorithm.
    /// </summary>
    public class PathfindingAgent
    {
        public event Action<PathfindingAgent> AgentCreated;

        public event Action<List<PathNode>> PathFound;

        public List<PathNode> VisitedNodes { get; }
        public PathNode CurrentNode { get; }
        public PathNode Destination { get; }

        public PathfindingAgent(PathNode startNode, PathNode destination)
        {
            VisitedNodes = new List<PathNode> { startNode };
            CurrentNode = startNode;
            Destination = destination;
        }

        private PathfindingAgent(List<PathNode> visitedNodes, PathNode currentNode, PathNode destination)
        {
            VisitedNodes = visitedNodes;
            CurrentNode = currentNode;
            Destination = destination;
        }

        /// <summary>
        ///     If <see cref="CurrentNode"/> is the <see cref="Destination"/>,
        ///     notifies that it's been found.
        ///     Otherwise, creates more agents and
        ///     places them in connected nodes that haven't been visited yet.
        ///     Raises the <see cref="AgentCreated"/> event on agent creation.
        /// </summary>
        public void Run()
        {
            if (CurrentNode == Destination)
                OnDestinationEncountered();
            else
                CreateMoreAgents();
        }

        private void CreateMoreAgents()
        {
            foreach (var connectedNode in CurrentNode.Connections)
                if (!VisitedNodes.Contains(connectedNode))
                    CreateAgent(connectedNode);
        }

        private void CreateAgent(PathNode connectedNode)
        {
            List<PathNode> visitedNodes = new(VisitedNodes.Count + 1);
            visitedNodes.AddRange(VisitedNodes);
            visitedNodes.Add(connectedNode);
            var newAgent = new PathfindingAgent(visitedNodes, connectedNode, Destination);
            SignalAgentCreation(newAgent);
        }

        private void OnDestinationEncountered() => PathFound(VisitedNodes);

        private void SignalAgentCreation(PathfindingAgent newAgent) => AgentCreated(newAgent);
    }
}