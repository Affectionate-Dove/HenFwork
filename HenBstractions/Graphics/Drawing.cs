// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Numerics;
using Raylib_cs;
using System.Numerics;

namespace HenBstractions.Graphics
{
    public static class Drawing
    {
        public static void BeginDrawing() => Raylib.BeginDrawing();

        public static void BeginMode3D(Camera camera) => Raylib.BeginMode3D(camera.RaylibCamera);

        public static void EndMode3D() => Raylib.EndMode3D();

        public static void EndDrawing() => Raylib.EndDrawing();

        public static void ClearBackground(ColorInfo color) => Raylib.ClearBackground(color);

        public static void BeginTextureMode(RenderTexture renderTexture) => Raylib.BeginTextureMode(renderTexture.RenderTexture2D);

        public static void EndTextureMode() => Raylib.EndTextureMode();

        public static void EndScissorMode() => Raylib.EndScissorMode();

        public static void BeginScissorMode(RectangleF mask) => Raylib.BeginScissorMode((int)mask.Left, (int)mask.Top, (int)mask.Width, (int)mask.Height);

        public static void DrawCircle(Circle circle, ColorInfo color) => Raylib.DrawCircleV(circle.CenterPosition, circle.Radius, color);

        public static void DrawRectangle(RectangleF rectangle, ColorInfo color) => Raylib.DrawRectangleV(rectangle.TopLeft, rectangle.Size, color);

        public static void DrawRectangleLines(RectangleF rectangle, float thickness, ColorInfo color)
        {
            var ht = thickness * 0.5f;
            DrawLine(new(rectangle.Left, rectangle.Top + ht), new(rectangle.Right - thickness, rectangle.Top + ht), thickness, color);
            DrawLine(new(rectangle.Right - ht, rectangle.Top), new(rectangle.Right - ht, rectangle.Bottom - thickness), thickness, color);
            DrawLine(new(rectangle.Right, rectangle.Bottom - ht), new(rectangle.Left + thickness, rectangle.Bottom - ht), thickness, color);
            DrawLine(new(rectangle.Left + ht, rectangle.Bottom), new(rectangle.Left + ht, rectangle.Top + thickness), thickness, color);
        }

        public static void DrawTriangle3D(Triangle3 triangle, ColorInfo color) => Raylib.DrawTriangle3D(triangle.A, triangle.B, triangle.C, color);

        public static void DrawCube(Vector3 position, Vector3 size, ColorInfo color) => Raylib.DrawCubeV(position, size, color);

        public static void DrawLine3D(Vector3 a, Vector3 b, ColorInfo color) => Raylib.DrawLine3D(a, b, color);

        public static void DrawCubeWires(Vector3 position, Vector3 size, ColorInfo color) => Raylib.DrawCubeWiresV(position, size, color);

        public static void DrawTexture(Texture texture, RectangleF source, RectangleF target, Vector2 origin, float rotation, ColorInfo tint) => Raylib.DrawTexturePro(texture.Texture2D, ToRaylibRectangle(source), ToRaylibRectangle(target), origin, rotation, tint);

        public static void DrawLine(Vector2 a, Vector2 b, float thickness, ColorInfo color) => Raylib.DrawLineEx(a, b, thickness, color);

        public static void DrawTexture(RenderTexture texture, RectangleF source, RectangleF target, Vector2 origin, float rotation, ColorInfo tint) => Raylib.DrawTexturePro(texture.RenderTexture2D.texture, ToRaylibRectangle(source), ToRaylibRectangle(target), origin, rotation, tint);

        // TODO tint default value
        public static void DrawTexture(Texture texture, Vector2 position, ColorInfo tint) => Raylib.DrawTexture(texture.Texture2D, (int)position.X, (int)position.Y, tint);

        public static void DrawTexture(Texture texture, Vector2 position) => Raylib.DrawTexture(texture.Texture2D, (int)position.X, (int)position.Y, new ColorInfo(255, 255, 255));

        public static void DrawModel(Model model, Vector3 position, Vector3 rotationAxis, float rotationAngle, Vector3 scale, ColorInfo tint) => Raylib.DrawModelEx(model.InternalModel, position, rotationAxis, rotationAngle, scale, tint);

        public static void DrawText(Font font, string text, Vector2 position, float fontSize, float letterSpacing, ColorInfo color) => Raylib.DrawTextEx(font.InternalFont, text, position, fontSize, letterSpacing, color);

        public static Vector2 MeasureText(Font font, string text, float fontSize, float letterSpacing) => Raylib.MeasureTextEx(font.InternalFont, text, fontSize, letterSpacing);

        public static void DrawTriangleLines(Triangle2 triangle, ColorInfo color) => Raylib.DrawTriangleLines(triangle.A, triangle.B, triangle.C, color);

        public static void DrawTriangle(Triangle2 triangle, ColorInfo color) => Raylib.DrawTriangle(triangle.A, triangle.B, triangle.C, color);

        public static void DrawCylinder(Vector3 position, float radiusTop, float radiusBottom, int height, int slices, ColorInfo color) => Raylib.DrawCylinder(position, radiusTop, radiusBottom, height, slices, color);

        private static Rectangle ToRaylibRectangle(RectangleF source) => new(source.Left, source.Top, source.Width, source.Height);
    }
}