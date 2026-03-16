using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MulanCambio : MonoBehaviour
{
    public GameObject[] ropa;
    public GameObject[] accesorios;

    public Material colorMaterial;
    private Color[] ropaOriginal;
    private Color materialOriginal;
    Color color;
    int antacces = -1;
    // Start is called before the first frame update
    void Start()
    {
        ropaOriginal = new Color[ropa.Length];

        for (int i = 0; i < ropa.Length; i++)
        {
            ropaOriginal[i] = ropa[i].GetComponent<Renderer>().material.color;
        }
        materialOriginal = colorMaterial.color;
    }
    public void ChangeColor_BTN()
    {
        
        for (int i = 0; i < ropa.Length; i++)
        {
            color = new Color(Random.value, Random.value, Random.value);
            ropa[i].GetComponent<Renderer>().material.color = color;
        }
        color = new Color(Random.value, Random.value, Random.value);
        colorMaterial.color = color;

    }
    public void ChangeOriginal()
    {
        if (ropa.Length != ropaOriginal.Length)
            return;
        for (int i = 0; i < ropa.Length; i++)
        {
            ropa[i].GetComponent<Renderer>().material.color = ropaOriginal[i];
        }
        colorMaterial.color = materialOriginal;
    }
    public void ChangeAcc()
    {
        if (accesorios.Length == 0) return;

        for (int i = 0; i < accesorios.Length; i++)
        {
            accesorios[i].SetActive(false);
        }
        int acces = Random.Range(0, accesorios.Length);
        do
        {
            acces = Random.Range(0, accesorios.Length);
        } while (acces == antacces);
        antacces = acces;
        accesorios[acces].SetActive(true);
    }
}
