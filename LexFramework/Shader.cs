using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.IO;

namespace LexFramework
{
    public class Shader
    {
        private readonly int programID;
        private readonly Dictionary<string, int> uniformLocationCache;

        public Shader(string vertexFileName, string fragmentFileName) {
            programID = GL.CreateProgram();
            uniformLocationCache = new Dictionary<string, int>();

            if (programID == 0) {
                Console.WriteLine("Error creating shader: Could not generate program buffer.");
            }

            AddShader(vertexFileName, ShaderType.VertexShader);
            AddShader(fragmentFileName, ShaderType.FragmentShader);
            CompileShader();
        }

        private void AddShader(string fileName, ShaderType type) {
            string shader = readShaderFromFile(fileName);
            int shaderID = GL.CreateShader(type);
            if (shaderID == 0) {
                Console.WriteLine("Error creating shader: Could not generate shader buffer.");
            }

            GL.ShaderSource(shaderID, shader);
            GL.CompileShader(shaderID);

            int compileStatus;
            GL.GetShader(shaderID, ShaderParameter.CompileStatus, out compileStatus);
            if (compileStatus == 0) {
                Console.WriteLine("Error compiling shader: Could not compile shader.");
                Console.WriteLine(GL.GetShaderInfoLog(shaderID));
            }

            GL.AttachShader(programID, shaderID);
        }

        private void CompileShader() {
            GL.LinkProgram(programID);
            int linkStatus;
            GL.GetProgram(programID, GetProgramParameterName.LinkStatus, out linkStatus);
            if (linkStatus == 0) {
                Console.WriteLine("Erorr linking shader program: Could not link shader program!");
                Console.WriteLine(GL.GetProgramInfoLog(programID));
            }

            GL.ValidateProgram(programID);
            int validatationStatus;
            GL.GetProgram(programID, GetProgramParameterName.ValidateStatus, out validatationStatus);
            if (validatationStatus == 0) {
                Console.WriteLine("Erorr validating shader program: Could not validate shader program!");
                Console.WriteLine(GL.GetProgramInfoLog(programID));
            }
        }

        public void Bind() {
            GL.UseProgram(programID);
        }

        public void Unbind() {
            GL.UseProgram(0);
        }

        public void AddUniform(string uniformName) {
            int uniform = GetUniformLocation(uniformName);
            if (uniform == -1) {
                Console.WriteLine("Could not find uniform: " + uniformName + "!");
            }
            uniformLocationCache.Add(uniformName, uniform);
        }

        private int GetUniformLocation(string uniformName) {
            return GL.GetUniformLocation(programID, uniformName);
        }

        public void SetInt(string uniformName, int value) {
            GL.Uniform1(uniformLocationCache[uniformName], value);
        }

        public void SetFloat(string uniformName, float value) {
            GL.Uniform1(uniformLocationCache[uniformName], value);
        }

        public void SetDouble(string uniformName, double value) {
            GL.Uniform1(uniformLocationCache[uniformName], value);
        }

        public void SetVector(string uniformName, Vector2 value) {
            GL.Uniform2(uniformLocationCache[uniformName], value);
        }

        public void SetVector(string uniformName, Vector3 value) {
            GL.Uniform3(uniformLocationCache[uniformName], value);
        }

        public void SetVector(string uniformName, Vector4 value) {
            GL.Uniform4(uniformLocationCache[uniformName], value);
        }

        public void SetBoolean(string uniformName, bool value) {
            GL.Uniform1(uniformLocationCache[uniformName], value ? 1 : 0);
        }

        public void SetMatrix(string uniformName, Matrix4 value) {
            GL.UniformMatrix4(uniformLocationCache[uniformName], true, ref value);
        }

        private static string readShaderFromFile(string fileName) {
            StringBuilder shader = new StringBuilder();

            try {
                using (StreamReader reader = new StreamReader(fileName)) {
                    string line;
                    while ((line = reader.ReadLine()) != null) {
                        shader.Append(line).Append("\n");
                    }
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            return shader.ToString();
        }
    }

}
