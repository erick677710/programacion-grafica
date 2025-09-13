using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

class Objeto
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    // Lista de caras que forman esta Objeto
    public List<Parte> ListaPartes { get; set; }


    public void ObjetosPersonalizados(float x, float y, float z)
    {
        for (int i = 0; i < ListaPartes.Count; i++)
        {
            ListaPartes[i].carasperosnalizadas(x, y, z);
        }
    }
    // Constructor vacío (para deserializar)
    public Objeto()
    {
        ListaPartes = new List<Parte>();
    }

    // Constructor normal
    public Objeto(float x, float y, float z, List<Parte> caras)
    {
        X = x;
        Y = y;
        Z = z;
        ListaPartes = caras ?? new List<Parte>();
        ObjetosPersonalizados(x, y, z);
        InitGL();
    }

    // Inicializar OpenGL para todas las caras
    public void InitGL()
    {
        foreach (Parte Parte in ListaPartes)
        {
            Parte.InitGL();
        }
    }

    // Dibujar la Objeto (dibuja todas sus caras)
    public void Draw(Matrix4 mvp)
    {
        foreach (Parte Parte in ListaPartes)
        {
            Parte.Draw(mvp);
        }
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

