using UnityEngine;

public class ClampChildInsideParent : MonoBehaviour
{
    public Transform parentObject;  // Assign the parent GameObject
    public Transform childObject;   // Assign the child (index 0)

    private Bounds parentBounds;
    private Vector3 parentCenter;
    private Vector3 parentSize;

    void Start()
    {
        if (parentObject.TryGetComponent<SpriteRenderer>(out SpriteRenderer parentSprite))
        {
            parentBounds = parentSprite.bounds;  // Get parent sprite bounds
            parentCenter = parentBounds.center;  // Center of parent
            parentSize = parentBounds.size;      // Size of parent sprite
        }
    }

    void Update()
    {
        if (parentObject.transform.childCount == 1)
        {
            childObject = parentObject.transform.GetChild(0).transform;
        }

        ClampChildPosition();
    }

    void ClampChildPosition()
    {
        //if (parentObject == null || childObject == null) return;

        if (parentObject.transform.childCount == 1)
        {
            Vector3 childPos = childObject.position;

            // Calculate min/max bounds using parent size
            float minX = parentCenter.x - parentSize.x / 2;
            Debug.Log(minX + " minX");
            float maxX = parentCenter.x + parentSize.x / 2;
            Debug.Log(maxX + " maxX");
            float minY = parentCenter.y - parentSize.y / 2;
            Debug.Log(minY + " minY");
            float maxY = parentCenter.y + parentSize.y / 2;
            Debug.Log(maxY + " maxY ");

            // Clamp the child's position within these bounds
            childPos.x = Mathf.Clamp(childPos.x, minX, maxX);
            childPos.y = Mathf.Clamp(childPos.y, minY, maxY);

            childObject.position = childPos;
            Debug.Log("Move " + childPos);
        }
    }
}