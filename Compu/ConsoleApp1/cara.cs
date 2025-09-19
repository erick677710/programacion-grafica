using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

public class Cara
{


    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public List<float> Vertices { get; set; }

    private int _vao;
    private int _vbo;
    private int _shaderProgram;
    private int _mvpLocation;
    public Matrix4 Transform { get; set; } = Matrix4.Identity;

    public void verticesPersonalizadosvoid(float x, float y, float z)
    {
        List<float> resultado = new List<float>();
        for (int i = 0; i < Vertices.Count; i++)
        {
            int coord = i % 6; // 0=x, 1=y, 2=z, 3=r, 4=g, 5=b

            switch (coord)
            {
                case 0:
                    resultado.Add(Vertices[i] + x); // x
                    break;
                case 1:
                    resultado.Add(Vertices[i] + y); // y
                    break;
                case 2:
                    resultado.Add(Vertices[i] + z); // z
                    break;
                default:
                    resultado.Add(Vertices[i]);     // r, g, b se mantiene
                    break;
            }
        }
        Vertices = resultado;
    }
    private List<float> verticesPersonalizados(List<float> verticeOrigen, float x, float y, float z) {
        List<float> resultado = new List<float>();
        for (int i = 0; i < verticeOrigen.Count; i++)
        {
            int coord = i % 6; // 0=x, 1=y, 2=z, 3=r, 4=g, 5=b

            switch (coord)
            {
                case 0:
                    resultado.Add(verticeOrigen[i] + x); // x
                    break;
                case 1:
                    resultado.Add(verticeOrigen[i] + y); // y
                    break;
                case 2:
                    resultado.Add(verticeOrigen[i] + z); // z
                    break;
                default:
                    resultado.Add(verticeOrigen[i]);     // r, g, b se mantiene
                    break;
            }
        }
        return resultado;
    }
    // Constructor vacío (para recibit el json)
    public Cara()
    {
        Vertices = new List<float>();
       
    }

    // Constructor normal
    public Cara(float x, float y, float z, List<float> vertices)
    {
        X = x;
        Y = y;
        Z = z;
        Vertices = vertices;
        verticesPersonalizadosvoid(x, y, z);
        InitGL();
    }

    // Inicializar OpenGL (buffers + shaders)
    public void InitGL()
    {
        // Generar VAO y VBO
        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);

        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Count * sizeof(float), Vertices.ToArray(), BufferUsageHint.StaticDraw);

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

        // Configurar atributos de vértices
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        _mvpLocation = GL.GetUniformLocation(_shaderProgram, "uMVP");
    }

    // Dibujar usando la matriz interna Transform  , Matrix4 projection
    public void Draw(Matrix4 view)
    {
        GL.UseProgram(_shaderProgram);

        // MVP correcto para cada cara
        Matrix4 mvp = Transform * view * Matrix4.Identity;

        GL.UniformMatrix4(_mvpLocation, false, ref mvp);

        GL.BindVertexArray(_vao);
        GL.DrawArrays(PrimitiveType.Triangles, 0, Vertices.Count / 6);
    }

    // Liberar memoria
    public void Dispose()
    {
        GL.DeleteBuffer(_vbo);
        GL.DeleteVertexArray(_vao);
        GL.DeleteProgram(_shaderProgram);
    }

    public void Trasladar(float x, float y, float z)
    {
        Transform *= Matrix4.CreateTranslation(x, y, z);
    }

    public void RotarX(float angleRadians)
    {
        Transform *= Matrix4.CreateRotationX(angleRadians);
    }

    public void RotarY(float angleRadians)
    {
        Transform *= Matrix4.CreateRotationY(angleRadians);
    }

    public void RotarZ(float angleRadians)
    {
        Transform *= Matrix4.CreateRotationZ(angleRadians);
    }

    public void Escalar(float factor)
    {
        Transform *= Matrix4.CreateScale(factor);
    }
}
