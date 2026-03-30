using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
public class Move : MonoBehaviour
{
    public GameObject model;
    public ObserverBehaviour[] ImageTargets;
    public int currentTarget;
    public float speed = 1.0f;
    private bool isMoving = false;

    public Animator animator;
    public float rotationSpeed = 5f;

    private Eventos eventosTargets;
    private Mensajes mensajeE;

    private ObserverBehaviour ultimaTarjetaValida;
    private Vector3 ultimaPosicionLocal = Vector3.zero;
    private Quaternion ultimaRotacionLocal = Quaternion.identity;
    private Vector3 ultimaEscalaLocal = Vector3.one;


    // Start is called before the first frame update
    void Start()
    {
        if (animator != null)
            animator.SetBool("caminar", false);
        eventosTargets = FindObjectOfType<Eventos>();
        mensajeE = FindObjectOfType<Mensajes>();

        foreach (var target in ImageTargets)
        {
            if (target != null)
                target.OnTargetStatusChanged += OnTargetStatusChanged;
        }

        if (ImageTargets != null && currentTarget >= 0 && currentTarget < ImageTargets.Length)
        {
            if (ImageTargets[currentTarget] != null)
            {
                GuardarUltimaTarjeta(ImageTargets[currentTarget]);
            }
        }
    }
    void OnDestroy()
    {
        foreach (var target in ImageTargets)
        {
            if (target != null)
                target.OnTargetStatusChanged -= OnTargetStatusChanged;
        }
    }
    public void moveToNextMarker()
    {
        if (!isMoving) {
            if (animator != null)
                animator.SetBool("caminar", true);
            StartCoroutine(MoveModel());
        }
    }

    private IEnumerator MoveModel() 
    {
        isMoving = true;
        ObserverBehaviour target = GetNextDetectedTarget();
        if (target == null)
        {
            if (animator != null)
                animator.SetBool("caminar", false);

            RestaurarEnUltimaTarjeta();
            isMoving = false;
            yield break;
        }
        Vector3 startPosition = model.transform.position;
        Vector3 endPosition = target.transform.position;

        float journey = 0;

        while (journey <= 1f)
        {
            if (target.TargetStatus.Status == Status.NO_POSE)
            {
                if (animator != null)
                    animator.SetBool("caminar", false);

                RestaurarEnUltimaTarjeta();
                isMoving = false;
                yield break;
            }
            journey += Time.deltaTime * speed;

            Vector3 direction = endPosition - model.transform.position;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                model.transform.rotation = Quaternion.Slerp( model.transform.rotation, targetRotation, 5f * Time.deltaTime);
            }

            model.transform.position = Vector3.Lerp(startPosition, endPosition, journey);
            yield return null;
        }
        model.transform.SetParent(target.transform, false);
        model.transform.localPosition = Vector3.zero;
        model.transform.localRotation = Quaternion.identity;
        currentTarget = System.Array.IndexOf(ImageTargets, target);
        GuardarUltimaTarjeta(target);
        if (eventosTargets != null)
        {
            string mensaje = eventosTargets.TargetL(target);

            Debug.Log(mensaje);

            if (mensajeE != null)
                mensajeE.MostrarMensaje(mensaje);
        }
        if (animator != null)
            animator.SetBool("caminar", false);
        isMoving = false;
    }
    private ObserverBehaviour GetNextDetectedTarget()
    {
        ObserverBehaviour extendedTrackedTarget = null;
        ObserverBehaviour currentExtendedTracked = null;
        ObserverBehaviour currentTracked = null;

        foreach (var target in ImageTargets)
        {
            if (target == null) continue;

            var status = target.TargetStatus.Status;
            bool isCurrent = System.Array.IndexOf(ImageTargets, target) == currentTarget;

            //Prioridad máxima: target nuevo y TRACKED
            if (!isCurrent && status == Status.TRACKED)
            {
                return target;
            }
            // El actual está TRACKED
            if (isCurrent && status == Status.TRACKED)
            {
                currentTracked = target;
            }
            // target nuevo EN EXTENDED_TRACKED
            if (!isCurrent && status == Status.EXTENDED_TRACKED && extendedTrackedTarget == null)
            {
                extendedTrackedTarget = target;
            }
            // Guardar si el actual está EXTENDED_TRACKED
            if (isCurrent && status == Status.EXTENDED_TRACKED)
            {
                currentExtendedTracked = target;
            }
        }
        // Si no hubo uno nuevo TRACKED, usa el actual TRACKED
        if (currentTracked != null)
            return currentTracked;

        // Si no hubo TRACKED, usa uno nuevo EXTENDED_TRACKED
        if (extendedTrackedTarget != null)
            return extendedTrackedTarget;

        // Si no hubo nada mejor, usa el actual EXTENDED_TRACKED
        if (currentExtendedTracked != null)
            return currentExtendedTracked;

        return null;
    }
    private void GuardarUltimaTarjeta(ObserverBehaviour target)
    {
        if (target == null || model == null) return;

        ultimaTarjetaValida = target;
        ultimaPosicionLocal = model.transform.localPosition;
        ultimaRotacionLocal = model.transform.localRotation;
        ultimaEscalaLocal = model.transform.localScale;
    }

    private void RestaurarEnUltimaTarjeta()
    {
        if (ultimaTarjetaValida == null || model == null) return;

        var status = ultimaTarjetaValida.TargetStatus.Status;
        if (status != Status.TRACKED && status != Status.EXTENDED_TRACKED)
            return;

        model.transform.SetParent(ultimaTarjetaValida.transform, false);
        model.transform.localPosition = ultimaPosicionLocal;
        model.transform.localRotation = ultimaRotacionLocal;
        model.transform.localScale = ultimaEscalaLocal;

        currentTarget = System.Array.IndexOf(ImageTargets, ultimaTarjetaValida);
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (behaviour == null || model == null) return;

        bool esTarjetaActual = System.Array.IndexOf(ImageTargets, behaviour) == currentTarget;

        if (esTarjetaActual && status.Status == Status.NO_POSE)
        {
            RestaurarEnUltimaTarjeta();
        }

        if (behaviour == ultimaTarjetaValida &&
            (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED))
        {
            RestaurarEnUltimaTarjeta();
        }
    }
}
