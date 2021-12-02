﻿// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Extensions;
using System.Numerics;

namespace HenBstractions.Graphics
{
    public class Camera
    {
        private Vector3? rotation;
        private Vector3? lookingAt;

        public Vector3 Position { get; set; }

        public Vector3? Rotation
        {
            get => rotation;
            set
            {
                if (value is not null)
                {
                    rotation = value;
                    lookingAt = null;
                }
                else
                {
                    rotation = Vector3.Zero;
                    lookingAt = null;
                }
            }
        }

        public Vector3? LookingAt
        {
            get => lookingAt;
            set
            {
                if (value is not null)
                {
                    lookingAt = value;
                    rotation = null;
                }
                else
                {
                    rotation = Vector3.Zero;
                    lookingAt = null;
                }
            }
        }

        public Raylib_cs.Camera3D RaylibCamera { get; private set; }

        public Camera() => Rotation = Vector3.Zero;

        public void Update()
        {
            RaylibCamera = new Raylib_cs.Camera3D
            {
                position = Position,
                up = new Vector3(0, -1, 0),
                target = CalculateWhereLookingAt(),
                fovy = 70,
                projection = Raylib_cs.CameraProjection.CAMERA_PERSPECTIVE
            };
        }

        public Vector3 CalculateWhereLookingAt()
        {
            if (LookingAt.HasValue)
                return LookingAt.Value;

            var point = Position;
            var direction = new Vector3(0, 0, 1).GetRotated(Rotation!.Value);
            return point += direction;
        }
    }
}