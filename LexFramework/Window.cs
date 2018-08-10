using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace LexFramework
{
    public class Window
    {
        private GameWindow window;

        public VSyncMode VSync {
            get { return window.VSync; }
            set { window.VSync = value; }
        }

        public string Title {
            get { return window.Title; }
            set { window.Title = value; }
        }

        public int Width {
            get { return window.Width; }
            set { window.Width = value; }
        }

        public int Height {
            get { return window.Height; }
            set { window.Height = value; }
        }

        public int X {
            get { return window.X; }
            set { window.X = value; }
        }

        public int Y {
            get { return window.Y; }
            set { window.Y = value; }
        }

        public Window(int width, int height, string title) {
            window = new GameWindow(width, height, OpenTK.Graphics.GraphicsMode.Default, title);
            window.RenderFrame += (s, e) => OnRender(e);
            window.UpdateFrame += (s, e) => OnUpdate(e);

            window.Resize += (s, e) => OnResize();
            window.MouseMove += (s, e) => MouseMove(e);

            window.Load += (s, e) => OnLoad();
        }

        public virtual void OnRender(FrameEventArgs e) {
            window.SwapBuffers();
        }
        public virtual void OnUpdate(FrameEventArgs e) { }

        public virtual void MouseMove(MouseEventArgs e) { }

        public virtual void OnResize() {
            GL.Viewport(0, 0, window.Width, window.Height);
        }

        public virtual void OnLoad() { }

        public void Run() {
            window.Run();
        }
    }
}
