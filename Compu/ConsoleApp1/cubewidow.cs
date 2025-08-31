using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using ConsoleApp1;

namespace ConsoleApp1
{
    class CubeWindow : GameWindow
    {
        //private Cube _cube;
        private Objeto _teclado;
        private Objeto _monitor;
        private Objeto _cpu;
        private Objeto _objeto;
        private Objeto _objeto1;
        private Matrix4 _projection;
        private Matrix4 _view;
        private float _rotation = 0f;

        public CubeWindow(GameWindowSettings gwSettings, NativeWindowSettings nwSettings)
            : base(gwSettings, nwSettings) { }

        float[] cubo =  {
            // Frente (rojo)
            - 0.1f,  - 0.1f,     0.1f, 0.6f, 0f, 0f,
              0.1f,  - 0.1f,     0.1f, 0.6f, 0f, 0f,
              0.1f,   0.1f,     0.1f, 0.6f, 0f, 0f,
              0.1f,  0.1f,     0.1f, 0.6f, 0f, 0f,
             - 0.1f,   0.1f,     0.1f, 0.6f, 0f, 0f,
             - 0.1f,  - 0.1f,     0.1f, 0.6f, 0f, 0f,

            // Atrás (verde)
            -0.1f, -0.1f,  - 0.1f, 0f, 0.6f, 0f,
              0.1f, -0.1f,  - 0.1f, 0f, 0.6f, 0f,
              0.1f,   0.1f,  - 0.1f, 0f, 0.6f, 0f,
              0.1f,   0.1f,  - 0.1f, 0f, 0.6f, 0f,
            -0.1f,   0.1f,  - 0.1f, 0f, 0.6f, 0f,
            -0.1f,  - 0.1f,  - 0.1f, 0f, 0.6f, 0f,

            // Iquierda (aul)
             - 0.1f,   0.1f,    0.1f, 0f, 0f, 0.6f,
            -0.1f,  0.1f,  - 0.1f, 0f, 0f, 0.6f,
            -0.1f, -0.1f,  - 0.1f, 0f, 0f, 0.6f,
            -0.1f, -0.1f, - 0.1f, 0f, 0f, 0.6f,
            -0.1f,  - 0.1f,    0.1f, 0f, 0f, 0.6f,
            -0.1f,  0.1f,    0.1f, 0f, 0f, 0.6f,
                
            // Derecha (amarillo)
              0.1f,   0.1f,    0.1f, 0.6f, 0.6f, 0f,
              0.1f,   0.1f,  - 0.1f, 0.6f, 0.6f, 0f,
              0.1f, -0.1f,  - 0.1f, 0.6f, 0.6f, 0f,
              0.1f, -0.1f,  - 0.1f, 0.6f, 0.6f, 0f,
              0.1f,  - 0.1f,    0.1f, 0.6f, 0.6f, 0f,
              0.1f,   0.1f,    0.1f, 0.6f, 0.6f, 0f,

            // Arriba (magenta)
             - 0.1f,    0.1f,  - 0.1f, 0.6f, 0f, 0.6f,
              0.1f,    0.1f,  - 0.1f, 0.6f, 0f, 0.6f,
             0.1f,    0.1f,   0.1f, 0.6f, 0f, 0.6f,
              0.1f,    0.1f,   0.1f, 0.6f, 0f, 0.6f,
            -0.1f,    0.1f,   0.1f, 0.6f, 0f, 0.6f,
             - 0.1f,    0.1f,  - 0.1f, 0.6f, 0f, 0.6f,

            // Abajo (cyan)
            -0.1f, -0.1f,  - 0.1f, 0f, 0.6f, 0.6f,
            0.1f, -0.1f,  - 0.1f, 0f, 0.6f, 0.6f,
            0.1f, -0.1f,   0.1f, 0f, 0.6f, 0.6f,
            0.1f, -0.1f,   0.1f, 0f, 0.6f, 0.6f,
            -0.1f, -0.1f,   0.1f, 0f, 0.6f, 0.6f,
            -0.1f, -0.1f,  - 0.1f, 0f, 0.6f, 0.6f
        };
        float[] monitor = {
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
        float[] teclado = {
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
        float[] cpu = {
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
        protected override void OnLoad()
        {
            base.OnLoad();
            GL.Enable(EnableCap.DepthTest);
            //_cube = new Cube()
            //_objeto =  new Objeto(0.5f,0.5f,0.5f, cubo);
            //_objeto1 = new Objeto(0f, 0f, 0f,cubo);
            _teclado = new Objeto(0f, 0f, 0f, teclado);
            _monitor = new Objeto(0f, 0.5f, 0f, monitor);
            _cpu = new Objeto(0f, 0f, 0f, cpu);
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
                _rotation = (float)args.Time;   // Rota a la derecha
            if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Left))
                _rotation = (float)args.Time;   // Rota a la izquierda
            if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Up))
                _rotation += (float)args.Time;  // Rota hacia arriba
            if (input.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Down))
                _rotation -= (float)args.Time;  // Rota hacia abajo
           
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 model = Matrix4.CreateRotationY(_rotation) * Matrix4.CreateRotationX(_rotation);

            Matrix4 mvp1 = Matrix4.CreateTranslation(0f, 0f, 0f) * model * _view * _projection;
            Matrix4 mvp2 = Matrix4.CreateTranslation(0f, 0f, 0f) * model * _view * _projection;
            Matrix4 mvp3 = Matrix4.CreateTranslation(0f, 0f, 0f) * model * _view * _projection;
            //Matrix4 mvp4 = Matrix4.CreateTranslation(0f, 0f, 0f) * model * _view * _projection;

            // Dibujar objetos
            _monitor.Draw(mvp1);
            _cpu.Draw(mvp2);
            _teclado.Draw(mvp3);
            //_cube.Draw(mvp3);

            //_objeto.Draw(mvp1);
            //_objeto1.Draw(mvp2);

            SwapBuffers();
        }
        protected override void OnUnload()
        {
            base.OnUnload();
            _cpu.Dispose();
            _monitor.Dispose();
            _teclado.Dispose();
            //_objeto.Dispose();
            //_objeto1.Dispose();
        }
    }

}
