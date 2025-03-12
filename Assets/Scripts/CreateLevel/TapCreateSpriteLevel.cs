using UnityEngine;

public class TapCreateSpriteLevel : MonoBehaviour
{
    public static TapCreateSpriteLevel Instance;

    [Header("GameObject")]
    public GameObject spritePrefab;
    private GameObject rightAnsSprite;

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

    public float xPos;
    public float yPos;
    public float xScale;
    public float yScale;

    private void Awake()
    {
        Instance = this;
    }

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

        if(rightAnsSprite != null)
        {
            xPos = rightAnsSprite.transform.position.x;
            yPos = rightAnsSprite.transform.position.y;
            xScale = rightAnsSprite.transform.localScale.x;
            yScale = rightAnsSprite.transform.localScale.y;
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
                rightAnsSprite = Instantiate(spritePrefab, mousePosition, Quaternion.identity);
                rightAnsSprite.transform.SetParent(level);
                rightAnsSprite.name = "CreatedSprite";

                if (rightAnsSprite.GetComponent<BoxCollider2D>() == null)
                {
                    rightAnsSprite.AddComponent<BoxCollider2D>();
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