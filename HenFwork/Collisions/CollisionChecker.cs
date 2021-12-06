// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Extensions;
using HenBstractions.Numerics;
using HenFwork.Worlds.Functional.Mediums;
using HenFwork.Worlds.Functional.Nodes;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace HenFwork.Collisions
{
    public static class CollisionChecker
    {
        /// <summary>
        /// Checks all provided nodes for overlapping
        /// <see cref="Node.CollisionBody"/>'s.
        /// </summary>
        /// <remarks>
        /// <paramref name="onCollision"/> is only called once per
        /// a pair of <see cref="Node"/>s.
        /// </remarks>
        public static void CheckNodeCollisions(IReadOnlyList<Node> nodes, Action<Node, Node> onCollision)
        {
            for (var i = 0; i < nodes.Count - 1; i++)
            {
                var a = nodes[i];
                if (a.CollisionBody is null)
                    continue;

                for (var j = i + 1; j < nodes.Count; j++)
                {
                    var b = nodes[j];
                    if (b.CollisionBody is null)
                        continue;

                    var containingSphereA = a.CollisionBody.ContainingSphere + a.Position;
                    var containingSphereB = b.CollisionBody.ContainingSphere + b.Position;

                    if (ElementaryCollisions.AreSpheresColliding(containingSphereA, containingSphereB))
                    {
                        if (a.CollisionBody.Spheres.Count is 1 && b.CollisionBody.Spheres.Count is 1)
                            onCollision(a, b);
                        else if (ThouroughlyCheckNodesForCollision(a, b))
                            onCollision(a, b);
                    }
                }
            }
        }

        /// <summary>
        /// Approximately checks whether the whole
        /// <paramref name="node"/>'s <see cref="Node.CollisionBody"/>
        /// is contained within <paramref name="mediums"/>.
        /// </summary>
        public static bool IsNodeContainedInMediums(Node node, IEnumerable<Medium> mediums)
        {
            if (node.CollisionBody is null)
                return IsPointContainedInMediums(node.Position.ToTopDownPoint(), mediums);

            foreach (var localSphere in node.CollisionBody.Spheres)
            {
                var absSphere = localSphere + node.Position;
                if (!IsCircleContainedInMediums(absSphere.ToTopDownCircle(), mediums))
                    return false;
            }
            return true;
        }

        private static IEnumerable<Vector2> SunflowerPoints(Circle circle, int pointsAmount)
        {
            // credit: https://stackoverflow.com/a/28572551

            const float alpha = 1;
            var boundaryPointsCount = (int)(alpha * MathF.Sqrt(pointsAmount));
            var phi = (MathF.Sqrt(5) + 1) / 2; // golden ratio

            float Radius(int k) => k > pointsAmount - boundaryPointsCount ? 1 : MathF.Sqrt(k - 0.5f) / MathF.Sqrt(pointsAmount - ((boundaryPointsCount + 1) * 0.5f));

            for (var k = 1; k <= pointsAmount; k++)
            {
                var r = Radius(k) * circle.Radius;
                var theta = 2 * MathF.PI * k / (phi * phi);
                yield return new Vector2(r * MathF.Cos(theta), r * MathF.Sin(theta)) + circle.CenterPosition;
            }
        }

        private static bool IsPointContainedInMediums(Vector2 p, IEnumerable<Medium> mediums)
        {
            foreach (var medium in mediums)
            {
                if (ElementaryCollisions.IsPointInTriangle(p, medium.Triangle.ToTopDownTriangle()))
                    return true;
            }
            return false;
        }

        private static bool IsCircleContainedInMediums(Circle circle, IEnumerable<Medium> mediums)
        {
            const float pointsPerSquareUnit = 50;
            var pointsAmount = (int)(pointsPerSquareUnit * circle.Area);
            foreach (var point in SunflowerPoints(circle, pointsAmount))
            {
                var pointContained = false;
                foreach (var medium in mediums)
                {
                    if (ElementaryCollisions.IsPointInTriangle(point, medium.Triangle.ToTopDownTriangle()))
                    {
                        pointContained = true;
                        break;
                    }
                }

                // if no triangles contain this point, the circle is not contained in any medium
                if (!pointContained)
                    return false;
            }
            return true;
        }

        private static bool ThouroughlyCheckNodesForCollision(Node a, Node b)
        {
            foreach (var aLocalSphere in a.CollisionBody.Spheres)
            {
                var aAbsSphere = aLocalSphere + a.Position;
                foreach (var bLocalSphere in b.CollisionBody.Spheres)
                {
                    var bAbsSphere = bLocalSphere + b.Position;
                    if (ElementaryCollisions.AreSpheresColliding(aAbsSphere, bAbsSphere))
                        return true;
                }
            }
            return false;
        }
    }
}