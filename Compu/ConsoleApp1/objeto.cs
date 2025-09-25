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
    public float rotx { get; set; } = 0;
    public float roty { get; set; } = 0;
    public float rotz { get; set; } = 0;
    public float posx { get; set; } = 0;
    public float posy { get; set; } = 0;
    public float posz { get; set; } = 0;
    public float escx { get; set; } = 1;
    public float escy { get; set; } = 1;
    public float escz { get; set; } = 1;
    public serializadorObjeto GetSerializable()
    {
        var data = new serializadorObjeto
        {
            PX = X,
            PY = Y,
            PZ = Z
        };
        foreach (var parte in ListaPartes)
            data.ListaPartes.Add(parte.GetSerializable());
        return data;
    }

    public void LoadFromSerializable(serializadorObjeto data)
    {
        X = data.PZ;
        Y = data.PY;
        Z = data.PZ;
        ListaPartes.Clear();
        foreach (var parteData in data.ListaPartes)
        {
            var parte = new Parte();
            parte.LoadFromSerializable(parteData);
            ListaPartes.Add(parte);
        }
    }

    

    

    
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
        X = X;
        Y = Y;
        Z = Z;
        ListaPartes = new List<Parte>();
        ModeloDeseoso *= Matrix4.CreateTranslation(X,Y,Z);
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
    public void Draw(Matrix4 ModeloP, Matrix4 VixPro)
    {
        foreach (Parte Parte in ListaPartes)
        {
            Parte.Draw(ModeloDeseoso, VixPro);
        }
    }
    public void Actualizar()
    {
        foreach (Parte Parte in ListaPartes)
        {
            Parte.Actualizar();
        }
    }

    private void ActualizarModelo()
    {
        Matrix4 traslacionOrigen = Matrix4.CreateTranslation(-X, -Y, -Z);

        Matrix4 rotaciones =
            Matrix4.CreateRotationX(rotx) *
            Matrix4.CreateRotationY(roty) *
            Matrix4.CreateRotationZ(rotx);

        Matrix4 traslacionFinal = Matrix4.CreateTranslation(X,Y,Z);

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
    // Liberar memoria
    public void Dispose()
    {
        foreach (Parte Parte in ListaPartes)
        {
            Parte.Dispose();
        }
    }
}

