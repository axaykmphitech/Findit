using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TapSprite : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject rightObject;
    public GameObject wrongObject;
    public GameObject levelObject;
    public GameObject answerParent;

    [Header("Bool")]
    public bool iswon = false;

    [Header("Int & Float")]
    public float doubleClickTime = 0.3f;
    private float lastClickTime;

    private UiManagerGame uiManage;

    public static TapSprite Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        uiManage = UiManagerGame.Instance;
        rightObject = Resources.Load<GameObject>("Right");
        wrongObject = Resources.Load<GameObject>("Wrong");
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && !UiManagerGame.Instance.isSelectOver && !iswon)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            Debug.DrawRay(mousePosition, Vector2.zero, Color.red, 1f);

            if (hit.collider != null)
            {
                float timeSinceLastClick = Time.time - lastClickTime;
                if (timeSinceLastClick <= doubleClickTime)
                {
                    Debug.Log("Hit Object " + hit.collider.name);

                    if (hit.collider.name == "RightAns")
                    {
                        Debug.Log("Right Object Click");
                        iswon = true;
                        RightAndWrongObjectCreate(mousePosition, rightObject);
                        BoxColliderEnbale();
                        StartCoroutine(WinCheck());
                    }
                    else
                    {
                        Debug.Log("Wrong Object Click");
                        RightAndWrongObjectCreate(mousePosition, wrongObject);
                    }

                    if (!iswon)
                    {
                        uiManage.livesCount += 1;

                        for (int i = 0; i <= uiManage.leviesObject.transform.childCount; i++)
                        {
                            if (i == uiManage.livesCount)
                            {
                                uiManage.leviesObject.transform.GetChild(i - 1).transform.GetComponent<Image>().color = new Color32(255, 255, 255, 150);
                            }
                        }

                        if (uiManage.livesCount == 2)
                        {
                            uiManage.hintButton.SetActive(true);
                        }
                    }
                    StartCoroutine(LivesCheck());
                }
                lastClickTime = Time.time;
            }
        }
    }

    public void RightAndWrongObjectCreate(Vector2 pos, GameObject createObject)
    {
        GameObject objectCreate = Instantiate(createObject);
        objectCreate.transform.SetParent(answerParent.transform);
        objectCreate.transform.position = pos;
    }

    public IEnumerator LivesCheck()
    {
        yield return new WaitForSeconds(0.1f);

        if (!iswon && !UiManagerGame.Instance.isSelectCameraZoom)
        {
            if (uiManage.livesCount == 3)
            {
                BoxColliderEnbale();
                uiManage.isSelectOver = true;
                uiManage.gameOverPanel.SetActive(true);
                UiManagerGame.Instance.isSelectCameraZoom = true;
            }
        }
    }

    public IEnumerator WinCheck()
    {
        yield return new WaitForSeconds(1);

        if (!UiManagerGame.Instance.isSelectCameraZoom)
        {
            uiManage.gameWinPanel.SetActive(true);
            UiManagerGame.Instance.isSelectCameraZoom = true;
        }
    }

    public void BoxColliderEnbale()
    {
        levelObject.transform.GetComponent<BoxCollider2D>().enabled = false;
        levelObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
    }
}