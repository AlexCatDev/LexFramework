using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace LexFramework
{

    public class VertexBuffer
    {
        public int VertexCount { get; private set; }
        public int ID { get; private set; }

        public int SizeInBytes => VertexCount * Vertex.SizeInBytes;

        public VertexBuffer(Vertex[] verticies) {
            ID = GL.GenBuffer();
            SetData(verticies);
        }

        public void SetData(Vertex[] verticies) {
            VertexCount = verticies.Length;

            Bind();
            GL.BufferData(BufferTarget.ArrayBuffer, SizeInBytes, verticies, BufferUsageHint.StaticDraw);
        }

        public void Update(int startIndex, Vertex[] verticies) {
            if (verticies.Length - startIndex - 1 > VertexCount)
                throw new InvalidDataException($"The input doesnt fit into the buffer {verticies.Length - startIndex - 1} > {VertexCount}");

            Bind();
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)(startIndex * Vertex.SizeInBytes), verticies.Length * Vertex.SizeInBytes, verticies);
        }

        public void Bind() {
            GL.BindBuffer(BufferTarget.ArrayBuffer, ID);
        }
    }
}
