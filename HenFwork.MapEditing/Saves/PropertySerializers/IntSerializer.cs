// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

namespace HenFwork.MapEditing.Saves.PropertySerializers
{
    public class IntSerializer : SaveableMemberSerializer
    {
        protected override object DeserializeInternal(string data) => int.Parse(data);

        protected override string SerializeInternal(object obj) => obj.ToString()!;
    }
}