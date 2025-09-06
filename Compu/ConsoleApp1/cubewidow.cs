using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

public class CubeWindow : GameWindow
{
    private Objeto _teclado;
    private Objeto _monitor;
    private Objeto _cpu;
    private Objeto _pc;
    private Matrix4 _projection;
    private Matrix4 _view;
    private float _rotation = 0f;

    public CubeWindow(GameWindowSettings gwSettings, NativeWindowSettings nwSettings)
        : base(gwSettings, nwSettings) { }

    // Ejemplo de vértices
    private List<float> cubo = new List<float> {
    // Frente (rojo)
    -0.1f,-0.1f,0.1f,0.6f,0f,0f,
     0.1f,-0.1f,0.1f,0.6f,0f,0f,
     0.1f,0.1f,0.1f,0.6f,0f,0f,
     0.1f,0.1f,0.1f,0.6f,0f,0f,
    -0.1f,0.1f,0.1f,0.6f,0f,0f,
    -0.1f,-0.1f,0.1f,0.6f,0f,0f,

    // Atrás (verde)
    -0.1f,-0.1f,-0.1f,0f,0.6f,0f,
     0.1f,-0.1f,-0.1f,0f,0.6f,0f,
     0.1f,0.1f,-0.1f,0f,0.6f,0f,
     0.1f,0.1f,-0.1f,0f,0.6f,0f,
    -0.1f,0.1f,-0.1f,0f,0.6f,0f,
    -0.1f,-0.1f,-0.1f,0f,0.6f,0f,

    // Izquierda (azul)
    -0.1f,0.1f,0.1f,0f,0f,0.6f,
    -0.1f,0.1f,-0.1f,0f,0f,0.6f,
    -0.1f,-0.1f,-0.1f,0f,0f,0.6f,
    -0.1f,-0.1f,-0.1f,0f,0f,0.6f,
    -0.1f,-0.1f,0.1f,0f,0f,0.6f,
    -0.1f,0.1f,0.1f,0f,0f,0.6f,

    // Derecha (amarillo)
     0.1f,0.1f,0.1f,0.6f,0.6f,0f,
     0.1f,0.1f,-0.1f,0.6f,0.6f,0f,
     0.1f,-0.1f,-0.1f,0.6f,0.6f,0f,
     0.1f,-0.1f,-0.1f,0.6f,0.6f,0f,
     0.1f,-0.1f,0.1f,0.6f,0.6f,0f,
     0.1f,0.1f,0.1f,0.6f,0.6f,0f,

    // Arriba (magenta)
    -0.1f,0.1f,-0.1f,0.6f,0f,0.6f,
     0.1f,0.1f,-0.1f,0.6f,0f,0.6f,
     0.1f,0.1f,0.1f,0.6f,0f,0.6f,
     0.1f,0.1f,0.1f,0.6f,0f,0.6f,
    -0.1f,0.1f,0.1f,0.6f,0f,0.6f,
    -0.1f,0.1f,-0.1f,0.6f,0f,0.6f,

    // Abajo (cyan)
    -0.1f,-0.1f,-0.1f,0f,0.6f,0.6f,
     0.1f,-0.1f,-0.1f,0f,0.6f,0.6f,
     0.1f,-0.1f,0.1f,0f,0.6f,0.6f,
     0.1f,-0.1f,0.1f,0f,0.6f,0.6f,
    -0.1f,-0.1f,0.1f,0f,0.6f,0.6f,
    -0.1f,-0.1f,-0.1f,0f,0.6f,0.6f
};

    private List<float> monitor = new List<float> {
    // Frente
    -0.19f,-0.19f,0.2f,0f,1f,1f,
     0.19f,-0.19f,0.2f,0f,1f,1f,
     0.19f,0.19f,0.2f,0f,1f,1f,
     0.19f,0.19f,0.2f,0f,1f,1f,
    -0.19f,0.19f,0.2f,0f,1f,1f,
    -0.19f,-0.19f,0.2f,0f,1f,1f,

    // Atrás
    -0.2f,-0.2f,-0.2f,0.753f,0.753f,0.753f,
     0.2f,-0.2f,-0.2f,0.753f,0.753f,0.753f,
     0.2f,0.2f,-0.2f,0.753f,0.753f,0.753f,
     0.2f,0.2f,-0.2f,0.753f,0.753f,0.753f,
    -0.2f,0.2f,-0.2f,0.753f,0.753f,0.753f,
    -0.2f,-0.2f,-0.2f,0.753f,0.753f,0.753f,

    // Izquierda
    -0.2f,0.2f,0.2f,0.753f,0.753f,0.753f,
    -0.2f,0.2f,-0.2f,0.753f,0.753f,0.753f,
    -0.2f,-0.2f,-0.2f,0.753f,0.753f,0.753f,
    -0.2f,-0.2f,-0.2f,0.753f,0.753f,0.753f,
    -0.2f,-0.2f,0.2f,0.753f,0.753f,0.753f,
    -0.2f,0.2f,0.2f,0.753f,0.753f,0.753f,

    // Derecha
     0.2f,0.2f,0.2f,0.753f,0.753f,0.753f,
     0.2f,0.2f,-0.2f,0.753f,0.753f,0.753f,
     0.2f,-0.2f,-0.2f,0.753f,0.753f,0.753f,
     0.2f,-0.2f,-0.2f,0.753f,0.753f,0.753f,
     0.2f,-0.2f,0.2f,0.753f,0.753f,0.753f,
     0.2f,0.2f,0.2f,0.753f,0.753f,0.753f,

    // Arriba
    -0.2f,0.2f,-0.2f,0.753f,0.753f,0.753f,
     0.2f,0.2f,-0.2f,0.753f,0.753f,0.753f,
     0.2f,0.2f,0.2f,0.753f,0.753f,0.753f,
     0.2f,0.2f,0.2f,0.753f,0.753f,0.753f,
    -0.2f,0.2f,0.2f,0.753f,0.753f,0.753f,
    -0.2f,0.2f,-0.2f,0.753f,0.753f,0.753f,

    // Abajo
    -0.2f,-0.2f,-0.2f,0.753f,0.753f,0.753f,
     0.2f,-0.2f,-0.2f,0.753f,0.753f,0.753f,
     0.2f,-0.2f,0.2f,0.753f,0.753f,0.753f,
     0.2f,-0.2f,0.2f,0.753f,0.753f,0.753f,
    -0.2f,-0.2f,0.2f,0.753f,0.753f,0.753f,
    -0.2f,-0.2f,-0.2f,0.753f,0.753f,0.753f
};


    private List<float> teclado = new List<float> {
    // Frente
    -0.2f,-0.25f,0.2f,0.753f,0.753f,0.753f,
     0.2f,-0.25f,0.2f,0.753f,0.753f,0.753f,
     0.2f,-0.2f,0.2f,0.753f,0.753f,0.753f,
     0.2f,-0.2f,0.2f,0.753f,0.753f,0.753f,
    -0.2f,-0.2f,0.2f,0.753f,0.753f,0.753f,
    -0.2f,-0.25f,0.2f,0.753f,0.753f,0.753f,

    // Atrás
    -0.2f,-0.25f,0.2f,0.753f,0.753f,0.753f,
     0.2f,-0.25f,0.2f,0.753f,0.753f,0.753f,
     0.2f,-0.2f,0.2f,0.753f,0.753f,0.753f,
     0.2f,-0.2f,0.2f,0.753f,0.753f,0.753f,
    -0.2f,-0.2f,0.2f,0.753f,0.753f,0.753f,
    -0.2f,-0.25f,0.2f,0.753f,0.753f,0.753f,

    // Izquierda
    -0.2f,-0.2f,0.4f,0.753f,0.753f,0.753f,
    -0.2f,-0.2f,0.2f,0.753f,0.753f,0.753f,
    -0.2f,-0.25f,0.2f,0.753f,0.753f,0.753f,
    -0.2f,-0.25f,0.2f,0.753f,0.753f,0.753f,
    -0.2f,-0.25f,0.4f,0.753f,0.753f,0.753f,
    -0.2f,-0.2f,0.4f,0.753f,0.753f,0.753f,

    // Derecha
     0.2f,-0.2f,0.4f,0.753f,0.753f,0.753f,
     0.2f,-0.2f,0.2f,0.753f,0.753f,0.753f,
     0.2f,-0.25f,0.2f,0.753f,0.753f,0.753f,
     0.2f,-0.25f,0.2f,0.753f,0.753f,0.753f,
     0.2f,-0.25f,0.4f,0.753f,0.753f,0.753f,
     0.2f,-0.2f,0.4f,0.753f,0.753f,0.753f,

    // Arriba
    -0.2f,-0.2f,0.2f,0.753f,0.753f,0.753f,
     0.2f,-0.2f,0.2f,0.753f,0.753f,0.753f,
     0.2f,-0.2f,0.4f,0.753f,0.753f,0.753f,
     0.2f,-0.2f,0.4f,0.753f,0.753f,0.753f,
    -0.2f,-0.2f,0.4f,0.753f,0.753f,0.753f,
    -0.2f,-0.2f,0.2f,0.753f,0.753f,0.753f,

    // Abajo
    -0.2f,-0.25f,0.2f,0.753f,0.753f,0.753f,
     0.2f,-0.25f,0.2f,0.753f,0.753f,0.753f,
     0.2f,-0.25f,0.4f,0.753f,0.753f,0.753f,
     0.2f,-0.25f,0.4f,0.753f,0.753f,0.753f,
    -0.2f,-0.25f,0.4f,0.753f,0.753f,0.753f,
    -0.2f,-0.25f,0.2f,0.753f,0.753f,0.753f
};


    private List<float> cpu = new List<float> {
    // Frente
    -0.0f,-1f,-0.3f,0.753f,0.753f,0.753f,
     0.3f,-1f,-0.3f,0.753f,0.753f,0.753f,
     0.3f,-0.5f,-0.3f,0.753f,0.753f,0.753f,
     0.3f,-0.5f,-0.3f,0.753f,0.753f,0.753f,
    -0.0f,-0.5f,-0.3f,0.753f,0.753f,0.753f,
    -0.0f,-1f,-0.3f,0.753f,0.753f,0.753f,

    // Atrás
    -0.0f,-1f,-0.3f,0.753f,0.753f,0.753f,
     0.3f,-1f,-0.3f,0.753f,0.753f,0.753f,
     0.3f,-0.5f,-0.3f,0.753f,0.753f,0.753f,
     0.3f,-0.5f,-0.3f,0.753f,0.753f,0.753f,
    -0.0f,-0.5f,-0.3f,0.753f,0.753f,0.753f,
    -0.0f,-1f,-0.3f,0.753f,0.753f,0.753f,

    // Izquierda
    -0.0f,-0.5f,0.3f,0.753f,0.753f,0.753f,
    -0.0f,-0.5f,-0.3f,0.753f,0.753f,0.753f,
    -0.0f,-1f,-0.3f,0.753f,0.753f,0.753f,
    -0.0f,-1f,-0.3f,0.753f,0.753f,0.753f,
    -0.0f,-1f,0.3f,0.753f,0.753f,0.753f,
    -0.0f,-0.5f,0.3f,0.753f,0.753f,0.753f,

    // Derecha
     0.3f,-0.5f,0.3f,0.753f,0.753f,0.753f,
     0.3f,-0.5f,-0.3f,0.753f,0.753f,0.753f,
     0.3f,-1f,-0.3f,0.753f,0.753f,0.753f,
     0.3f,-1f,-0.3f,0.753f,0.753f,0.753f,
     0.3f,-1f,0.3f,0.753f,0.753f,0.753f,
     0.3f,-0.5f,0.3f,0.753f,0.753f,0.753f,

    // Arriba
    -0.0f,-0.5f,-0.3f,0.753f,0.753f,0.753f,
     0.3f,-0.5f,-0.3f,0.753f,0.753f,0.753f,
     0.3f,-0.5f,0.3f,0.753f,0.753f,0.753f,
     0.3f,-0.5f,0.3f,0.753f,0.753f,0.753f,
    -0.0f,-0.5f,0.3f,0.753f,0.753f,0.753f,
    -0.0f,-0.5f,-0.3f,0.753f,0.753f,0.753f,

    // Abajo
    -0.0f,-1f,-0.3f,0.753f,0.753f,0.753f,
     0.3f,-1f,-0.3f,0.753f,0.753f,0.753f,
     0.3f,-1f,0.3f,0.753f,0.753f,0.753f,
     0.3f,-1f,0.3f,0.753f,0.753f,0.753f,
    -0.0f,-1f,0.3f,0.753f,0.753f,0.753f,
    -0.0f,-1f,-0.3f,0.753f,0.753f,0.753f
};



    protected override void OnLoad()
    {
        base.OnLoad();
        GL.Enable(EnableCap.DepthTest);

        // Crear objetos
        _teclado = new Objeto(0f, 0f, 0f, teclado);
        _monitor = new Objeto(0f, 0.5f, 0f,monitor);
        _cpu = new Objeto(0f, 0f, 0f, cpu);
        List<Objeto> partes = new List<Objeto>();
        partes.Add(_teclado);
        partes.Add(_monitor);
        partes.Add(_cpu);
        _pc = new Objeto(0f, 0f, 0f, cpu, partes);

        _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60f), Size.X / (float)Size.Y, 0.1f, 100f);
        _view = Matrix4.LookAt(new Vector3(1.5f, 1.5f, 3f), Vector3.Zero, Vector3.UnitY);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        var input = KeyboardState;

        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Right))
            _rotation += (float)args.Time;
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Left))
            _rotation -= (float)args.Time;
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        Matrix4 model = Matrix4.CreateRotationY(_rotation) * Matrix4.CreateRotationX(_rotation);
        Matrix4 mvp = Matrix4.CreateTranslation(0f, 0f, 0f) * model * _view * _projection;

        //_monitor.Draw(mvp); // incluye teclado
        _pc.Draw(mvp);

        SwapBuffers();
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        _cpu.Dispose();
        _monitor.Dispose();
        _teclado.Dispose();
        _pc.Dispose();
    }
}
