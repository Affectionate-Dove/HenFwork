// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using System;

namespace HenFwork.MapEditing.Saves.PropertySerializers
{
    public class BooleanSerializer : SaveableMemberSerializer
    {
        protected override object DeserializeInternal(string data) => data switch
        {
            "0" => false,
            "1" => true,
            _ => throw new ArgumentOutOfRangeException(nameof(data)),
        };

        protected override string SerializeInternal(object obj) => (bool)obj ? "1" : "0";
    }
}