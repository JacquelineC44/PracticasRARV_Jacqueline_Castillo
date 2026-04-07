using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Vuforia;
using TMPro;

public class Eventos : MonoBehaviour
{
    public enum TipoSuceso
    {
        Inicio,
        Escape,
        Campamento,
        Montana,
        Imperio,
        Final
    }
    public TipoSuceso[] orden =
    {
        TipoSuceso.Escape,
        TipoSuceso.Campamento,
        TipoSuceso.Montana,
        TipoSuceso.Imperio,
        TipoSuceso.Final
    };
    public ObserverBehaviour[] imageTargets;
    private Dictionary<ObserverBehaviour, TipoSuceso> sucesosPorTarget = new Dictionary<ObserverBehaviour, TipoSuceso>();
    private HashSet<ObserverBehaviour> targetsVisitados = new HashSet<ObserverBehaviour>();
    private int correcta = 0;
    private System.Random rnd;
    private Cambios mulan;
    [SerializeField] private TextMeshProUGUI textoMision;

    [SerializeField] private Secuencias secuenciaEvento;
    [SerializeField] private SecuenciaPorSuceso[] secuencias;
    [SerializeField] private float retrasoSecuencia = 3f;
    private UIManager uiManager;


    void Start()
    {
        int semilla = (int)(DateTime.Now.Ticks % int.MaxValue);
        rnd = new System.Random(semilla);
        uiManager = FindObjectOfType<UIManager>();
        mulan = FindAnyObjectByType<Cambios>();
        SucesosAleatorios();
        ActualizarMision();
    }
    void SucesosAleatorios()
    {
        List<TipoSuceso> sucesosDisponibles = new List<TipoSuceso>
        {
            TipoSuceso.Escape,
            TipoSuceso.Campamento,
            TipoSuceso.Montana,
            TipoSuceso.Imperio,
            TipoSuceso.Final
        };

        for (int i = 0; i < sucesosDisponibles.Count; i++)
        {
            TipoSuceso temp = sucesosDisponibles[i];
            int randomIndex = rnd.Next(i, sucesosDisponibles.Count);
            sucesosDisponibles[i] = sucesosDisponibles[randomIndex];
            sucesosDisponibles[randomIndex] = temp;
        }
        for (int i = 0; i < imageTargets.Length; i++)
        {
            if (i < sucesosDisponibles.Count)
                sucesosPorTarget[imageTargets[i]] = sucesosDisponibles[i];
            else
                sucesosPorTarget[imageTargets[i]] = TipoSuceso.Inicio;
        }
    }
    private void ActualizarMision()
    {
        if (textoMision == null) return;

        if (correcta >= orden.Length)
        {
            textoMision.text = "Misión completada. Ya terminaste todos los objetivos.";
            return;
        }

        TipoSuceso sucesoActual = orden[correcta];
        textoMision.text = ObtenerTextoMision(sucesoActual); ;
    }

    private string ObtenerTextoMision(TipoSuceso suceso)
    {
        switch (suceso)
        {
            case TipoSuceso.Escape:
                return "Colocate en el target d emushu y encuentra la espada para poder huir de casa.";

            case TipoSuceso.Campamento:
                return "Busca el campamento para comenzar tu entrenamiento.";

            case TipoSuceso.Montana:
                return "¡Rápido! Encuentra mushu para encender el cañon y detener al ejército enemigo.";

            case TipoSuceso.Imperio:
                return "Tienes que advertirle a tus compañeros. Encuentra la ciudad imperial ";

            case TipoSuceso.Final:
                return "Han secuestrado al emperador. Llega al enfrentamiento final y derrota a Shan Yu.";

            default:
                return "No hay misión disponible.";
        }
    }
    public TipoSuceso ObtenerSuceso(ObserverBehaviour target)
    {
        if (sucesosPorTarget.ContainsKey(target))
            return sucesosPorTarget[target];

        return TipoSuceso.Inicio;
    }
    private IEnumerator MostrarSecuenciaConRetraso(TipoSuceso suceso)
    {
        yield return new WaitForSeconds(retrasoSecuencia);
        MostrarSecuenciaDeSuceso(suceso);
    }

    public string TargetL(ObserverBehaviour target)
    {
        TipoSuceso evento = ObtenerSuceso(target);

        if (correcta >= orden.Length)
        {
            return "Completaste todos los objetivos";
        }

        TipoSuceso sucesoEsperado = orden[correcta];

        // Si el target ya había sido completado correctamente antes
        if (targetsVisitados.Contains(target))
        {
            return "Ya pasaste por aquí";
        }

        // Si llegó al target correcto en el orden correcto
        if (evento == sucesoEsperado)
        {
            targetsVisitados.Add(target);

            string mensaje = ObtenerMensajeDeExito(evento);

            correcta++;
            ActualizarMision();

            bool juegoTerminado = correcta >= orden.Length;

            if (secuenciaEvento != null)
            {
                secuenciaEvento.ConfigurarMostrarFinAlCerrar(juegoTerminado);
            }

            StartCoroutine(MostrarSecuenciaConRetraso(evento));

            return mensaje;
        }

        // Si llegó a un target fuera de orden
        return "Antes tienes que " + NombreSuceso(sucesoEsperado).ToLower();
    }

    private string ObtenerMensajeDeExito(TipoSuceso suceso)
    {
        switch (suceso)
        {
            case TipoSuceso.Escape:
                mulan.ChangeColor_BTN();
                mulan.ChangeAccHist();
                return "¡Has conseguido la espada!";
            case TipoSuceso.Campamento:
                mulan.ChangeColor_BTN();
                mulan.ChangeAccHist();
                return "Completaste el entrenamiento";
            case TipoSuceso.Montana:
                mulan.ChangeColor_BTN();
                mulan.ChangeAccHist();
                return "Encontraste a mushu. Ahora puedes usar el cañon.";
            case TipoSuceso.Imperio:
                mulan.ChangeColor_BTN();
                mulan.ChangeAccHist();
                return "Encontraste la ciudad imperial";
            case TipoSuceso.Final:
                mulan.ChangeOriginal();
                mulan.ChangeAccHist();
                return "Haz vencido a Shan Yu. Y el emperador te ha obsequiado su medallon para dar honor a tu familia.";
            default:
                return "No encontraste nada";
        }
    }

    private string NombreSuceso(TipoSuceso suceso)
    {
        switch (suceso)
        {
            case TipoSuceso.Escape:
                return "encontrar la espada";
            case TipoSuceso.Campamento:
                return "llegar al campamento";
            case TipoSuceso.Montana:
                return "Busca a Mushu para activar el cañon.";
            case TipoSuceso.Imperio:
                return "Volver a la ciudad Imperial";
            case TipoSuceso.Final:
                return "Derrotar a Shan Yu";
            default:
                return "No hay suceso encontrado";
        }
    }

    private void MostrarSecuenciaDeSuceso(TipoSuceso suceso)
    {
        if (secuenciaEvento == null)
        {
            Debug.LogWarning("No hay referencia a SecuenciaEvento.");
            return;
        }

        if (secuencias == null || secuencias.Length == 0)
        {
            Debug.LogWarning("No hay secuencias configuradas.");
            return;
        }

        foreach (SecuenciaPorSuceso secuencia in secuencias)
        {
            if (secuencia.suceso == suceso)
            {
                if (secuencia.pasos == null || secuencia.pasos.Length == 0)
                {
                    Debug.LogWarning("El suceso " + suceso + " no tiene pasos configurados.");
                    return;
                }

                secuenciaEvento.IniciarSecuencia(secuencia.pasos);
                return;
            }
        }

        Debug.LogWarning("No se encontró secuencia para el suceso: " + suceso);
    }
}
