// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using System.Collections.Generic;

namespace HenFwork.Worlds.Functional.PathFinding
{
    /// <summary>
    ///     Responsible for finding the shortest path
    ///     between two given points A and B.
    /// </summary>
    public class Pathfinder
    {
        private readonly List<PathfindingAgent> pathfindingAgents = new();
        private List<PathNode> result = new();
        public PathfindingState State { get; protected set; } = PathfindingState.NotStarted;

        /// <summary>
        ///     The found requested path.
        ///     Directly connected points that lead from
        ///     <see cref="PathRequest.Start"/> point to
        ///     <see cref="PathRequest.End"/> point
        ///     of the <see cref="Request"/> property.
        /// </summary>
        public IReadOnlyList<PathNode> Result => State == PathfindingState.Successful ? result : null;

        public PathRequest Request { get; }

        public Pathfinder(PathRequest request)
        {
            Request = request;
            State = PathfindingState.InProgress;
            var agent = new PathfindingAgent(Request.Start, Request.End);
            OnNewAgentCreated(agent);
        }

        /// <summary>
        ///     Performs pathfinding computations.
        /// </summary>
        /// <param name="limit">
        ///     The maximum amount of iterations to perform.
        ///     If 0 or less, there is no limit.
        /// </param>
        public void Update(int limit = 0)
        {
            for (var i = 0; limit <= 0 || i < limit; i++)
            {
                if (pathfindingAgents.Count == 0)
                    break;
                var agent = pathfindingAgents[0];
                agent.Run();
                pathfindingAgents.RemoveAt(0);
                if (State == PathfindingState.Successful)
                    break;
            }
            if (State != PathfindingState.Successful && pathfindingAgents.Count == 0)
                State = PathfindingState.Failed;
        }

        private void OnNewAgentCreated(PathfindingAgent newAgent)
        {
            newAgent.PathFound += OnPathFound;
            newAgent.AgentCreated += OnNewAgentCreated;
            pathfindingAgents.Add(newAgent);
        }

        private void OnPathFound(List<PathNode> path)
        {
            result = path;
            State = PathfindingState.Successful;
        }
    }
}