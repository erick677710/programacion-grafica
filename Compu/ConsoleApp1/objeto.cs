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
    // Constructor vacío (para recibit el json)
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
            Parte.Draw(mvp);
        }
    }

    public void RotarX(float X)
    {
        foreach (Parte Parte in ListaPartes)
        {
            Parte.RotarX(X);
        }
    }
    public void RotarXUno(int n, float X)
    {
        ListaPartes[n].RotarX(X);

    }
    public void RotarXUnoUno(int n, float X)
    {
        ListaPartes[n].RotarXUno(n,X);

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

