// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

namespace HenFwork.Worlds.Functional.PathFinding
{
    /// <summary>
    /// Represents a request for a path to be found
    /// between two points.
    /// </summary>
    public record PathRequest(PathNode Start, PathNode End);
}