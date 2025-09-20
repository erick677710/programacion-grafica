public class serializadorCara
{
    public float PosX { get; set; }
    public float PosY { get; set; }
    public float PosZ { get; set; }

    public float RotX { get; set; }
    public float RotY { get; set; }
    public float RotZ { get; set; }

    public List<float> Vertices { get; set; }
}

public class serializadorParte
{
    public float PosX { get; set; }
    public float PosY { get; set; }
    public float PosZ { get; set; }

    public List<serializadorCara> ListaCaras { get; set; } = new List<serializadorCara>();
}

public class serializadorObjeto
{
    public float PosX { get; set; }
    public float PosY { get; set; }
    public float PosZ { get; set; }

    public List<serializadorParte> ListaPartes { get; set; } = new List<serializadorParte>();
}
