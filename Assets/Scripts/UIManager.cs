using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject uiInicio;
    [SerializeField] private GameObject uiIntro;
    [SerializeField] private GameObject uiJuego;
    [SerializeField] private GameObject uiHistoria;
    [SerializeField] private GameObject uiFin;

    private void Start()
    {
        MostrarInicio();
    }

    public void OcultarTodo()
    {
        uiInicio.SetActive(false);
        uiIntro.SetActive(false);
        uiJuego.SetActive(false);
        uiHistoria.SetActive(false);
        uiFin.SetActive(false);
    }

    public void MostrarInicio()
    {
        OcultarTodo();
        uiInicio.SetActive(true);
    }

    public void MostrarIntro()
    {
        OcultarTodo();
        uiIntro.SetActive(true);
    }

    public void MostrarJuego()
    {
        OcultarTodo();
        uiJuego.SetActive(true);
    }
    public void MostrarFin()
    {
        OcultarTodo();
        uiFin.SetActive(true);
    }
}