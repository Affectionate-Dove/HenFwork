// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using System;

namespace HenFwork.Worlds
{
    /// <summary>
    ///     Declares that a field or property can
    ///     be saved and then restored.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class SaveableAttribute : Attribute
    {
    }
}