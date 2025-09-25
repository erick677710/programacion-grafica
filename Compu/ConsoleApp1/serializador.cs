public class serializadorCara
{
    public float PX { get; set; }
    public float PY { get; set; }
    public float PZ { get; set; }
    public List<float> Vertices { get; set; }
}

public class serializadorParte
{
    public float PX { get; set; }
    public float PY { get; set; }
    public float PZ { get; set; }

    public List<serializadorCara> ListaCaras { get; set; } = new List<serializadorCara>();
}

public class serializadorObjeto
{
    public float PX { get; set; }
    public float PY { get; set; }
    public float PZ { get; set; }

    public List<serializadorParte> ListaPartes { get; set; } = new List<serializadorParte>();
}
