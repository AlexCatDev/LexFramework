using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.Runtime.InteropServices;

namespace LexFramework
{
    public unsafe class BatchRenderer2D {
        public static readonly int MaxRender = 10000;
        public static readonly int VerticiesPerRender = 4;
        public static readonly int MaxRenderSize = (MaxRender * VerticiesPerRender) * Vertex.SizeInBytes;
        public static readonly int MaxRenderIndicies = MaxRender * 6;

        int vbo;
        Vertex* mappedVertexBufferPtr;
        int ibo;
        int vao;

        private int indexCount = 0;
        public BatchRenderer2D() {
            vao = GL.GenVertexArray();
            vbo = GL.GenBuffer();
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            GL.BufferData(BufferTarget.ArrayBuffer, MaxRenderSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);

            //Input vertex positions
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, Marshal.OffsetOf<Vertex>("position"));

            //Input texture positions positions
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, Marshal.OffsetOf<Vertex>("texCoord"));

            ushort[] indicies = new ushort[MaxRenderIndicies];

            int offset = 0;
            for (int i = 0; i < MaxRenderIndicies; i += 6) {
                indicies[  i  ] = (ushort)(offset + 0);
                indicies[i + 1] = (ushort)(offset + 1);
                indicies[i + 2] = (ushort)(offset + 2);

                indicies[i + 3] = (ushort)(offset + 2);
                indicies[i + 4] = (ushort)(offset + 3);
                indicies[i + 5] = (ushort)(offset + 0);

                offset += 4;
            }

            ibo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, MaxRenderIndicies, indicies, BufferUsageHint.StaticDraw);

        }

        public void Begin() {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            
            mappedVertexBufferPtr = (Vertex*)GL.MapBuffer(BufferTarget.ArrayBuffer, BufferAccess.WriteOnly);
        }

        public void Submit(Vertex[] verts) {
            if (indexCount >= MaxRenderIndicies) {
                //Console.WriteLine("Render buffer exeeded flushing and beginning from new..");
                End();
                Begin();
            }
            
            for (int i = 0; i < verts.Length; i++) {
                mappedVertexBufferPtr->position = verts[i].position;
                mappedVertexBufferPtr->texCoord = verts[i].texCoord;
                mappedVertexBufferPtr++;
            }


            indexCount += 6;
        }

        public void End() {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.UnmapBuffer(BufferTarget.ArrayBuffer);

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);

            GL.DrawElements(PrimitiveType.Triangles, indexCount, DrawElementsType.UnsignedShort, 0);
            indexCount = 0;
        }
    }
}
