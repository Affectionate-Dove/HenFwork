﻿// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.IO.Stores;

namespace HenFwork.Tests.IO.Stores
{
    public class TestStore : ResourceStore<string>
    {
        protected override string GetInternal(string assetName) => assetName;

        protected override void LoadInternal(string assetName)
        {
        }

        protected override void UnloadInternal(string assetName)
        {
        }
    }
}