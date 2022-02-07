// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Testing;
using System;

[assembly: System.Reflection.Metadata.MetadataUpdateHandler(typeof(HotReloadManager))]

namespace HenFwork.Testing
{
    public static class HotReloadManager
    {
        public static event Action HotReloaded = delegate { };

        //private static void ClearCache(Type[]? types)
        //{
        //    Console.WriteLine("ClearCache");
        //}

        public static void UpdateApplication(Type[]? _)
        {
            Console.WriteLine("Hot reloaded");
            HotReloaded?.Invoke();
        }
    }
}