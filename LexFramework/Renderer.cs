using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexFramework
{
    public class Renderer
    {
        public static void Render(VertexBuffer vertexBuffer, PrimitiveType vertexFormat = PrimitiveType.Quads) {
            vertexBuffer.Bind();

            //Input vertex positions
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 0);

            //Draw array
            GL.DrawArrays(vertexFormat, 0, vertexBuffer.VertexCount);
        }

        public static void Render(VertexBuffer vertexBuffer, IndexBuffer indexBuffer, Shader shader, PrimitiveType vertexFormat = PrimitiveType.Quads) {
            vertexBuffer.Bind();
            indexBuffer.Bind();

            //Input vertex positions
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 0);

            //Input texture positions positions
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, Vector2.SizeInBytes);


            //Draw array
            GL.DrawElements(vertexFormat, indexBuffer.IndexCount, DrawElementsType.UnsignedInt, 0);
        }
    }
}
