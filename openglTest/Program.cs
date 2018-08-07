using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Graphics;
using System.Diagnostics;
using System.Threading;
using System.IO;
using LexFramework;

namespace openglTest
{
    class Program
    {
        static void Main(string[] args) {
            GameWindow window = new GameWindow(1280, 720); //,new GraphicsMode(new ColorFormat(8, 8, 8, 0), 24, 8, 8));
            int mouseX = 0, mouseY = 0;
            window.MouseMove += (s, e) => {
                mouseX = e.X;
                mouseY = e.Y;
            };
            window.VSync = VSyncMode.Off;
            float angle = 0;

            Console.Title =  $"{GL.GetString(StringName.Vendor)} OpenGL {GL.GetString(StringName.Version)} GLSL {GL.GetString(StringName.ShadingLanguageVersion)}";
            window.Resize += (s, e) => {
                GL.Viewport(0, 0, window.Width, window.Height);
                ImmediateMode.SetupView(window.Width, window.Height);
            };

            GLBuffer<Vertex> vb = new GLBuffer<Vertex>();
            vb.SetData(VertexGenerator.GetQuad());
            BatchRenderer2D batch = new BatchRenderer2D();
            Shader shader = new Shader("./vert.glsl", "./frag.glsl");
            shader.AddUniform("projectionMatrix");
            shader.AddUniform("inputColor");
            shader.AddUniform("tex");
            Texture2D tex = new Texture2D("./lmao.png");
            float moveX = 0;
            GL.ActiveTexture(TextureUnit.Texture0);
            window.RenderFrame += (s, e) => {
                ImmediateMode.MandatoryLogic();

                MouseState mInput = Mouse.GetState();
                int x = mouseX;
                int y = mouseY;
                
                int width = 100;
                int height = 100;
                angle += 3 * (float)e.Time;
                moveX += 100f * (float)e.Time;
                
                Transform t = new Transform();
                    t.Position = new Vector2(x, y);
                    t.Rotation = angle;
                    t.Scale = new Vector2(100, 100);
                Matrix4 projection = Matrix4.CreateOrthographicOffCenter(0, 1280, 720, 0, -1, 1);
                shader.Bind();
                shader.SetMatrix("projectionMatrix", t.TransformationMatrix * projection);
                shader.SetVector("inputColor", new Vector4(0, 0, 1, 1));
                shader.SetInt("tex", 0);
                tex.Bind();
                batch.Begin();
                for (int x1 = 0; x1 < 1; x1++) {
                    batch.Submit(VertexGenerator.GetQuad());
                }
                batch.End();
                tex.Unbind();
                shader.Unbind();
                // ImmediateMode.RenderQuad(x, y, 500, 500, new Vector4(1, 1, 1, 1));
                //ImmediateMode.RenderVerticies(new Vector2(x,y), new Vector2(100,100), angle, VertexGenerator.GetQuad(), PrimitiveType.Quads, Vector4.One);

                window.SwapBuffers();
            };


            int fps = 0;
            double elapsedTime = 0;
            window.UpdateFrame += (s, e) => {
                fps++;
                elapsedTime += e.Time;
                if (elapsedTime >= 1.0) {
                    window.Title = "FPS: " + fps;
                    elapsedTime = 0;
                    fps = 0;
                }
            };
            window.Run();
        }

        static void Benchmark(Action a, string name = "") {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            a?.Invoke();
            sw.Stop();
            Console.WriteLine($"Action {name } => {Math.Round(((double)sw.ElapsedTicks / Stopwatch.Frequency) * 1000.0, 3)}");
        }
    }
}
