// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Numerics;
using HenFwork.MapEditing.Saves.PropertySerializers;
using HenFwork.Worlds.Functional.Mediums;

namespace HenFwork.MapEditing.Saves
{
    public record MediumSave
    {
        private const char separator = '|';
        private static readonly Triangle3Serializer triangle3Serializer = new();

        public Triangle3 Triangle { get; }
        public MediumType Type { get; }

        public MediumSave(Triangle3 triangle, MediumType type)
        {
            Triangle = triangle;
            Type = type;
        }

        public MediumSave(string data)
        {
            var parts = data.Split(separator);
            Type = (MediumType)int.Parse(parts[0]);
            Triangle = (Triangle3)triangle3Serializer.Deserialize(parts[1]);
        }

        public string ToStringData()
        {
            var serializedTriangle = triangle3Serializer.Serialize(Triangle);
            return ((int)Type).ToString() + separator + serializedTriangle;
        }
    }
}