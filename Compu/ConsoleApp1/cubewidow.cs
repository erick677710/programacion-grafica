using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Text.Json;
using System.IO;
using OpenTK.Compute.OpenCL;

public class CubeWindow : GameWindow
{   
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
    //private List<float> _escala = new List<float>();
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
        _cara1 = new Cara(0.5f, .05f, 0.5f, monitor);
        _cara2 = new Cara();


        _cara3 = new Cara(0f, 0f, 0f, teclado);
        _cara4 = new Cara(0f, 0f, 0f, cpu);


        objeto1.Add(_cara);
        objeto1.Add(_cara1);
        objeto1.Add(_cara2);

        objeto2.Add(_cara4);
        objeto2.Add(_cara3);
        //objeto2.Add(_cara);
        parteObj = new Parte(0.5f, 0f, 0f, objeto1);
        parteObj1 = new Parte(0f, 0f, 0f, objeto2);
        //pala = new Parte();
        pala = parteObj;




        ListaParte.Add(parteObj);
        ListaParte.Add(parteObj1);
        prueba = new Objeto(0f,0f,0f, ListaParte);
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

        /*
        string json = JsonSerializer.Serialize(objData, new JsonSerializerOptions { WriteIndented = true });

        // Deserializar
        serializadorObjeto cargado = JsonSerializer.Deserialize<serializadorObjeto>(json);
        var nuevoObjeto = new Objeto();
        nuevoObjeto.LoadFromSerializable(cargado);*/


        //serializadorObjeto objData = prueba.GetSerializable();
        //string json = JsonSerializer.Serialize(prueba);
        //File.WriteAllText(@"C:\programacion grafica\programacion-grafica\Compu\obj.json", json);

        

        // convertirlo a la clase serializable
       

        // opciones de JSON (para que quede bonito)
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        // guardar en un archivo
        string json = JsonSerializer.Serialize(prueba);
        File.WriteAllText(@"C:\programacion grafica\programacion-grafica\Compu\obj.json", json);


        //string json = JsonSerializer.Serialize(objData, new JsonSerializerOptions { WriteIndented = true });
        string jsonleido = File.ReadAllText(@"C:\programacion grafica\programacion-grafica\Compu\obj.json");

        objpala = JsonSerializer.Deserialize<Objeto>(jsonleido);
        //Console.WriteLine(json);
        Console.WriteLine(prueba.ListaPartes[0].ListaCaras[1].X);
        Console.WriteLine(prueba.ListaPartes[0].ListaCaras[1].Y);
        Console.WriteLine(prueba.ListaPartes[0].ListaCaras[1].Z);

        Console.WriteLine(objpala.ListaPartes[0].ListaCaras[1].X);
        Console.WriteLine(objpala.ListaPartes[0].ListaCaras[1].Y);
        Console.WriteLine(objpala.ListaPartes[0].ListaCaras[1].Z);



        objpala.Actualizar();
        _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60f), Size.X / (float)Size.Y, 0.1f, 100f);
        _view = Matrix4.LookAt(new Vector3(1.5f, 1.5f, 3f), Vector3.Zero, Vector3.UnitY);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        var input = KeyboardState;

        // Rotación en X (izquierda/derecha)
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Right))
            objpala.ListaPartes[0].ListaCaras[1].Rotar(0.001f, 0f, 0f);
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Left))
            objpala.ListaPartes[0].ListaCaras[1].Rotar(-0.001f, 0f, 0f);

        // Rotación en Y (arriba/abajo)
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Up))
            objpala.ListaPartes[0].ListaCaras[1].Rotar(0f, 0.001f, 0f);
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Down))
            objpala.ListaPartes[0].ListaCaras[1].Rotar(0f, -0.001f, 0f); ;

        // Rotación en Z (Q/E)
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Q))
            objpala.ListaPartes[0].ListaCaras[1].Rotar(0f, 0f, 0.001f); ;
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.E))
            objpala.ListaPartes[0].ListaCaras[1].Rotar(0f, 0f, -0.001f);


        // trasalacion
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.W))
            objpala.ListaPartes[0].ListaCaras[1].Trasladar(0f, 0.001f, 0f);
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.S))
            objpala.ListaPartes[0].ListaCaras[1].Trasladar(0f, -0.001f, 0f); ;
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.A))
            objpala.ListaPartes[0].ListaCaras[1].Trasladar(0.001f, 0f, 0f);
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.D))
            objpala.ListaPartes[0].ListaCaras[1].Trasladar(-0.001f, 0f, 0f);

        // --- Escalado ---
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.T))
            objpala.ListaPartes[0].ListaCaras[1].Escalar(0.001f, 0.001f, 0.001f);

        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Y))
            objpala.ListaPartes[0].ListaCaras[1].Escalar(-0.001f, -0.001f, -0.001f);

        // reflejo
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.R))
            objpala.ListaPartes[0].ListaCaras[1].ReflejarX();




        /*if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.V))
            prueba.Rotar(0.001f,0f,0f);

        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.C))
            prueba.ListaPartes[0].Rotar(0.001f, 0f, 0f);
        //prueba.RotarXUno(1,0.001f);

        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.X))
            objpala.ListaPartes[0].ListaCaras[1].Rotar(0f, 0.001f, 0f);
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.F))
            objpala.Rotar(0.00f, 0f, 0.001f);
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.G))
            objpala.Rotar(0.00f, 0.001f, 0f);
        if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Z))
            objpala.ListaPartes[0].ListaCaras[1].Escalar(0.01f,0f,0f);*/

    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

       

        Matrix4 mvp =  _view * _projection;





        /*parteObj.InitGL();
        parteObj.Draw(mvp);*/

        /*pala.InitGL();
        pala.Draw(mvp);*/


        objpala.InitGL();
        objpala.Draw(Matrix4.Identity,(_view * _projection));
        /*objpala.InitGL();
        objpala.Draw(mvp);*/
        SwapBuffers();
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        
        _cara.Dispose();
        _cara1.Dispose();
        _cara2.Dispose();
        objpala.Dispose();
        prueba.Dispose();
        pala.Dispose();
    }
}