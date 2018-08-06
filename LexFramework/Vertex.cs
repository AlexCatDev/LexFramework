using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexFramework
{
    public struct Vertex
    {
        public Vector2 position;
        public Vector2 texCoord;

        public static int SizeInBytes => Vector2.SizeInBytes * 2;

        public Vertex(Vector2 position, Vector2 texCoord) {
            this.position = position;
            this.texCoord = texCoord;
        }

        public Vertex(Vector2 position) : this(position, Vector2.Zero) { }

        public static Vertex Default => new Vertex(Vector2.Zero, Vector2.Zero);
    }
}
