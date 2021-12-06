// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenFwork.Graphics3d.Shapes;
using HenFwork.Worlds.Functional.Chunks;
using System.Collections.Generic;
using System.Numerics;

namespace HenFwork.Graphics3d
{
    /// <summary>
    ///     Helps visualize a <see cref="ChunksAreaObserver"/>
    ///     by giving an option to show the observed area
    ///     and/or loaded chunks.
    /// </summary>
    public class ChunksAreaObserverVisualizer
    {
        private readonly Dictionary<Chunk, ChunkIndicator> chunkIndicators = new();

        private readonly VisibleAreaIndicator visibleAreaIndicator;
        private bool chunksVisible;
        private bool observedAreaVisible;

        public bool ChunksVisible
        {
            get => chunksVisible;
            set
            {
                if (chunksVisible == value)
                    return;

                chunksVisible = value;
                foreach (var chunkIndicator in chunkIndicators.Values)
                {
                    if (value)
                        Scene.Spatials.Add(chunkIndicator);
                    else
                        Scene.Spatials.Remove(chunkIndicator);
                }
            }
        }

        public bool ObservedAreaVisible
        {
            get => observedAreaVisible;
            set
            {
                if (observedAreaVisible == value)
                    return;

                observedAreaVisible = value;
                if (value)
                    Scene.Spatials.Add(visibleAreaIndicator);
                else
                    Scene.Spatials.Remove(visibleAreaIndicator);
            }
        }

        public Scene Scene { get; }

        public ChunksAreaObserverVisualizer(ChunksAreaObserver observer, Scene scene)
        {
            observer.BeganObservingChunk += OnBeganObservingChunk;
            observer.StoppedObservingChunk += OnStoppedObservingChunk;
            Scene = scene;
            visibleAreaIndicator = new VisibleAreaIndicator(observer);
        }

        private void OnBeganObservingChunk(Chunk chunk)
        {
            var chunkIndicator = new ChunkIndicator(chunk);
            chunkIndicators.Add(chunk, chunkIndicator);
            if (ChunksVisible)
                Scene.Spatials.Add(chunkIndicator);
        }

        private void OnStoppedObservingChunk(Chunk chunk)
        {
            if (ChunksVisible)
                Scene.Spatials.Remove(chunkIndicators[chunk]);
            chunkIndicators.Remove(chunk);
        }

        protected class ChunkIndicator : BoxSpatial
        {
            public Chunk Chunk { get; }

            public ChunkIndicator(Chunk chunk)
            {
                Chunk = chunk;
                Box = HenBstractions.Numerics.Box.FromPositionAndSize(new(chunk.Coordinates.Center.X, 0, chunk.Coordinates.Center.Y), new(chunk.Coordinates.Size.X, 0, chunk.Coordinates.Size.Y), new(0.5f));
                Color = null;
                WireColor = ColorInfo.LIGHTGRAY;
            }
        }

        protected class VisibleAreaIndicator : BoxSpatial
        {
            private readonly ChunksAreaObserver observer;

            public VisibleAreaIndicator(ChunksAreaObserver observer)
            {
                this.observer = observer;
                Color = null;
                WireColor = ColorInfo.BLACK;
            }

            protected override void OnUpdate(float elapsed)
            {
                base.OnUpdate(elapsed);
                var rect = observer.ObservedArea;
                Box = new(rect.Left, rect.Right, 0.1f, 0.1f, rect.Bottom, rect.Top);
            }

            protected override void OnRender()
            {
                base.OnRender();
                var circlePos = new Vector3(observer.ObservedArea.Center.X, 0, observer.ObservedArea.Center.Y);
                Drawing.DrawCylinder(circlePos, 0.3f, 0.3f, 1, 4, ColorInfo.BLACK);
            }
        }
    }
}