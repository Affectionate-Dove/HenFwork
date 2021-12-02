// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

namespace HenBstractions.Graphics
{
    public class Model
    {
        ~Model() => Raylib_cs.Raylib.UnloadModel(InternalModel);

        internal Raylib_cs.Model InternalModel { get; }

        public Model(string path) => InternalModel = Raylib_cs.Raylib.LoadModel(path);
    }
}