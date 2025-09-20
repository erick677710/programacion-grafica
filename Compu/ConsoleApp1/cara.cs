using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
    [JsonIgnore]
    public Matrix4 ModeloDeseoso { get; set; } = Matrix4.Identity;
    [JsonIgnore]
    private Vector3 _rotacion = Vector3.Zero;
    [JsonIgnore]
    private Vector3 _posicion = Vector3.Zero;
    [JsonIgnore]
    private Vector3 _escala = new Vector3(1f, 1f, 1f);

    public Vector3 PosicionJson
    {
        get => _posicion;
        set { _posicion = value; ActualizarModelo(); }
    }

    public Vector3 RotacionJson
    {
        get => _rotacion;
        set { _rotacion = value; ActualizarModelo(); }
    }

    public float EscalaJson { get; set; } = 1f;


    public serializadorCara GetSerializable()
    {
        return new serializadorCara
        {
            PosX = _posicion.X,
            PosY = _posicion.Y,
            PosZ = _posicion.Z,
            RotX = _rotacion.X,
            RotY = _rotacion.Y,
            RotZ = _rotacion.Z,
            Vertices = new List<float>(Vertices) // copia de la lista
        };
    }

    public void LoadFromSerializable(serializadorCara data)
    {
        _posicion = new Vector3(X,Y,Z);
        _rotacion = new Vector3(data.RotX, data.RotY, data.RotZ);
        Vertices = new List<float>(data.Vertices);
        ActualizarModelo();
    }

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
        X = _posicion.X;
        Y = _posicion.Y;
        Z = _posicion.Z;
        Vertices = new List<float>();
        ModeloDeseoso *= Matrix4.CreateTranslation(_posicion.X, _posicion.Y, _posicion.Z);

        _posicion = new Vector3(_posicion.X, _posicion.Y, _posicion.Z);
        verticesPersonalizadosvoid(_posicion.X, _posicion.Y, _posicion.Z);

    }

    // Constructor normal
    public Cara(float x, float y, float z, List<float> vertices)
    {
        X = x;
        Y = y;
        Z = z;
        Vertices = vertices;
        ModeloDeseoso *= Matrix4.CreateTranslation(x,y,z);
        
        _posicion = new Vector3(x, y, z);
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
    public void Draw(Matrix4 mvpE)
    {
        GL.UseProgram(_shaderProgram);

        // MVP acumulado = transformación local de la cara * transformaciones superiores
        Matrix4 mvp = ModeloDeseoso * mvpE;

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
    private void ActualizarModelo()
    {
        // Centro de la cara (su propio origen local)
        Vector3 centro = new Vector3(X, Y, Z);

        // 1. Trasladar al origen local
        Matrix4 traslacionAlOrigen = Matrix4.CreateTranslation(-centro);

        // 2. Rotaciones
        Matrix4 rotaciones =
            Matrix4.CreateRotationX(_rotacion.X) *
            Matrix4.CreateRotationY(_rotacion.Y) *
            Matrix4.CreateRotationZ(_rotacion.Z);

        // 3. Escala
        Matrix4 escala = Matrix4.CreateScale(_escala);

        // 4. Regresar al centro local
        Matrix4 traslacionDeVuelta = Matrix4.CreateTranslation(centro);

        // 5. Traslación global de la cara (su posición en la escena)
        Matrix4 traslacionFinal = Matrix4.CreateTranslation(_posicion);

        // Orden correcto: origen → rotación/escala → volver al centro → mover a posición global
        ModeloDeseoso = traslacionAlOrigen * rotaciones * escala * traslacionDeVuelta * traslacionFinal;
    }

    public void Rotar(float x, float y, float z)
    {
        _rotacion += new Vector3(x, y, z);
        ActualizarModelo();
    }

    public void Trasladar(float x, float y, float z)
    {
        _posicion += new Vector3(x, y, z);
        ActualizarModelo();
    }
    public void Escalar(float x, float y, float z)
    {
        //_escala *= new Vector3(x, y, z); // escala no uniforme
        _escala.X = _escala.X + x;
        _escala.Y = _escala.Y + y;
        _escala.Z = _escala.Z + z;

        ActualizarModelo();
    }
    public void ReflejarX()
    {
        _escala.X *= -1f;
        ActualizarModelo();
    }

    public void ReflejarY()
    {
        _escala.Y *= -1f;
        ActualizarModelo();
    }

    public void ReflejarZ()
    {
        _escala.Z *= -1f;
        ActualizarModelo();
    }
}
