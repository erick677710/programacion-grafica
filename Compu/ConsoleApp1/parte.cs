using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

class Parte
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    // Lista de caras que forman esta parte
    public List<Cara> ListaCaras { get; set; }

    // Constructor vacío (para recibit el json)
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
    public void Draw(Matrix4 mvp)
    {
        foreach (Cara cara in ListaCaras)
        {
            cara.Draw(mvp);
        }
    }
    public void RotarX(float X)
    {
        foreach (Cara cara in ListaCaras)
        {
            cara.RotarX(X);
        }
    }
    public void RotarXUno(int n,float X)
    {
        ListaCaras[n].RotarX(X);
        
    }
    // Liberar memoria
    public void Dispose()
    {
        foreach (Cara cara in ListaCaras)
        {
            cara.Dispose();
        }
    }
}


