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
    public Matrix4 ModeloDeseoso { get; set; }

    public float rotx { get; set; } 
    public float roty { get; set; } 
    public float rotz { get; set; } 
    
    public float escx { get; set; } 
    public float escy { get; set; } 
    public float escz { get; set; } 

    
    
    
    //para borrar
    public serializadorCara GetSerializable()
    {
        return new serializadorCara
        {
            /*PX = X,
            PY = Y,
            PZ = Z,
            Vertices = new List<float>(Vertices) // copia de la lista*/
        };
    }
    //para borrar
    public void LoadFromSerializable(serializadorCara data)
    {
      /*  X = data.PZ;
        Y = data.PY;
        Z = data.PZ;
        Vertices = new List<float>(data.Vertices);
        ActualizarModelo();*/
    }
    //para borrar
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
    //para modificar
    public Cara()

    {
        
        
        Vertices = new List<float>();
        ModeloDeseoso = Matrix4.Identity * Matrix4.CreateTranslation(X, Y, Z);
        escx = 1;
        escy = 1;
        escz = 1;
        verticesPersonalizadosvoid(X, Y, Z);

    }

    // Constructor normal
    public Cara(float x, float y, float z, List<float> vertices)
    {
        
        X = x;
        Y = y;
        Z = z;
        escx = 1;
        escy = 1;
        escz = 1;
        Vertices = vertices;
        ModeloDeseoso = Matrix4.Identity * Matrix4.CreateTranslation(X, Y, Z);
        GetSerializable();
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
    public void Draw(Matrix4 ModeloP, Matrix4 VixPro)
    {
        GL.UseProgram(_shaderProgram);
        Matrix4 mvp = ModeloP * ModeloDeseoso * VixPro;
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
    public void ActualizarModelo()
    {
        // Centro de la cara (su propio origen local)
        Vector3 centro = new Vector3(X, Y, Z);

        // 1. Trasladar al origen local
        Matrix4 traslacionAlOrigen = Matrix4.CreateTranslation(-centro);
        //Matrix4 traslacionAlOrigen = Matrix4.Identity;

    
        // 2. Rotaciones o
        Matrix4 rotaciones =
            Matrix4.CreateRotationX(rotx) *
            Matrix4.CreateRotationY(roty) *
            Matrix4.CreateRotationZ(rotz);

        // 3. Escala
        Matrix4 escala = Matrix4.CreateScale(escx,escy,escz);

        // 4. Regresar al centro local
        Matrix4 traslacionDeVuelta = Matrix4.CreateTranslation(centro);

        // 5. Traslación global de la cara (su posición en la escena)
        Matrix4 traslacionFinal = Matrix4.CreateTranslation(X, Y, Z) ;
        traslacionFinal = traslacionFinal * Matrix4.CreateTranslation(X, Y, Z);
        
        //traslacionFinal = Matrix4.Identity;
        // Orden correcto: origen → rotación/escala → volver al centro → mover a posición global
        ModeloDeseoso = escala * rotaciones * traslacionDeVuelta  ;
    }

    public void Rotar(float x, float y, float z)
    {
        //_rotacion += new Vector3(x, y, z);
        rotx = rotx + x;
        roty = roty + y;
        rotz = rotz + z;
        ActualizarModelo();
    }

    public void Trasladar(float x, float y, float z)
    {
        //_posicion += new Vector3(x, y, z);
        X = X + x;
        Y = Y + y;
        Z = Z + z;
        ActualizarModelo();
    }
    public void Escalar(float x, float y, float z)
    {
        //_escala *= new Vector3(x, y, z); // escala no uniforme
        escx = escx + x;
        escy = escy + y;
        escz = escz + z;

        ActualizarModelo();
    }
    public void ReflejarX()
    {
        escx *= -1f;
        ActualizarModelo();
    }

    public void ReflejarY()
    {
        escy *= -1f;
        ActualizarModelo();
    }

    public void ReflejarZ()
    {
        escz *= -1f;
        ActualizarModelo();
    }
}
