using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

class Objeto
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    // Lista de caras que forman esta Objeto
    public List<Parte> ListaPartes { get; set; }
    [JsonIgnore]
    public Matrix4 ModeloDeseoso { get; set; } = Matrix4.Identity;
    [JsonIgnore]
    private Vector3 _rotacion = Vector3.Zero;
    [JsonIgnore]
    private Vector3 _posicion = Vector3.Zero;
    [JsonIgnore]
    private Vector3 _escala = new Vector3(1f, 1f, 1f);
    public serializadorObjeto GetSerializable()
    {
        var data = new serializadorObjeto
        {
            PosX = _posicion.X,
            PosY = _posicion.Y,
            PosZ = _posicion.Z
        };
        foreach (var parte in ListaPartes)
            data.ListaPartes.Add(parte.GetSerializable());
        return data;
    }

    public void LoadFromSerializable(serializadorObjeto data)
    {
        _posicion = new Vector3(data.PosX, data.PosY, data.PosZ);
        ListaPartes.Clear();
        foreach (var parteData in data.ListaPartes)
        {
            var parte = new Parte();
            parte.LoadFromSerializable(parteData);
            ListaPartes.Add(parte);
        }
    }

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
    public void ObjetosPersonalizados(float x, float y, float z)
    {
        for (int i = 0; i < ListaPartes.Count; i++)
        {
            ListaPartes[i].carasperosnalizadas(x, y, z);
        }
    }
    // Constructor vacío (para recibit el json)
    public Objeto()
    {
        X = _posicion.X;
        Y = _posicion.Y;
        Z = _posicion.Z;
        ListaPartes = new List<Parte>();
        ModeloDeseoso *= Matrix4.CreateTranslation(_posicion.X, _posicion.Y, _posicion.Z);
    }

    // Constructor normal
    public Objeto(float x, float y, float z, List<Parte> caras)
    {
        X = x;
        Y = y;
        Z = z;
        ListaPartes = caras ?? new List<Parte>();
        ObjetosPersonalizados(x, y, z);
        ModeloDeseoso *= Matrix4.CreateTranslation(x, y, z);
        InitGL();
    }

    // la magia de open gl
    public void InitGL()
    {
        foreach (Parte Parte in ListaPartes)
        {
            Parte.InitGL();
        }
    }

    // Dibujar la Objeto (dibuja todas sus partes)
    public void Draw(Matrix4 mvp)
    {
        foreach (Parte Parte in ListaPartes)
        {
            Parte.Draw(ModeloDeseoso * mvp);
        }
    }

    private void ActualizarModelo()
    {
        Matrix4 traslacionOrigen = Matrix4.CreateTranslation(-X, -Y, -Z);

        Matrix4 rotaciones =
            Matrix4.CreateRotationX(_rotacion.X) *
            Matrix4.CreateRotationY(_rotacion.Y) *
            Matrix4.CreateRotationZ(_rotacion.Z);

        Matrix4 traslacionFinal = Matrix4.CreateTranslation(_posicion);

        Matrix4 escala = Matrix4.CreateScale(_escala);

        ModeloDeseoso = traslacionOrigen * rotaciones * escala * traslacionFinal;
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
        foreach (Parte Parte in ListaPartes)
        {
            Parte.Dispose();
        }
    }
}

