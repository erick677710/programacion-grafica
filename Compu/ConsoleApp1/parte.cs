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

    // Constructor vacío (para deserializar)
    public Parte()
    {
        ListaCaras = new List<Cara>();
    }
    public void carasperosnalizadas(float x, float y, float z) {
        for (int i = 0; i < ListaCaras.Count; i++)
        {
            ListaCaras[i].verticesPersonalizadosvoid(x, y, z);
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

    // Inicializar OpenGL para todas las caras
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

    // Liberar memoria
    public void Dispose()
    {
        foreach (Cara cara in ListaCaras)
        {
            cara.Dispose();
        }
    }
}


