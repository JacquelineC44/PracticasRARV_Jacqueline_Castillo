using System.Collections;
using TMPro;
using UnityEngine;

public class Mensajes : MonoBehaviour
{
    public GameObject panelMensaje;
    public TMP_Text textoMensaje;
    public float duracion = 2f;
    private Coroutine rutinaActual;

    void Start()
    {
        panelMensaje.SetActive(false);
    }

    public void MostrarMensaje(string mensaje)
    {
        if (rutinaActual != null)
            StopCoroutine(rutinaActual);

        rutinaActual = StartCoroutine(MostrarMensajeRutina(mensaje));
    }

    private IEnumerator MostrarMensajeRutina(string mensaje)
    {
        textoMensaje.text = mensaje;
        panelMensaje.SetActive(true);

        yield return new WaitForSeconds(duracion);

        panelMensaje.SetActive(false);
    }
}