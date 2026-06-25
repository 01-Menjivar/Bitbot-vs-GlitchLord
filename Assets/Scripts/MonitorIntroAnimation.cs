using System.Collections;
using UnityEngine;

/// <summary>
/// MonitorIntroAnimation — Controla la animación de introducción del monitor.
/// Primero muestra un texto de alerta parpadeante y luego escala el monitor de 0 a 1.25.
/// </summary>
public class MonitorIntroAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float duration = 2.0f;
    [SerializeField] private Vector3 targetScale = new Vector3(2.0f, 1.5f, 1f);
    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [Header("Callbacks")]
    public UnityEngine.Events.UnityEvent onAnimationComplete;

    private void Start()
    {
        StartCoroutine(AnimateSequence());
    }

    private IEnumerator AnimateSequence()
    {
        // 1. Ocultar el monitor inicialmente
        transform.localScale = Vector3.zero;

        // 2. Buscar el texto de error "ErrorText" en la escena
        GameObject errorTextObj = GameObject.Find("ErrorText");
        TextMesh textMesh = errorTextObj != null ? errorTextObj.GetComponent<TextMesh>() : null;

        if (errorTextObj != null)
        {
            errorTextObj.SetActive(true);
            MeshRenderer errorMR = errorTextObj.GetComponent<MeshRenderer>();
            if (errorMR != null) errorMR.sortingOrder = 11;
            foreach (Transform child in errorTextObj.transform)
            {
                MeshRenderer mr = child.GetComponent<MeshRenderer>();
                if (mr != null) mr.sortingOrder = 10;
            }
        }

        // 3. Secuencia de parpadeo y cambio de color (Alerta/Error)
        float alertDuration = 2.5f; // Duración total del texto de error
        float elapsedAlert = 0f;
        float blinkInterval = 0.2f; // Velocidad del parpadeo
        float nextBlink = 0f;
        bool isRed = true;
        bool isVisible = true;

        while (elapsedAlert < alertDuration)
        {
            elapsedAlert += Time.deltaTime;

            if (elapsedAlert >= nextBlink)
            {
                nextBlink = elapsedAlert + blinkInterval;

                // Alternar color entre rojo y blanco
                isRed = !isRed;
                if (textMesh != null)
                {
                    textMesh.color = isRed ? Color.red : Color.white;
                }

                // Alternar visibilidad de toda la jerarquía (parpadea texto + borde)
                isVisible = !isVisible;
                if (errorTextObj != null)
                {
                    errorTextObj.SetActive(isVisible);
                }
            }

            yield return null;
        }

        // 4. Eliminar el texto de error una vez terminada la alerta
        if (errorTextObj != null)
        {
            Destroy(errorTextObj);
        }

        // 5. Iniciar la animación de escala del monitor
        float elapsedScale = 0f;
        while (elapsedScale < duration)
        {
            elapsedScale += Time.deltaTime;
            float t = elapsedScale / duration;
            float curveValue = scaleCurve.Evaluate(t);
            transform.localScale = Vector3.LerpUnclamped(new Vector3(0.01f, 0.01f, 1f), targetScale, curveValue);
            yield return null;
        }

        transform.localScale = targetScale;

        // 6. Secuencia de Cuenta Regresiva (3, 2, 1, GO!)
        GameObject countdownObj = null;
        if (transform.parent != null)
        {
            Transform t = transform.parent.Find("CountdownText");
            if (t != null) countdownObj = t.gameObject;
        }
        if (countdownObj == null)
        {
            foreach (GameObject rootGo in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (rootGo.name == "CountdownText")
                {
                    countdownObj = rootGo;
                    break;
                }
                Transform t = rootGo.transform.Find("CountdownText");
                if (t != null)
                {
                    countdownObj = t.gameObject;
                    break;
                }
            }
        }

        if (countdownObj != null)
        {
            countdownObj.SetActive(true);
            MeshRenderer countdownMR = countdownObj.GetComponent<MeshRenderer>();
            if (countdownMR != null) countdownMR.sortingOrder = 11;
            foreach (Transform child in countdownObj.transform)
            {
                MeshRenderer mr = child.GetComponent<MeshRenderer>();
                if (mr != null) mr.sortingOrder = 10;
            }

            TextMesh countdownMesh = countdownObj.GetComponent<TextMesh>();

            // Mostrar "3"
            UpdateTextMeshAndOutlines(countdownObj, countdownMesh, "3");
            yield return new WaitForSeconds(1.0f);

            // Mostrar "2"
            UpdateTextMeshAndOutlines(countdownObj, countdownMesh, "2");
            yield return new WaitForSeconds(1.0f);

            // Mostrar "1"
            UpdateTextMeshAndOutlines(countdownObj, countdownMesh, "1");
            yield return new WaitForSeconds(1.0f);

            // Mostrar "GO!"
            if (countdownMesh != null) countdownMesh.color = Color.white; // Mantener blanco para GO!
            UpdateTextMeshAndOutlines(countdownObj, countdownMesh, "GO!");
            yield return new WaitForSeconds(1.0f);

            // Desaparecer/Destruir el objeto contador
            Destroy(countdownObj);
        }

        // Notificar a los listeners del evento
        if (onAnimationComplete != null)
        {
            onAnimationComplete.Invoke();
        }

        // Disparar el inicio del juego en el DebugSmashController
        DebugSmashController controller = GetComponentInParent<DebugSmashController>();
        if (controller != null)
        {
            controller.StartMinigame();
        }
        else
        {
            Debug.LogWarning("MonitorIntroAnimation: No se encontró DebugSmashController en los padres. El minijuego no se inició automáticamente.");
        }
    }

    private void UpdateTextMeshAndOutlines(GameObject textObj, TextMesh mesh, string newText)
    {
        if (mesh != null)
        {
            mesh.text = newText;
        }
        foreach (Transform child in textObj.transform)
        {
            TextMesh childTM = child.GetComponent<TextMesh>();
            if (childTM != null)
            {
                childTM.text = newText;
                if (mesh != null)
                {
                    // Mantener el color sincronizado si cambia (ej. a verde en GO!)
                    childTM.color = Color.black; // El outline siempre es negro
                }
            }
        }
    }
}
