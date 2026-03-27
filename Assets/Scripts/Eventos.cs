using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Vuforia;

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

    void Start()
    {
        int semilla = (int)(DateTime.Now.Ticks % int.MaxValue);
        rnd = new System.Random(semilla);
        mulan = FindAnyObjectByType<Cambios>();
        SucesosAleatorios();
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
    public TipoSuceso ObtenerSuceso(ObserverBehaviour target)
    {
        if (sucesosPorTarget.ContainsKey(target))
            return sucesosPorTarget[target];

        return TipoSuceso.Inicio;
    }
    public string TargetL(ObserverBehaviour target)
    {
        TipoSuceso evento = ObtenerSuceso(target);

        if (correcta >= orden.Length)
        {
            return "Ya completaste todos los objetivos";
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
                return "Has conseguido la espada. Ahora debes llegar al campamento";
            case TipoSuceso.Campamento:
                mulan.ChangeColor_BTN();
                return "Alcanzaste la flecha y completaste tu entrenamiento";
            case TipoSuceso.Montana:
                mulan.ChangeColor_BTN();
                return "Conseguiste el cañon para vencer al ejercito enemigo";
            case TipoSuceso.Imperio:
                mulan.ChangeColor_BTN();
                return "Encontrate el caballo para regresar a la ciudad imperial";
            case TipoSuceso.Final:
                mulan.ChangeOriginal();
                return "Haz vencido a Shan Yu";
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
                return "ir por el cañon";
            case TipoSuceso.Imperio:
                return "Volver a la ciudad Imperial";
            case TipoSuceso.Final:
                return "Derrotar a Shan Yu";
            default:
                return "No hay suceso encontrado";
        }
    }
}
