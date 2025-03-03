using UnityEngine;

public class ColliderSizeSetter : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public SpriteRenderer spriteRenderer;

    void Update()
    {
        if (spriteRenderer != null && boxCollider != null)
        {
            boxCollider.size = spriteRenderer.bounds.size;
            boxCollider.offset = Vector2.zero;
        }
    }
}