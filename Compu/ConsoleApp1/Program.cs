using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using ConsoleApp1;



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
