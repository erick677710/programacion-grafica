using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

public class ObjetoR : IDisposable
{
    public List<ObjetoR> partes { get; set; }
    public List<float> vertice { get; set; }
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }

    private int _vao;
    private int _vbo;
    private int _shaderProgram;
    private int _mvpLocation;
    //x y z tiene que ser una var
    //x y z tiene que ser una var
    //x y z tiene que ser una var
    public List<float> VerticeReubicado(float x, float y, float z, List<float> verticeOriginal)
    {
        List<float> resultado = new List<float>(verticeOriginal.Count);

        for (int i = 0; i < verticeOriginal.Count; i++)
        {
            int coord = i % 6; // 0=x, 1=y, 2=z, 3=r, 4=g, 5=b

            switch (coord)
            {
                case 0:
                    resultado.Add(verticeOriginal[i] + x); // x
                    break;
                case 1:
                    resultado.Add(verticeOriginal[i] + y); // y
                    break;
                case 2:
                    resultado.Add(verticeOriginal[i] + z); // z
                    break;
                default:
                    resultado.Add(verticeOriginal[i]);     // r, g, b se mantiene
                    break;
            }

        }
        return resultado;
    }
    // Constructor de las partes
    public ObjetoR(float x, float y, float z, List<float> vertices)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.vertice = VerticeReubicado(x, y, z, vertices);
        // Inicializar OpenGL para dibujar
        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);

        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertice.Count * sizeof(float), vertice.ToArray(), BufferUsageHint.StaticDraw);

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
        }";

        string fragmentShaderSource = @"
        #version 330 core
        in vec3 vColor;
        out vec4 FragColor;
        void main()
        {
            FragColor = vec4(vColor, 1.0);
        }";

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

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        _mvpLocation = GL.GetUniformLocation(_shaderProgram, "uMVP");
    }

    // Constructor del ObjetoR completo
    public ObjetoR(float x, float y, float z, List<float> vertices, List<ObjetoR> partes)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        // si partes = null usa una lsita vacia
        this.partes = partes ?? new List<ObjetoR>();

        // Vertices del ObjetoR + vertices de partes
        vertice = new List<float>(vertices);
        if (this.partes.Count > 0)
        {
            foreach (var parte in this.partes)
            {
                vertice.AddRange(parte.vertice);
            }
        }
        vertice = VerticeReubicado(x, y, z, vertice);
        // Inicializar OpenGL para dibujar
        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);

        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertice.Count * sizeof(float), vertice.ToArray(), BufferUsageHint.StaticDraw);

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
        }";

        string fragmentShaderSource = @"
        #version 330 core
        in vec3 vColor;
        out vec4 FragColor;
        void main()
        {
            FragColor = vec4(vColor, 1.0);
        }";

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

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        _mvpLocation = GL.GetUniformLocation(_shaderProgram, "uMVP");
    }

    public void Draw(Matrix4 mvp)
    {
        GL.UseProgram(_shaderProgram);
        GL.BindVertexArray(_vao);
        GL.UniformMatrix4(_mvpLocation, false, ref mvp);
        GL.DrawArrays(PrimitiveType.Triangles, 0, vertice.Count / 6);
    }

    public void Dispose()
    {
        GL.DeleteBuffer(_vbo);
        GL.DeleteVertexArray(_vao);
        GL.DeleteProgram(_shaderProgram);
    }
}