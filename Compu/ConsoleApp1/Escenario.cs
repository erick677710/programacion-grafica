using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

class Escenario
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    // Lista de caras que forman esta Escenario
    public List<Objeto> ListaObjetos { get; set; }
    
    public Matrix4 ModeloDeseoso { get; set; } = Matrix4.Identity;
    private Vector3 _rotacion = Vector3.Zero;
    private Vector3 _posicion = Vector3.Zero;
    private Vector3 _escala = new Vector3(1f, 1f, 1f);
    public void EscenariosPersonalizados(float x, float y, float z)
    {
        for (int i = 0; i < ListaObjetos.Count; i++)
        {
            //ListaObjetos[i].carasperosnalizadas(x, y, z);
        }
    }
    // Constructor vacío (para recibit el json)
    public Escenario()
    {
        ListaObjetos = new List<Objeto>();
    }

    // Constructor normal
    public Escenario(float x, float y, float z, List<Objeto> caras)
    {
        X = x;
        Y = y;
        Z = z;
        ListaObjetos = caras ?? new List<Objeto>();
        EscenariosPersonalizados(x, y, z);
        ModeloDeseoso *= Matrix4.CreateTranslation(x, y, z);
        InitGL();
    }

    // la magia de open gl
    public void InitGL()
    {
        foreach (Objeto Objeto in ListaObjetos)
        {
            Objeto.InitGL();
        }
    }

    // Dibujar la Escenario (dibuja todas sus Objetos)
    public void Draw(Matrix4 mvp)
    {
        foreach (Objeto Objeto in ListaObjetos)
        {
            Objeto.Draw(ModeloDeseoso * mvp);
        }
    }

    private void ActualizarModelo()
    {
        // Trasladar al origen local (si quieres rotar sobre su centro)
        Matrix4 traslacionOrigen = Matrix4.CreateTranslation(-X, -Y, -Z);

        // Rotaciones
        Matrix4 rotaciones =
            Matrix4.CreateRotationX(_rotacion.X) *
            Matrix4.CreateRotationY(_rotacion.Y) *
            Matrix4.CreateRotationZ(_rotacion.Z);

        // Trasladar a la posición final
        Matrix4 traslacionFinal = Matrix4.CreateTranslation(_posicion);

        traslacionFinal = traslacionFinal * Matrix4.CreateTranslation(_posicion);
        //traslacionOrigen *

        Matrix4 escalado = Matrix4.CreateScale(_escala);
        ModeloDeseoso = traslacionOrigen * rotaciones * escalado * traslacionFinal;
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
    // Liberar memoria
    public void Dispose()
    {
        foreach (Objeto Objeto in ListaObjetos)
        {
            Objeto.Dispose();
        }
    }
}

