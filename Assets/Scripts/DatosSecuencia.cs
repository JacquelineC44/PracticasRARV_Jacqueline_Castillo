using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PasoVisual
{
    public Sprite imagen;

    [TextArea(2, 5)]
    public string mensaje;
}

[System.Serializable]
public class SecuenciaPorSuceso
{
    public Eventos.TipoSuceso suceso;
    public PasoVisual[] pasos;
}
