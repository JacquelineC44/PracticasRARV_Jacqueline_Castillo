using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    [Header("UI")]
    public GameObject panelInicio;
    public GameObject panelIntro;
    public Image imagenIntro;
    public TMP_Text textoIntro;

    [Header("Contenido")]
    public Sprite[] imagenes;
    [TextArea(2, 5)]
    public string[] descripciones;

    
    public GameObject hud;

    private int indiceActual = 0;
    private bool introActiva = false;

    void Start()
    {
        panelIntro.SetActive(false);

        if (hud != null)
            hud.SetActive(false);
    }

    void Update()
    {
        if (!introActiva)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            SiguientePantalla();
        }
    }

    public void IniciarIntro()
    {
        if (imagenes.Length == 0 || descripciones.Length == 0)
            return;

        if (panelInicio != null)
            panelInicio.SetActive(false);

        indiceActual = 0;
        introActiva = true;
        panelIntro.SetActive(true);
        MostrarPantallaActual();
    }

    void MostrarPantallaActual()
    {
        if (indiceActual < imagenes.Length)
        {
            imagenIntro.sprite = imagenes[indiceActual];
            imagenIntro.preserveAspect = true;
        }

        if (indiceActual < descripciones.Length)
        {
            textoIntro.text = descripciones[indiceActual];
        }
    }

    void SiguientePantalla()
    {
        indiceActual++;

        if (indiceActual >= imagenes.Length || indiceActual >= descripciones.Length)
        {
            TerminarIntro();
            return;
        }

        MostrarPantallaActual();
    }

    void TerminarIntro()
    {
        introActiva = false;
        panelIntro.SetActive(false);

        if (hud != null)
            hud.SetActive(true);
    }
}