using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public GameObject model;    
    public Color color;
    public Material colorMaterial;
    private Color colorOriginal;
    private Color materialOriginal;
    private bool colorCambiado = false;

    // Start is called before the first frame update
    void Start()
    {
        colorOriginal = model.GetComponent<Renderer>().material.color;
        materialOriginal = colorMaterial.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeColor_BTN()
    {
        if (!colorCambiado)
        {
            model.GetComponent<Renderer>().material.color = color;
            colorMaterial.color = color;
            colorCambiado = true;
        }
        else
        {
            model.GetComponent<Renderer>().material.color = colorOriginal;
            colorMaterial.color = materialOriginal;
            colorCambiado = false;
        }
        
    }
}
