using System.Collections.Generic;

public class PreguntaAPI
{
    public int idPrompt { get; set; }
    public string prompt { get; set; }
    public List<RespuestaAPI> respuestas { get; set; }
}

public class RespuestaAPI
{
    public string texto { get; set; }
    public bool correcta { get; set; }
}
