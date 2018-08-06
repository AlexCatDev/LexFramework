using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace LexFramework
{

    public class IndexBuffer
    {
        public int IndexCount { get; private set; }
        public int ID { get; private set; }

        public int SizeInBytes => IndexCount * 4;

        public IndexBuffer(uint[] indicies) {
            ID = GL.GenBuffer();
            SetData(indicies);
        }

        public void SetData(uint[] indicies) {
            IndexCount = indicies.Length;

            Bind();
            GL.BufferData(BufferTarget.ElementArrayBuffer, SizeInBytes, indicies, BufferUsageHint.StaticDraw);
        }

        public void Update(int startIndex, uint[] indicies) {
            if (indicies.Length - startIndex - 1 > IndexCount)
                throw new InvalidDataException($"The input doesnt fit into the buffer {indicies.Length - startIndex - 1} > {IndexCount}");

            Bind();
            GL.BufferSubData(BufferTarget.ElementArrayBuffer, (IntPtr)(startIndex * 4), indicies.Length * 4, indicies);
        }

        public void Bind() {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ID);
        }
    }
}
