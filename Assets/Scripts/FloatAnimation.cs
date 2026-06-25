using UnityEngine;

public class FloatAnimation : MonoBehaviour
{
    [Header("Float Settings")]
    [SerializeField] private float speed = 2f;      // Speed of bobbing
    [SerializeField] private float amplitude = 0.25f; // Height range of bobbing

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.localPosition;
    }

    private void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * amplitude;
        transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
    }
}
