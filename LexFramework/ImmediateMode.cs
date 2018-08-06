using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace LexFramework
{
    public static class ImmediateMode
    {
        /// <summary>
        /// Sets matrix mode to MatrixMode.Projection then loads the identity matrix,
        /// multiplies the identity matrix with an orthographic projection and sets the matrix mode to MatrixMode.ModelView.
        /// </summary>
        /// <param name="width">Orthographic projection width</param>
        /// <param name="height">Orthographics projection height</param>
        public static void SetupView(int width, int height) {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, width, height, 0, 0, 1);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        /// <summary>
        /// Clears the screen, enables alpha, textures and loads identity matrix, bla bla bla
        /// </summary>
        public static void MandatoryLogic() {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.Texture2D);
            GL.LoadIdentity();
        }

        public static void RenderQuad(int x, int y, int width, int height, Vector4 color = default(Vector4)) {
            RenderVerticies(new Vector2(x, y), new Vector2(width, height), 0, VertexGenerator.GetQuad(), PrimitiveType.Quads, color);
        }

        public static void RenderVerticies(Vector2 position, Vector2 scale, float rotation, Vertex[] verticies, PrimitiveType vertexType, Vector4 color = default(Vector4)) {
            GL.Translate(position.X, position.Y, 0);
            GL.Rotate(rotation, 0, 0, 1);
            GL.Scale(scale.X, scale.Y, 1);
            GL.Begin(vertexType);
            GL.Color4(color.X, color.Y, color.Z, color.W);
            for (int i = 0; i < verticies.Length; i++) {
                Vertex currentVertex = verticies[i];
                GL.TexCoord2(currentVertex.texCoord);
                GL.Vertex2(currentVertex.position);
            }
            GL.End();
            GL.LoadIdentity();
        }
    }
}
