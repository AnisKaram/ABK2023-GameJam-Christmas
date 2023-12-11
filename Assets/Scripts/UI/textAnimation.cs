using UnityEngine;

public class TextAnimation : MonoBehaviour
{
    private float zoomSpeed = 0.2f;
    private float maxZoom = 1.2f;
    private float minZoom = 1.0f;

    private RectTransform imageRectTransform;
    private bool zoomingIn = true;

    private void Start()
    {
        imageRectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        float scaleFactor = zoomingIn ? (1.0f + Time.deltaTime * zoomSpeed) : (1.0f - Time.deltaTime * zoomSpeed);

        imageRectTransform.localScale *= scaleFactor;
        imageRectTransform.localScale = new Vector3(
            Mathf.Clamp(imageRectTransform.localScale.x, minZoom, maxZoom),
            Mathf.Clamp(imageRectTransform.localScale.y, minZoom, maxZoom),
            1.0f
        );

        if (imageRectTransform.localScale.x >= maxZoom || imageRectTransform.localScale.x <= minZoom)
        {
            zoomingIn = !zoomingIn;
        }
    }
}
