using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
namespace ConsoleApp1
{
    class Objeto
    {
       

        // Constructor que recibe x, y, z y la lista de vertices
        
        private int _vao;
        private int _vbo;
        private int _shaderProgram;
        private int _mvpLocation;
        public List<Objeto> partes { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }

        //public int escala { get; set; } en desarrollo
        public float[] VerticePersonalizado(float[] vertices, float x, float y, float z)
        {

            
            for (int i = 0; i < vertices.Length; i++)
            {
                int coord = i % 6; // 0=x, 1=y, 2=z, 3=r, 4=g, 5=b

                switch (coord)
                {
                    case 0: vertices[i] += x; break; // x
                    case 1: vertices[i] += y; break; // y
                    case 2: vertices[i] += z; break; // z
                }
                //vertices.SetValue(i,5);
            }
            return vertices;
        }
        public Objeto(float x, float y, float z, float[] vertice)
        {
            
            // Vértices del cubo (posX, posY, posZ, r, g, b)
            float [] vertices = VerticePersonalizado(vertice, x, y, z);
         

            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);

            _vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // Shaders
            string vertexShaderSource = @"
            #version 330 core
            layout(location = 0) in vec3 aPosition;
            layout(location = 1) in vec3 aColor;
            uniform mat4 uMVP;
            out vec3 vColor;
            void main()
            {
                gl_Position = uMVP * vec4(aPosition, 1.0);
                vColor = aColor;
            }
        ";
            string fragmentShaderSource = @"
            #version 330 core
            in vec3 vColor;
            out vec4 FragColor;
            void main()
            {
                FragColor = vec4(vColor, 1.0);
            }
        ";

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);
            GL.CompileShader(fragmentShader);

            _shaderProgram = GL.CreateProgram();
            GL.AttachShader(_shaderProgram, vertexShader);
            GL.AttachShader(_shaderProgram, fragmentShader);
            GL.LinkProgram(_shaderProgram);

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            // Atributos
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            _mvpLocation = GL.GetUniformLocation(_shaderProgram, "uMVP");
        }

        public void Draw(Matrix4 mvp)
        {
            GL.UseProgram(_shaderProgram);
            GL.UniformMatrix4(_mvpLocation, false, ref mvp);
            GL.BindVertexArray(_vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(_vbo);
            GL.DeleteVertexArray(_vao);
            GL.DeleteProgram(_shaderProgram);
        }
    }





}
