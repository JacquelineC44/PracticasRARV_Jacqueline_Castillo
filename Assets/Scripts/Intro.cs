using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Intro : MonoBehaviour
{
    public TextMeshProUGUI dialogos;
    private UIManager uiManager;
    private string[] lineas =
    {
        "Todo comenzo cuando la guerra se desató en china",
        "Todos los hombres de cada familia tenían que ir a la guerra. Ese era el destino de tu padre al no haber tenido un hijo varón que lo sustituyera.",
        "Solo con verlo, sabías que no sobreviviría.",
        "Una noche antes de su partida, dicutiste con él. El estaba dispuesto a dar su vida.",
        "Fue entonces que lo decidiste. No ibas a permitir que cumpliera ese destino, tomarías su lugar."
    };
    public float textSpeed = 0.1f;
    int index;
    public Image imagenFondo;
    public Sprite[] fondos;
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        dialogos.text = string.Empty;
        StartDialogue();
    }
    public void Siguiente()
    {
        if (dialogos.text == lineas[index])
        {
          NextLine();
        }
        else
        {
          StopAllCoroutines();
          dialogos.text = lineas[index];
        }
        

    }
    public void StartDialogue()
    {
        index = 0;
        CambiarFondo();
        StartCoroutine(WriteLine());
    }
    IEnumerator WriteLine()
    {
        foreach (char letter in lineas[index].ToCharArray())
        {
            dialogos.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

    }
    public void NextLine()
    {
        if(index < lineas.Length - 1)
        {
            index++;
            dialogos.text = string.Empty;
            CambiarFondo();
            StartCoroutine(WriteLine());
        }
        else
        {
            uiManager.MostrarJuego();
        }
    }
    public void CambiarFondo()
    {
        if (imagenFondo != null && fondos != null && index < fondos.Length)
        {
            imagenFondo.sprite = fondos[index];
        }
    }

}