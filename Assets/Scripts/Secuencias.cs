using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Secuencias : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Image imagenUI;
    [SerializeField] private TextMeshProUGUI textoUI;
    [SerializeField] private float textSpeed = 0.05f;

    private PasoVisual[] pasosActuales;
    private int index;
    private bool secuenciaActiva;
    private bool mostrarFinAlCerrar;

    public bool SecuenciaActiva => secuenciaActiva;
    private UIManager uiManager;

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    private void Awake()
    {
        secuenciaActiva = false;
        mostrarFinAlCerrar = false;

        if (panel != null)
            panel.SetActive(false);
    }
    public void ConfigurarMostrarFinAlCerrar(bool valor)
    {
        mostrarFinAlCerrar = valor;
    }

    public void SigSuceso()
    {
        if (!secuenciaActiva) return;
        if (textoUI.text == pasosActuales[index].mensaje)
        {
            SiguientePaso();
        }
        else
        {
            StopAllCoroutines();
            textoUI.text = pasosActuales[index].mensaje;
        }

    }


    public void IniciarSecuencia(PasoVisual[] pasos)
    {
        if (pasos == null || pasos.Length == 0)
        {
            Debug.LogWarning("No hay pasos para mostrar en la secuencia.");
            return;
        }

        pasosActuales = pasos;
        index = 0;
        secuenciaActiva = true;

        if (panel != null)
            panel.SetActive(true);

        MostrarPasoActual();
    }

    private void MostrarPasoActual()
    {
        if (imagenUI != null)
            imagenUI.sprite = pasosActuales[index].imagen;

        if (textoUI != null)
            textoUI.text = string.Empty;

        StopAllCoroutines();
        StartCoroutine(EscribirTexto());
    }

    IEnumerator EscribirTexto()
    {
        string mensaje = pasosActuales[index].mensaje;

        foreach (char letra in mensaje)
        {
            textoUI.text += letra;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void SiguientePaso()
    {
        if (index < pasosActuales.Length - 1)
        {
            index++;
            MostrarPasoActual();
        }
        else
        {
            CerrarSecuencia();
        }
    }

    public void CerrarSecuencia()
    {
        StopAllCoroutines();
        secuenciaActiva = false;

        if (panel != null)
            panel.SetActive(false);

        if (mostrarFinAlCerrar)
        {
            mostrarFinAlCerrar = false;
            uiManager.MostrarFin();
        }
        else
        {
            uiManager.MostrarJuego();
        }
    }
}