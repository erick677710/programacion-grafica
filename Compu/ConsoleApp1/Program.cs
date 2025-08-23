using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

class Cube
{
    private int _vao;
    private int _vbo;
    private int _shaderProgram;
    private int _mvpLocation;

    public Cube()
    {
        // Vértices del cubo (posX, posY, posZ, r, g, b)
        float[] vertices = {
            // Frente (rojo)
            -0.5f, -0.5f,  0.5f, 1f, 0f, 0f,
             0.5f, -0.5f,  0.5f, 1f, 0f, 0f,
             0.5f,  0.5f,  0.5f, 1f, 0f, 0f,
             0.5f,  0.5f,  0.5f, 1f, 0f, 0f,
            -0.5f,  0.5f,  0.5f, 1f, 0f, 0f,
            -0.5f, -0.5f,  0.5f, 1f, 0f, 0f,

            // Atrás (verde)
            -0.5f, -0.5f, -0.5f, 0f, 1f, 0f,
             0.5f, -0.5f, -0.5f, 0f, 1f, 0f,
             0.5f,  0.5f, -0.5f, 0f, 1f, 0f,
             0.5f,  0.5f, -0.5f, 0f, 1f, 0f,
            -0.5f,  0.5f, -0.5f, 0f, 1f, 0f,
            -0.5f, -0.5f, -0.5f, 0f, 1f, 0f,

            // Izquierda (azul)
            -0.5f,  0.5f,  0.5f, 0f, 0f, 1f,
            -0.5f,  0.5f, -0.5f, 0f, 0f, 1f,
            -0.5f, -0.5f, -0.5f, 0f, 0f, 1f,
            -0.5f, -0.5f, -0.5f, 0f, 0f, 1f,
            -0.5f, -0.5f,  0.5f, 0f, 0f, 1f,
            -0.5f,  0.5f,  0.5f, 0f, 0f, 1f,

            // Derecha (amarillo)
             0.5f,  0.5f,  0.5f, 1f, 1f, 0f,
             0.5f,  0.5f, -0.5f, 1f, 1f, 0f,
             0.5f, -0.5f, -0.5f, 1f, 1f, 0f,
             0.5f, -0.5f, -0.5f, 1f, 1f, 0f,
             0.5f, -0.5f,  0.5f, 1f, 1f, 0f,
             0.5f,  0.5f,  0.5f, 1f, 1f, 0f,

            // Arriba (magenta)
            -0.5f,  0.5f, -0.5f, 1f, 0f, 1f,
             0.5f,  0.5f, -0.5f, 1f, 0f, 1f,
             0.5f,  0.5f,  0.5f, 1f, 0f, 1f,
             0.5f,  0.5f,  0.5f, 1f, 0f, 1f,
            -0.5f,  0.5f,  0.5f, 1f, 0f, 1f,
            -0.5f,  0.5f, -0.5f, 1f, 0f, 1f,

            // Abajo (cyan)
            -0.5f, -0.5f, -0.5f, 0f, 1f, 1f,
             0.5f, -0.5f, -0.5f, 0f, 1f, 1f,
             0.5f, -0.5f,  0.5f, 0f, 1f, 1f,
             0.5f, -0.5f,  0.5f, 0f, 1f, 1f,
            -0.5f, -0.5f,  0.5f, 0f, 1f, 1f,
            -0.5f, -0.5f, -0.5f, 0f, 1f, 1f
        };

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

class Monitor
{
    private int _vao;
    private int _vbo;
    private int _shaderProgram;
    private int _mvpLocation;

    public Monitor()
    {
        // Vértices del cubo (posX, posY, posZ, r, g, b)
        float[] vertices = {
            // Frente
            -0.19f, -0.19f,  0.2f, 0f, 1f, 1f,
             0.19f, -0.19f,  0.2f, 0f, 1f, 1f,
             0.19f,  0.19f,  0.2f, 0f, 1f, 1f,
             0.19f,  0.19f,  0.2f, 0f, 1f, 1f,
            -0.19f,  0.19f,  0.2f, 0f, 1f, 1f,
            -0.19f, -0.19f,  0.2f, 0f, 1f, 1f,

            // Atrás
            -0.2f, -0.2f, -0.2f, 0.753f, 0.753f, 0.753f,
             0.2f, -0.2f, -0.2f, 0.753f, 0.753f, 0.753f,
             0.2f,  0.2f, -0.2f, 0.753f, 0.753f, 0.753f,
             0.2f,  0.2f, -0.2f, 0.753f, 0.753f, 0.753f,
            -0.2f,  0.2f, -0.2f, 0.753f, 0.753f, 0.753f,
            -0.2f, -0.2f, -0.2f, 0.753f, 0.753f, 0.753f,

            // Izquierda
            -0.2f,  0.2f,  0.2f, 0.753f, 0.753f, 0.753f,
            -0.2f,  0.2f, -0.2f, 0.753f, 0.753f, 0.753f,
            -0.2f, -0.2f, -0.2f, 0.753f, 0.753f, 0.753f,
            -0.2f, -0.2f, -0.2f, 0.753f, 0.753f, 0.753f,
            -0.2f, -0.2f,  0.2f, 0.753f, 0.753f, 0.753f,
            -0.2f,  0.2f,  0.2f, 0.753f, 0.753f, 0.753f,

            // Derecha
             0.2f,  0.2f,  0.2f, 0.753f, 0.753f, 0.753f,
             0.2f,  0.2f, -0.2f, 0.753f, 0.753f, 0.753f,
             0.2f, -0.2f, -0.2f, 0.753f, 0.753f, 0.753f,
             0.2f, -0.2f, -0.2f, 0.753f, 0.753f, 0.753f,
             0.2f, -0.2f,  0.2f, 0.753f, 0.753f, 0.753f,
             0.2f,  0.2f,  0.2f, 0.753f, 0.753f, 0.753f,

            // Arriba
            -0.2f,  0.2f, -0.2f, 0.753f, 0.753f, 0.753f,
             0.2f,  0.2f, -0.2f, 0.753f, 0.753f, 0.753f,
             0.2f,  0.2f,  0.2f, 0.753f, 0.753f, 0.753f,
             0.2f,  0.2f,  0.2f, 0.753f, 0.753f, 0.753f,
            -0.2f,  0.2f,  0.2f, 0.753f, 0.753f, 0.753f,
            -0.2f,  0.2f, -0.2f, 0.753f, 0.753f, 0.753f,

            // Abajo
            -0.2f, -0.2f, -0.2f, 0.753f, 0.753f, 0.753f,
             0.2f, -0.2f, -0.2f, 0.753f, 0.753f, 0.753f,
             0.2f, -0.2f,  0.2f, 0.753f, 0.753f, 0.753f,
             0.2f, -0.2f,  0.2f, 0.753f, 0.753f, 0.753f,
            -0.2f, -0.2f,  0.2f, 0.753f, 0.753f, 0.753f,
            -0.2f, -0.2f, -0.2f, 0.753f, 0.753f, 0.753f

        };

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


class Teclado
{
    private int _vao;
    private int _vbo;
    private int _shaderProgram;
    private int _mvpLocation;

    public Teclado()
    {
        // Vértices del cubo (posX, posY, posZ, r, g, b)
        float[] vertices = {
             // Frente
            -0.2f, -0.25f, 0.2f, 0.753f, 0.753f, 0.753f,
             0.2f, -0.25f, 0.2f, 0.753f, 0.753f, 0.753f,
             0.2f,  -0.2f, 0.2f, 0.753f, 0.753f, 0.753f,
             0.2f,  -0.2f, 0.2f, 0.753f, 0.753f, 0.753f,
            -0.2f,  -0.2f, 0.2f, 0.753f, 0.753f, 0.753f,
            -0.2f, -0.25f, 0.2f, 0.753f, 0.753f, 0.753f,

            // Atrás
            -0.2f, -0.25f, 0.2f, 0.753f, 0.753f, 0.753f,
             0.2f, -0.25f, 0.2f, 0.753f, 0.753f, 0.753f,
             0.2f,  -0.2f, 0.2f, 0.753f, 0.753f, 0.753f,
             0.2f,  -0.2f, 0.2f, 0.753f, 0.753f, 0.753f,
            -0.2f,  -0.2f, 0.2f, 0.753f, 0.753f, 0.753f,
            -0.2f, -0.25f, 0.2f, 0.753f, 0.753f, 0.753f,

            // Izquierda
            -0.2f,  -0.2f,  0.4f, 0.753f, 0.753f, 0.753f,
            -0.2f,  -0.2f, 0.2f, 0.753f, 0.753f, 0.753f,
            -0.2f, -0.25f, 0.2f, 0.753f, 0.753f, 0.753f,
            -0.2f, -0.25f, 0.2f, 0.753f, 0.753f, 0.753f,
            -0.2f, -0.25f,  0.4f, 0.753f, 0.753f, 0.753f,
            -0.2f,  -0.2f,  0.4f, 0.753f, 0.753f, 0.753f,

            // Derecha
             0.2f,  -0.2f,  0.4f, 0.753f, 0.753f, 0.753f,
             0.2f,  -0.2f, 0.2f, 0.753f, 0.753f, 0.753f,
             0.2f, -0.25f, 0.2f, 0.753f, 0.753f, 0.753f,
             0.2f, -0.25f, 0.2f, 0.753f, 0.753f, 0.753f,
             0.2f, -0.25f,  0.4f, 0.753f, 0.753f, 0.753f,
             0.2f,  -0.2f,  0.4f, 0.753f, 0.753f, 0.753f,

            // Arriba
            -0.2f,  -0.2f, 0.2f, 0.753f, 0.753f, 0.753f,
             0.2f,  -0.2f, 0.2f, 0.753f, 0.753f, 0.753f,
             0.2f,  -0.2f,  0.4f, 0.753f, 0.753f, 0.753f,
             0.2f,  -0.2f,  0.4f, 0.753f, 0.753f, 0.753f,
            -0.2f,  -0.2f,  0.4f, 0.753f, 0.753f, 0.753f,
            -0.2f,  -0.2f, 0.2f, 0.753f, 0.753f, 0.753f,

            // Abajo
            -0.2f, -0.25f, 0.2f, 0.753f, 0.753f, 0.753f,
             0.2f, -0.25f, 0.2f, 0.753f, 0.753f, 0.753f,
             0.2f, -0.25f,  0.4f, 0.753f, 0.753f, 0.753f,
             0.2f, -0.25f,  0.4f, 0.753f, 0.753f, 0.753f,
            -0.2f, -0.25f,  0.4f, 0.753f, 0.753f, 0.753f,
            -0.2f, -0.25f, 0.2f, 0.753f, 0.753f, 0.753f

        };

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


class Cpu
{
    private int _vao;
    private int _vbo;
    private int _shaderProgram;
    private int _mvpLocation;

    public Cpu()
    {
        // Vértices del cubo (posX, posY, posZ, r, g, b)
        float[] vertices = {
            // Frente
            -0.0f, -1f, -0.3f, 0.753f, 0.753f, 0.753f,
             0.3f, -1f, -0.3f, 0.753f, 0.753f, 0.753f,
             0.3f,  -0.5f, -0.3f, 0.753f, 0.753f, 0.753f,
             0.3f,  -0.5f, -0.3f, 0.753f, 0.753f, 0.753f,
            -0.0f,  -0.5f, -0.3f, 0.753f, 0.753f, 0.753f,
            -0.0f, -1f, -0.3f, 0.753f, 0.753f, 0.753f,

            // Atrás
            -0.0f, -1f, -0.3f, 0.753f, 0.753f, 0.753f,
             0.3f, -1f, -0.3f, 0.753f, 0.753f, 0.753f,
             0.3f,  -0.5f, -0.3f, 0.753f, 0.753f, 0.753f,
             0.3f,  -0.5f, -0.3f, 0.753f, 0.753f, 0.753f,
            -0.0f,  -0.5f, -0.3f, 0.753f, 0.753f, 0.753f,
            -0.0f, -1f, -0.3f, 0.753f, 0.753f, 0.753f,

            // Izquierda
            -0.0f,  -0.5f,  0.3f, 0.753f, 0.753f, 0.753f,
            -0.0f,  -0.5f, -0.3f, 0.753f, 0.753f, 0.753f,
            -0.0f, -1f, -0.3f, 0.753f, 0.753f, 0.753f,
            -0.0f, -1f, -0.3f, 0.753f, 0.753f, 0.753f,
            -0.0f, -1f,  0.3f, 0.753f, 0.753f, 0.753f,
            -0.0f,  -0.5f,  0.3f, 0.753f, 0.753f, 0.753f,

            // Derecha
             0.3f,  -0.5f,  0.3f, 0.753f, 0.753f, 0.753f,
             0.3f,  -0.5f, -0.3f, 0.753f, 0.753f, 0.753f,
             0.3f, -1f, -0.3f, 0.753f, 0.753f, 0.753f,
             0.3f, -1f, -0.3f, 0.753f, 0.753f, 0.753f,
             0.3f, -1f,  0.3f, 0.753f, 0.753f, 0.753f,
             0.3f,  -0.5f,  0.3f, 0.753f, 0.753f, 0.753f,

            // Arriba
            -0.0f,  -0.5f, -0.3f, 0.753f, 0.753f, 0.753f,
             0.3f,  -0.5f, -0.3f, 0.753f, 0.753f, 0.753f,
             0.3f,  -0.5f,  0.3f, 0.753f, 0.753f, 0.753f,
             0.3f,  -0.5f,  0.3f, 0.753f, 0.753f, 0.753f,
            -0.0f,  -0.5f,  0.3f, 0.753f, 0.753f, 0.753f,
            -0.0f,  -0.5f, -0.3f, 0.753f, 0.753f, 0.753f,

            // Abajo
            -0.0f, -1f, -0.3f, 0.753f, 0.753f, 0.753f,
             0.3f, -1f, -0.3f, 0.753f, 0.753f, 0.753f,
             0.3f, -1f,  0.3f, 0.753f, 0.753f, 0.753f,
             0.3f, -1f,  0.3f, 0.753f, 0.753f, 0.753f,
            -0.0f, -1f,  0.3f, 0.753f, 0.753f, 0.753f,
            -0.0f, -1f, -0.3f, 0.753f, 0.753f, 0.753f

        };

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


class CubeWindow : GameWindow
{
    //private Cube _cube;
    private Teclado _teclado;
    private Monitor _monitor;
    private Cpu _cpu;
    private Matrix4 _projection;
    private Matrix4 _view;
    private float _rotation = 0f;

    public CubeWindow(GameWindowSettings gwSettings, NativeWindowSettings nwSettings)
        : base(gwSettings, nwSettings) { }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.Enable(EnableCap.DepthTest);
        //_cube = new Cube();
        _teclado = new Teclado();
        _monitor = new Monitor();
        _cpu = new Cpu();
        _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60f), Size.X / (float)Size.Y, 0.1f, 100f);
        _view = Matrix4.LookAt(new Vector3(1.5f, 1.5f, 3f), Vector3.Zero, Vector3.UnitY);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        var input = KeyboardState; // Obtiene el estado del teclado

        // Rotación controlada por teclas
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Right))
            _rotation += (float)args.Time;   // Rota a la derecha
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Left))
            _rotation -= (float)args.Time;   // Rota a la izquierda
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Up))
            _rotation += (float)args.Time;  // Rota hacia arriba
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Down))
            _rotation -= (float)args.Time;  // Rota hacia abajo

        // Matriz de modelo usando las rotaciones controladas
        Matrix4 model = Matrix4.CreateRotationY(_rotation) * Matrix4.CreateRotationX(_rotation);
        Matrix4 mvp = model * _view * _projection;

        // Dibujar objetos
        _monitor.Draw(mvp);
        _cpu.Draw(mvp);
        _teclado.Draw(mvp);
        //_cube.Draw(mvp);

        SwapBuffers();
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        _cpu.Dispose();
        _monitor.Dispose();
        _teclado.Dispose();
        //_cube.Dispose();
    }
}

class Program
{
    static void Main()
    {
        var gwSettings = GameWindowSettings.Default;
        var nwSettings = new NativeWindowSettings()
        {
            Size = new Vector2i(800, 600),
            Title = "Cubo 3D OpenTK (Encapsulado)"
        };
        using var window = new CubeWindow(gwSettings, nwSettings);
        window.Run();
    }
}
