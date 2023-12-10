using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textAnimation : MonoBehaviour
{
    public float zoomSpeed = 1.0f;
    public float maxZoom = 2.0f;
    public float minZoom = 0.5f;

    private RectTransform imageRectTransform;
    private bool zoomingIn = true;

    void Start()
    {
        imageRectTransform = GetComponent<RectTransform>();
    }

    void Update()
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
