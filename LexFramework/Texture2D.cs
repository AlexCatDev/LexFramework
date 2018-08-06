using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace LexFramework
{
    //TODO: Improve loading so that we dont have to load the entire image into ram, and can load chunks at a time
    //TODO: Add texture slots maybe?
    public class Texture2D
    {
        public int ID { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Texture2D(string textureFilePath) {
            ID = GL.GenTexture();
            Bind();

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            Bitmap bmp = new Bitmap(textureFilePath);

            Width = bmp.Width;
            Height = bmp.Height;

            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), 
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bmp.UnlockBits(data);
            
            bmp.Dispose();

            Unbind();
        }

        /*
        private void load(string textureFilePath, int bufferSize = 8192) {
            byte[] buffer = new byte[bufferSize];
            int xOffset = 0;
            int yOffset = 0;
            int width = 0;
            int height = 0;

            int filePosition = 0;
            using(FileStream fs = File.Open(textureFilePath, FileMode.Open)) {
                int read = fs.Read(buffer, 0, buffer.Length);

                GL.TexSubImage2D(TextureTarget.Texture2D, 0, xOffset, yOffset, width, height, PixelFormat.Rgba, PixelType.Byte, buffer);
                filePosition += read;

                width = ((y * width) + x) * 4; ;
            }
        }
        */

        public void Bind() {
            GL.BindTexture(TextureTarget.Texture2D, ID);
        }

        public void Unbind() {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }
}
