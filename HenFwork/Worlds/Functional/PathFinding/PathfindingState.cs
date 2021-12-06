// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using System;

namespace HenFwork.Worlds.Functional.PathFinding
{
    [Flags]
    public enum PathfindingState
    {
        NotStarted =
            0b0000,

        InProgress =
            0b0010,

        Failed =
            0b1001,

        Successful =
            0b0101,

        Finished = Failed & Successful
    }
}