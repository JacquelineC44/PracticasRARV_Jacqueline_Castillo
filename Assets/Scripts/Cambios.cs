using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cambios : MonoBehaviour
{
    
    [System.Serializable]
    public class AccesorioData
    {
        public string nombre;
        public GameObject prefab;
        public Transform anchor;
        public Vector3 localPosition;
        public Vector3 localRotation;
        public Vector3 localScale = Vector3.one;
    }
    public GameObject[] ropa;
    public Material colorMaterial;
    private Color[] ropaOriginal;
    private Color materialOriginal;
    Color color;
    public AccesorioData[] accesorios;
    private GameObject accesorioActual;
    private int index = -1;
    private int antacces = -1;
    // Start is called before the first frame update

    void Start()
    {
        ropaOriginal = new Color[ropa.Length];

        for (int i = 0; i < ropa.Length; i++)
        {
            ropaOriginal[i] = ropa[i].GetComponent<Renderer>().material.color;
        }
        materialOriginal = colorMaterial.color;
        Random.InitState(System.DateTime.Now.GetHashCode());
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
    public void ChangeAccHist()
    {
        if (accesorios == null || accesorios.Length == 0)
            return;

        if (accesorioActual != null)
        {
            Destroy(accesorioActual);
        }

        index++;

        if (index >= accesorios.Length)
        {
            index = 0;
        }

        ColocarAccesorio(index);
    }
    private void ColocarAccesorio(int i)
    {
        if (accesorios[i] == null || accesorios[i].prefab == null || accesorios[i].anchor == null)
        {
            Debug.LogWarning("Accesorio incompleto en índice: " + i);
            return;
        }
        accesorioActual = Instantiate(accesorios[i].prefab, accesorios[i].anchor);
        accesorioActual.transform.localPosition = accesorios[i].localPosition;
        accesorioActual.transform.localRotation = Quaternion.Euler(accesorios[i].localRotation);
        accesorioActual.transform.localScale = accesorios[i].localScale;
    }
}
