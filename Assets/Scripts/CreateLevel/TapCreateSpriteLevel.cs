using UnityEngine;

public class TapCreateSpriteLevel : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject spritePrefab;

    [Header("Bool")]
    public bool isHolding = false;
    public bool isAnsSpriteAvailable;
    public bool isCreated = false;

    [Header("Int & Float")]
    public float holdTime = 0f;
    public float requiredHoldTime = 2f;
    public float doubleClickTime = 0.3f;
    private float lastClickTime;

    [Header("Transform")]
    public Transform level;

    public void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        //    Debug.DrawRay(mousePosition, Vector2.zero, Color.red, 1f);

        //    if (hit.collider != null)
        //    {
        //        float timeSinceLastClick = Time.time - lastClickTime;
        //        if (timeSinceLastClick <= doubleClickTime)
        //        {
        //            CreateSpriteAtPosition();
        //        }
        //        lastClickTime = Time.time;
        //    }
        //}

        if (isHolding)
        {
            holdTime += Time.deltaTime;
            if (holdTime >= requiredHoldTime)
            {
                CreateSpriteAtPosition();
                ResetHold(); 
            }
        }
    }

    public void OnMouseDown()
    {
        isHolding = true;
        holdTime = 0f;
    }

    public void OnMouseUp()
    {
        ResetHold();
    }

    public void ResetHold()
    {
        isHolding = false;
        holdTime = 0f;
    }

    public void CreateSpriteAtPosition()
    {
        if (!isCreated)
        {
            isCreated = true;
            if (spritePrefab != null)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0f;
                GameObject newSprite = Instantiate(spritePrefab, mousePosition, Quaternion.identity);
                newSprite.transform.SetParent(level);
                newSprite.name = "CreatedSprite";

                if (newSprite.GetComponent<BoxCollider2D>() == null)
                {
                    newSprite.AddComponent<BoxCollider2D>();
                }

                Debug.Log("New Sprite Created!");
                isAnsSpriteAvailable = true;
            }
            else
            {
                Debug.LogError("Sprite Prefab is not assigned in the Inspector.");
            }
        }
    }
}