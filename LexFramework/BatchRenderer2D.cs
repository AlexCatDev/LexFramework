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
        public static readonly int IndiciesPerRender = 6;
        public static readonly int MaxRenderIndicies = MaxRender * IndiciesPerRender;

        Vertex* mappedVertexBufferPtr;

        private int indexCount = 0;

        private GLBuffer<ushort> indexBuffer;
        private GLBuffer<Vertex> vertexBuffer;
        public BatchRenderer2D() {
            vertexBuffer = new GLBuffer<Vertex>(BufferTarget.ArrayBuffer, BufferUsageHint.DynamicDraw);
            vertexBuffer.SetData(MaxRenderSize, IntPtr.Zero);

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
            
            indexBuffer = new GLBuffer<ushort>(BufferTarget.ElementArrayBuffer, BufferUsageHint.StaticDraw);
            indexBuffer.SetData(indicies);

        }

        public void Begin() {
            mappedVertexBufferPtr = (Vertex*)vertexBuffer.Map(BufferAccess.WriteOnly);
        }

        public void Submit(Vertex[] verts) {
            if (indexCount >= MaxRenderIndicies) {
                //Console.WriteLine("Render buffer exeeded flushing and beginning from new..");
                End();
                Begin();
            }

            mappedVertexBufferPtr->position = verts[0].position;
            mappedVertexBufferPtr->texCoord = verts[0].texCoord;
            mappedVertexBufferPtr++;

            mappedVertexBufferPtr->position = verts[1].position;
            mappedVertexBufferPtr->texCoord = verts[1].texCoord;
            mappedVertexBufferPtr++;

            mappedVertexBufferPtr->position = verts[2].position;
            mappedVertexBufferPtr->texCoord = verts[2].texCoord;
            mappedVertexBufferPtr++;

            mappedVertexBufferPtr->position = verts[3].position;
            mappedVertexBufferPtr->texCoord = verts[3].texCoord;
            mappedVertexBufferPtr++;

            indexCount += 6;
        }

        public void End() {
            vertexBuffer.Unmap();

            indexBuffer.Bind();
            GL.DrawElements(PrimitiveType.Triangles, indexCount, DrawElementsType.UnsignedShort, 0);
            indexCount = 0;
        }
    }
}
