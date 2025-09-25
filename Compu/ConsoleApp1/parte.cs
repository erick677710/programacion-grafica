using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

class Parte
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    // Lista de caras que forman esta parte
    public List<Cara> ListaCaras { get; set; }
    [JsonIgnore]
    public Matrix4 ModeloDeseoso { get; set; } = Matrix4.Identity;
    public float rotx { get; set; } = 0;
    public float roty { get; set; } = 0;
    public float rotz { get; set; } = 0;
    public float posx { get; set; } = 0;
    public float posy { get; set; } = 0;
    public float posz { get; set; } = 0;
    public float escx { get; set; } = 1;
    public float escy { get; set; } = 1;
    public float escz { get; set; } = 1;
    public serializadorParte GetSerializable()
    {
        var data = new serializadorParte
        {
            PX = X,
            PY = Y,
            PZ = Z
        };
        foreach (var cara in ListaCaras)
           data.ListaCaras.Add(cara.GetSerializable());
        return data;
    }

    public void LoadFromSerializable(serializadorParte data)
    {
        X = data.PZ;
        Y = data.PY;
        Z = data.PZ;
        ListaCaras.Clear();
        foreach (var caraData in data.ListaCaras)
        {
            var cara = new Cara();
            cara.LoadFromSerializable(caraData);
            ListaCaras.Add(cara);
        }
    }


    
    public Parte()
    {
        ListaCaras = new List<Cara>();
    }
    public void carasperosnalizadas(float x, float y, float z) {
        for (int i = 0; i < ListaCaras.Count; i++)
        {
            //ListaCaras[i].verticesPersonalizadosvoid(x, y, z);
        }
    }
    // Constructor normal
    public Parte(float x, float y, float z, List<Cara> caras)
    {
        X = x;
        Y = y;
        Z = z;
        ListaCaras = caras ?? new List<Cara>();
        carasperosnalizadas(x, y, z);
        ModeloDeseoso *= Matrix4.CreateTranslation(x, y, z);
        InitGL();
    }

    // la magia de open gl
    public void InitGL()
    {
        foreach (var cara in ListaCaras)
        {
            cara.InitGL();
        }
    }

    // Dibujar la parte (dibuja todas sus caras)
    public void Draw(Matrix4 ModeloP, Matrix4 VixPro)
    {
        foreach (Cara cara in ListaCaras)
        {
            cara.Draw(ModeloDeseoso ,VixPro);
        }
    }


    public void Actualizar()
    {
        foreach (Cara cara in ListaCaras)
        {
            cara.ActualizarModelo();
        }
    }
    private void ActualizarModelo()
    {
        Matrix4 traslacionOrigen = Matrix4.CreateTranslation(-X, -Y, -Z);

        Matrix4 rotaciones =
            Matrix4.CreateRotationX(rotx) *
            Matrix4.CreateRotationY(roty) *
            Matrix4.CreateRotationZ(rotx);

        Matrix4 traslacionFinal = Matrix4.CreateTranslation(X, Y, Z);

        Matrix4 escala = Matrix4.CreateScale(escx, escy, escz);

        ModeloDeseoso = traslacionOrigen * rotaciones * escala * traslacionFinal;
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
        posx = posx + x;
        posy = posy + y;
        posz = posz + z;
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
    public void Dispose()
    {
        foreach (Cara cara in ListaCaras)
        {
            cara.Dispose();
        }
    }
}


