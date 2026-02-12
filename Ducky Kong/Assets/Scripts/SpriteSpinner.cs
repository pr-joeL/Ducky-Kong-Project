using UnityEngine;

public class SpriteSpinner : MonoBehaviour
{
    [Header("Target")]
    public GameObject targetObject;

    [Header("Rotation")]
    public float rotationSpeed = 180f; // degrees per second
    public bool rotateClockwise = true;

    private void Update()
    {
        if (targetObject == null)
            return;

        float direction = rotateClockwise ? -1f : 1f;

        targetObject.transform.Rotate(0f, 0f, direction * rotationSpeed * Time.deltaTime);
    }
}
