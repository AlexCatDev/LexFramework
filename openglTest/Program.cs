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
    class Program : Window
    {
        GLBuffer<Vertex> vb;
        BatchRenderer2D batch;
        Texture2D tex;
        Shader shader;

        float angle = 0;
        float moveX = 0;

        public Program() : base(1280, 720, "OpenGL Framework test") {
            VSync = VSyncMode.Off;

            Console.Title = $"{GL.GetString(StringName.Vendor)} OpenGL {GL.GetString(StringName.Version)} GLSL {GL.GetString(StringName.ShadingLanguageVersion)}";

            vb = new GLBuffer<Vertex>();
            vb.SetData(VertexFactory.GetQuad());

            batch = new BatchRenderer2D();

            tex = new Texture2D("./lmao.png");

            shader = new Shader("./vert.glsl", "./frag.glsl");
            shader.AddUniform("projectionMatrix");
            shader.AddUniform("inputColor");
            shader.AddUniform("tex");

            GL.ActiveTexture(TextureUnit.Texture0);
        }

        void Benchmark(Action a, string name = "") {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            a?.Invoke();
            sw.Stop();
            Console.WriteLine($"Action {name } => {Math.Round(((double)sw.ElapsedTicks / Stopwatch.Frequency) * 1000.0, 3)}");
        }

        int mouseX = 0;
        int mouseY = 0;

        public override void MouseMove(MouseEventArgs e) {
            mouseX = e.X;
            mouseY = e.Y;
            base.MouseMove(e);
        }

        public override void OnResize() {
            ImmediateMode.SetupView(Width, Height);
            base.OnResize();
        }

        public override void OnLoad() {
            base.OnLoad();
        }

        Transform t = new Transform();
        Matrix4 projection = Matrix4.CreateOrthographicOffCenter(0, 1280, 720, 0, -1, 1);

        public override void OnRender(FrameEventArgs e) {
            ImmediateMode.MandatoryLogic();

            int width = 100;
            int height = 100;
            angle += 3 * (float)e.Time;
            moveX += 100f * (float)e.Time;

            t.Position = new Vector2(mouseX, mouseY);
            t.Rotation = angle;
            t.Scale = new Vector2(100, 100);
            shader.Bind();
            shader.SetMatrix("projectionMatrix", t.TransformationMatrix * projection);
            shader.SetVector("inputColor", new Vector4(0, 0, 1, 1));
            shader.SetInt("tex", 0);
            tex.Bind();
            batch.Begin();
            for (int x1 = 0; x1 < 1000; x1++) {
                batch.Submit(VertexFactory.GetQuad());
            }
            batch.End();
            tex.Unbind();
            shader.Unbind();
            // ImmediateMode.RenderQuad(x, y, 500, 500, new Vector4(1, 1, 1, 1));
            //ImmediateMode.RenderVerticies(new Vector2(x,y), new Vector2(100,100), angle, VertexGenerator.GetQuad(), PrimitiveType.Quads, Vector4.One);

            base.OnRender(e);
        }

        int fps = 0;
        double elapsedTime = 0;

        public override void OnUpdate(FrameEventArgs e) {
            fps++;
            elapsedTime += e.Time;
            if (elapsedTime >= 1.0) {
                Title = "FPS: " + fps;
                elapsedTime = 0;
                fps = 0;
            }
            base.OnUpdate(e);
        }

        static void Main(string[] args) {
            new Program().Run();
        }
    }
}
