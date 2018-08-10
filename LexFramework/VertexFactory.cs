using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexFramework
{
    public static class VertexFactory
    {
        public static Vertex[] GetQuad() {

            return new Vertex[]
                {
                    new Vertex(new Vector2(-0.5f, -0.5f), new Vector2(0, 0)),
                    new Vertex(new Vector2(0.5f, -0.5f), new Vector2(1, 0)),
                    new Vertex(new Vector2(0.5f, 0.5f), new Vector2(1, 1)),
                    new Vertex(new Vector2(-0.5f, 0.5f), new Vector2(0, 1))
                };

        }

        private static Vector2[] GetLine(Vector2 start, Vector2 end, float thickness) {
            Vector2 difference = end - start;
            Vector2 perpen = new Vector2(difference.Y, -difference.X);
            perpen.Normalize();

            Vector2 topLeft = new Vector2(start.X + perpen.X * thickness / 2f,
                start.Y + perpen.Y * thickness / 2f);

            Vector2 topRight = new Vector2(start.X - perpen.X * thickness / 2f,
                start.Y - perpen.Y * thickness / 2f);

            Vector2 bottomRight = new Vector2(end.X + perpen.X * thickness / 2f,
                end.Y + perpen.Y * thickness / 2f);

            Vector2 bottomLeft = new Vector2(end.X - perpen.X * thickness / 2f,
                end.Y - perpen.Y * thickness / 2f);

            return new Vector2[]
            {
                    topLeft,
                    topRight,
                    bottomLeft,
                    bottomRight
            };
        }

        //Please don't use this function unless you are using immediatemode rendering
        public static Vertex[] GetEllipse(int sides) {

            Vertex[] verticies = new Vertex[sides];
            for (int ii = 0; ii < sides; ii++) {
                float theta = 2.0f * (float)Math.PI * ii / sides;//get the current angle

                float x = 1f * (float)Math.Cos(theta);//calculate the x component
                float y = 1f * (float)Math.Sin(theta);//calculate the y component

                verticies[ii] = new Vertex(new Vector2(x + 0.5f, y + 0.5f));//output vertex

            }
            return verticies;
        }
    }
}
