using System.Collections;
using UnityEngine;

public class MonitorIntroAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float duration = 2.5f;
    [SerializeField] private Vector3 targetScale = new Vector3(1.5f, 1.5f, 1f);
    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [Header("Callbacks")]
    public UnityEngine.Events.UnityEvent onAnimationComplete;

    private void Start()
    {
        StartCoroutine(AnimateMonitorIn());
    }

    private IEnumerator AnimateMonitorIn()
    {
        // Start very small
        transform.localScale = new Vector3(0.01f, 0.01f, 1f);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float curveValue = scaleCurve.Evaluate(t);
            transform.localScale = Vector3.LerpUnclamped(new Vector3(0.01f, 0.01f, 1f), targetScale, curveValue);
            yield return null;
        }

        transform.localScale = targetScale;
        
        // Notify any listeners
        if (onAnimationComplete != null)
        {
            onAnimationComplete.Invoke();
        }

        // Try to trigger the DebugSmashController minigame automatically
        DebugSmashController controller = GetComponent<DebugSmashController>();
        if (controller != null)
        {
            controller.StartMinigame();
        }
        else
        {
            Debug.LogWarning("MonitorIntroAnimation: DebugSmashController component not found on this GameObject. Minigame was not started automatically.");
        }
    }
}
