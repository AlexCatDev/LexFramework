using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexFramework
{
    public class Transform
    {
        public Vector2 Position;
        public Vector2 Scale;
        public float Rotation;

        public Transform(Vector2 position, Vector2 scale, float rotation) {
            Position = position;
            Scale = scale;
            Rotation = rotation;
        }

        public Transform() : this(Vector2.Zero, Vector2.One, 0f) { }

        public Matrix4 TransformationMatrix {
            get {
                Matrix4 translationMatrix = Matrix4.CreateTranslation(Position.X, Position.Y, 0);
                Matrix4 rotationMatrix = Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(0, 0, Rotation));
                Matrix4 scaleMatrix = Matrix4.CreateScale(Scale.X, Scale.Y, 1);

                return scaleMatrix * rotationMatrix * translationMatrix;
            }
        }
    }
}
