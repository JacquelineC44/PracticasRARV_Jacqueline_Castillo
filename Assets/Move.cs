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


    // Start is called before the first frame update
    void Start()
    {
        if (animator != null)
            animator.SetBool("caminar", false);
        eventosTargets = FindObjectOfType<Eventos>();
        mensajeE = FindObjectOfType<Mensajes>();
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
            isMoving = false;
            yield break;
        }
        Vector3 startPosition = model.transform.position;
        Vector3 endPosition = target.transform.position;

        float journey = 0;

        while (journey <= 1f)
        {
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
        model.transform.SetParent(target.transform, true);
        model.transform.localPosition = Vector3.zero;
        currentTarget = System.Array.IndexOf(ImageTargets, target);
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
    
    private void Update()
    {
    }
}
