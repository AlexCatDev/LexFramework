using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace LexFramework
{
    public class GLBuffer<T> where T : struct
    {
        private BufferTarget bufferType;
        private BufferUsageHint bufferUsage;

        private int sizeofType;

        public int ID { get; private set; }
        public int Count { get; private set; }

        public int SizeInBytes => Count * sizeofType;

        public GLBuffer(BufferTarget bufferType = BufferTarget.ArrayBuffer, BufferUsageHint bufferUsage = BufferUsageHint.StaticDraw) {
            this.bufferType = bufferType;
            this.bufferUsage = bufferUsage;

            sizeofType = Marshal.SizeOf<T>();

            ID = GL.GenBuffer();
        }

        /// <summary>
        /// Maps GPU data into the cpu and return an IntPtr pointing to that data, use (<typeparamref name="T"/>*)Map(...) for speed xD
        /// </summary>
        /// <param name="bufferAccess"></param>
        /// <returns></returns>
        public IntPtr Map(BufferAccess bufferAccess) {
            Bind();
            return GL.MapBuffer(bufferType, bufferAccess);
        }

        public bool Unmap() {
            Bind();
            return GL.UnmapBuffer(bufferType);
        }

        //Makes OpenGL delete old buffer and make a new
        public void SetData(T[] objects) {
            Bind();
            int sizeInBytes = objects.Length * sizeofType;

            GL.BufferData(bufferType, sizeInBytes, objects, bufferUsage);

            Count = objects.Length;
        }

        //Makes OpenGL delete old buffer and make a new
        public void SetData(int size, IntPtr ptr) {
            Bind();
            int sizeInBytes = size * sizeofType;

            GL.BufferData(bufferType, sizeInBytes, ptr, bufferUsage);

            Count = size;
        }

        //Updates current buffer
        public void UpdateData(int startIndex, T[] objects) {
            Bind();
            int sizeInBytes = objects.Length * sizeofType;
            int offsetInBytes = startIndex * sizeofType;

            GL.BufferSubData(bufferType, (IntPtr)offsetInBytes, sizeInBytes, objects);
        }

        public void Bind() {
            GL.BindBuffer(bufferType, ID);
        }
    }
}
