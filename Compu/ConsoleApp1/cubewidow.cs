using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Text.Json;
using System.IO;
using OpenTK.Compute.OpenCL;

public class CubeWindow : GameWindow
{
    /*private Objeto _teclado;
    private Objeto _teclado2;
    private Objeto _monitor;
    private Objeto _cpu;
    private Objeto _pc;*/
    private Cara _cara;
    private Cara _cara1;
    private Cara _cara2;
    private Cara _cara3;
    private Cara _cara4;
    private Matrix4 _projection;
    private Matrix4 _view;
    private float _rotationX = 0f;
    private float _rotationY = 0f;
    private float _rotationZ = 0f;
    private float _posX = 0f, _posY = 0f, _posZ = 0f;
    private float _scale = 1f;
    private bool _reflectionX = false;

    private Parte parteObj;
    private Parte parteObj1;
    private Parte pala;
    private Objeto prueba;
    private Objeto objpala;

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
    private List<float> cara = new List<float> { -0.1f, -0.1f, 0.1f, 0.6f, 0f, 0f,
     0.1f, -0.1f, 0.1f, 0.6f, 0f, 0f,
     0.1f, 0.1f, 0.1f, 0.6f, 0f, 0f,
     0.1f, 0.1f, 0.1f, 0.6f, 0f, 0f,
    -0.1f, 0.1f, 0.1f, 0.6f, 0f, 0f,
    -0.1f, -0.1f, 0.1f, 0.6f, 0f, 0f };


    protected override void OnLoad()
    {
        base.OnLoad();
        GL.Enable(EnableCap.DepthTest);
        List<Cara> objeto1 = new List<Cara>();
        List<Cara> objeto2 = new List<Cara>();
        List<Parte> ListaParte = new List<Parte>();
        _cara = new Cara(-0.5f,0f,0.0f, cubo);
        _cara1 = new Cara(0.5f, -0.5f, 0.5f, monitor);
        _cara2 = new Cara();


        _cara3 = new Cara(0.5f, 0.5f, 0.5f, teclado);
        _cara4 = new Cara(0f, 0f, 0f, cpu);


        objeto1.Add(_cara);
        objeto1.Add(_cara1);
        objeto1.Add(_cara2);

        objeto2.Add(_cara4);
        objeto2.Add(_cara3);

        parteObj = new Parte(0f, 0f, 0.5f, objeto1);
        parteObj1 = new Parte(0f, 0f, 0f, objeto2);
        //pala = new Parte();
        pala = parteObj;




        ListaParte.Add(parteObj);
        //ListaParte.Add(parteObj1);
        prueba = new Objeto(0f,2f,0f, ListaParte);
        // Crear objetos
        //string json = JsonSerializer.Serialize(_cara, new JsonSerializerOptions { WriteIndented = true });

        //File.WriteAllText(@"C:\programacion grafica\programacion-grafica\Compu\monitor.json", json);

        // Leer desde archivo
        //string json = File.ReadAllText(@"C:\programacion grafica\programacion-grafica\Compu\monitor.json");
        //Objeto desdeArchivo = JsonSerializer.Deserialize<Objeto>(json);




        /*string json = JsonSerializer.Serialize(_cara1);
        File.WriteAllText(@"C:\programacion grafica\programacion-grafica\Compu\cara.json", json);
        string jsonleido = File.ReadAllText(@"C:\programacion grafica\programacion-grafica\Compu\cara.json");

        _cara2 = JsonSerializer.Deserialize<Cara>(jsonleido);
        Console.WriteLine(json);*/


        //string json = JsonSerializer.Serialize(prueba);
        //File.WriteAllText(@"C:\programacion grafica\programacion-grafica\Compu\obj.json", json);
        string jsonleido = File.ReadAllText(@"C:\programacion grafica\programacion-grafica\Compu\obj.json");

        objpala = JsonSerializer.Deserialize<Objeto>(jsonleido);
        //Console.WriteLine(json);


        _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60f), Size.X / (float)Size.Y, 0.1f, 100f);
        _view = Matrix4.LookAt(new Vector3(1.5f, 1.5f, 3f), Vector3.Zero, Vector3.UnitY);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        var input = KeyboardState;

        // Rotación en X (arriba/abajo)
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Up))
            _rotationX += (float)args.Time;
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Down))
            _rotationX -= (float)args.Time;

        // Rotación en Y (izquierda/derecha)
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Right))
            _rotationY += (float)args.Time;
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Left))
            _rotationY -= (float)args.Time;

        // Rotación en Z (teclas Q y E por ejemplo)
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Q))
            _rotationZ += (float)args.Time;
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.E))
            _rotationZ -= (float)args.Time;

        // --- Traslación (mover con WASD) ---
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.W))
            _posY += 0.5f * (float)args.Time;
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.S))
            _posY -= 0.5f * (float)args.Time;
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.A))
            _posX -= 0.5f * (float)args.Time;
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.D))
            _posX += 0.5f * (float)args.Time;

        // --- Escalado ---
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.KeyPadAdd) ||
            input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Equal)) // tecla +
            _scale += 0.5f * (float)args.Time;

        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.KeyPadSubtract) ||
            input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Minus)) // tecla -
            _scale -= 0.5f * (float)args.Time;

        // --- Reflejo en X ---
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.R))
            _reflectionX = true;
        else
            _reflectionX = false;
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        Matrix4 model =
        Matrix4.CreateRotationX(_rotationX) *
        Matrix4.CreateRotationY(_rotationY) *
        Matrix4.CreateRotationZ(_rotationZ)*
        Matrix4.CreateScale(_reflectionX ? -_scale : _scale, _scale, _scale) * // Escala + reflejo
        Matrix4.CreateRotationX(_rotationX) *
        Matrix4.CreateRotationY(_rotationY) *
        Matrix4.CreateTranslation(_posX, _posY, _posZ);
        ;

        Matrix4 mvp = Matrix4.CreateTranslation(0f, 0f, 0f) * model * _view * _projection;

        /*_cara.Draw(mvp);
        _cara1.Draw(mvp);
        _cara2.Draw(mvp);*/

        /*pala.InitGL();
        pala.Draw(mvp);*/

        prueba.InitGL();
        prueba.Draw(mvp);
        SwapBuffers();
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        
        _cara.Dispose();
        _cara1.Dispose();
        _cara2.Dispose();
        objpala.Dispose();
        pala.Dispose();
    }
}