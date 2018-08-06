using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace LexFramework
{
    public class PixelBuffer
    {
        private byte[] buffer;
        private int width, height;

        public PixelBuffer(int width, int height) {
            buffer = new byte[width * height * 4];
            this.width = width;
            this.height = height;
        }

        public void SetPixel(int x, int y, byte r, byte g, byte b, byte a) {
            int i = ((y * width) + x) * 4;
            buffer[i] = r;
            buffer[i + 1] = g;
            buffer[i + 2] = b;
            buffer[i + 3] = a;
        }

        public void Render(int x, int y, int viewportWidth, int viewportHeight) {
            GL.RasterPos2(0, 0);
            GL.WindowPos2(x, viewportHeight - y - height);
            GL.DrawPixels(width, height, PixelFormat.Rgba, PixelType.UnsignedByte, buffer);
        }
    }
}
