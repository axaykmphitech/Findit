using UnityEngine;

public class DragSprite : MonoBehaviour
{
    [Header("Dragging")]
    public bool dragging = false;
    private Vector3 offset;

    [Header("Boundary Settings")]
    public Transform boundaryObject;

    private Bounds boundaryBounds;

    void Start()
    {
        boundaryObject = this.gameObject.transform.parent.transform;

        if (boundaryObject != null && boundaryObject.TryGetComponent<SpriteRenderer>(out SpriteRenderer boundarySprite))
        {
            boundaryBounds = boundarySprite.bounds;
        }
    }

    void Update()
    {
        if (dragging)
        {
            Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            newPos.z = 0;

            if (boundaryObject != null)
            {
                float halfWidth = this.gameObject.GetComponent<SpriteRenderer>().bounds.extents.x;
                float halfHeight = this.gameObject.GetComponent<SpriteRenderer>().bounds.extents.y;

                newPos.x = Mathf.Clamp(newPos.x, boundaryBounds.min.x + halfWidth, boundaryBounds.max.x - halfWidth);
                newPos.y = Mathf.Clamp(newPos.y, boundaryBounds.min.y + halfHeight, boundaryBounds.max.y - halfHeight);
            }

            transform.position = newPos;
        }
    }

    void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset.z = 0; 
        dragging = true;
    }

    void OnMouseUp()
    {
        dragging = false;
    }
}
