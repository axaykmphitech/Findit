using UnityEngine;

public class DragCornerHandles : MonoBehaviour
{
    [Header("Int & Float")]
    public float scaleSpeed = 0.1f;
    public float minScale = 0.5f;
    public float maxScale = 3f;

    private Vector2 previousTouchDistance;

    public static DragCornerHandles Instance;

    public void Awake()
    {
        Instance = this;
    }
    
    public void Update()
    {
        HandleMouseScroll();
        HandlePinchToZoom();
    }

    public void HandleMouseScroll()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            float scrollAmount = Input.mouseScrollDelta.y * scaleSpeed;
            Vector3 newScale = transform.localScale + Vector3.one * scrollAmount;
            newScale = ClampScale(newScale);
            transform.localScale = newScale;
        }
    }

    public void HandlePinchToZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 currentTouchDistance = touch0.position - touch1.position;

            if (previousTouchDistance == Vector2.zero)
                previousTouchDistance = currentTouchDistance;

            float pinchAmount = (currentTouchDistance.magnitude - previousTouchDistance.magnitude) * scaleSpeed * 0.01f;
            Vector3 newScale = transform.localScale + Vector3.one * pinchAmount;
            newScale = ClampScale(newScale);
            transform.localScale = newScale;
            previousTouchDistance = currentTouchDistance;
        }
        else
        {
            previousTouchDistance = Vector2.zero;
        }
    }

    public Vector3 ClampScale(Vector3 scale)
    {
        scale.x = Mathf.Clamp(scale.x, minScale, maxScale);
        scale.y = Mathf.Clamp(scale.y, minScale, maxScale);
        scale.z = Mathf.Clamp(scale.z, minScale, maxScale);
        return scale;
    }
}