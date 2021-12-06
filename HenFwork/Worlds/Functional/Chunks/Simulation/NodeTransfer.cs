// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Worlds.Functional.Chunks;
using HenFwork.Worlds.Functional.Nodes;

namespace HenFwork.Worlds.Chunks.Simulation
{
    public partial class ChunksSimulationManager
    {
        private struct NodeTransfer
        {
            public Chunk Chunk;
            public Node Node;
            public TransferType Type;

            public NodeTransfer(Node node, TransferType type, Chunk chunk)
            {
                Chunk = chunk;
                Node = node;
                Type = type;
            }

            public void Perform()
            {
                if (Type == TransferType.From)
                    Chunk.RemoveNode(Node);
                else if (Type == TransferType.To)
                    Chunk.AddNode(Node);
            }

            public enum TransferType
            {
                To,
                From
            }
        }
    }
}