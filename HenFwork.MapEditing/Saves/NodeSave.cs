// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Worlds.Functional.Nodes;
using System.Collections.Generic;
using System.Linq;

namespace HenFwork.MapEditing.Saves
{
    /// <summary>
    ///     A state of a <see cref="Node"/>.
    /// </summary>
    public record NodeSave
    {
        public string AssemblyName { get; }
        public string TypeName { get; }
        public string FullTypeName { get; }

        /// <summary>
        ///     Names of <see cref="Node"/>'s members and respective values.
        /// </summary>
        public Dictionary<string, string> MembersValues { get; }

        public NodeSave(string assemblyName, string typeName, IEnumerable<KeyValuePair<string, string>> membersValues)
        {
            AssemblyName = assemblyName;
            TypeName = typeName;
            FullTypeName = $"{AssemblyName}.{TypeName}";
            MembersValues = new(membersValues);
        }

        public NodeSave(string data)
        {
            var splitData = data.Split('\t');

            var assemblyAndTypeNames = splitData[0].Split('|');
            AssemblyName = assemblyAndTypeNames[0];
            TypeName = assemblyAndTypeNames[1];
            FullTypeName = $"{AssemblyName}.{TypeName}";

            MembersValues = splitData[1..].Select(StringAsKeyValue).ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        /// <summary>
        ///     Serializes this <see cref="NodeSave"/> to a text string.
        /// </summary>
        public string ToStringData() => $"{AssemblyName}|{TypeName}\t{string.Join('\t', MembersValues.Select(kv => $"{kv.Key}:{kv.Value}"))}";

        private static KeyValuePair<string, string> StringAsKeyValue(string s)
        {
            var split = s.Split(':');
            return new(split[0], split[1]);
        }
    }
}