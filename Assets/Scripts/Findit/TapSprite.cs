using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

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

    private string submitAnswer = "";

    private void Awake()
    {
        Debug.Log("awake");
        Instance = this;
    }

    public void Start()
    {
        submitAnswer = ApiDataCall.Instance.baseUrl + "user/submitAnswer";

        uiManage = UiManagerGame.Instance;
        rightObject = Resources.Load<GameObject>("Right");
        wrongObject = Resources.Load<GameObject>("Wrong");

        SetLives();
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
                        Debug.Log("Left  Object Click");
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
                    StartCoroutine(SubmitAnsRoutine(ApiDataCall.Instance.id, iswon.ToString().ToLower(), (3 - uiManage.livesCount).ToString(), (3 - uiManage.livesCount).ToString(), ApiDataCall.Instance.totalPoint.ToString()));
                }
                lastClickTime = Time.time;
            }
        }
    }

    private void SetLives()
    {
        /// 3 - 000
        /// 2 - 100
        /// 1 - 110
        /// 0 - 111

        if(ApiDataCall.Instance.toDayPoint == 3)
        {

        }
        if(ApiDataCall.Instance.toDayPoint == 2)
        {
            uiManage.leviesObject.transform.GetChild(0).transform.GetComponent<Image>().color = new Color32(255, 255, 255, 150);
        }
        if(ApiDataCall.Instance.toDayPoint == 1)
        {
            uiManage.leviesObject.transform.GetChild(0).transform.GetComponent<Image>().color = new Color32(255, 255, 255, 150);
            uiManage.leviesObject.transform.GetChild(1).transform.GetComponent<Image>().color = new Color32(255, 255, 255, 150);
        }
        if(ApiDataCall.Instance.toDayPoint == 0)
        {
            uiManage.leviesObject.transform.GetChild(0).transform.GetComponent<Image>().color = new Color32(255, 255, 255, 150);
            uiManage.leviesObject.transform.GetChild(1).transform.GetComponent<Image>().color = new Color32(255, 255, 255, 150);
            uiManage.leviesObject.transform.GetChild(2).transform.GetComponent<Image>().color = new Color32(255, 255, 255, 150);
        }
    }

    public void RightAndWrongObjectCreate(Vector2 pos, GameObject createObject)
    {
        Debug.Log("righand wrong object create");
        GameObject objectCreate = Instantiate(createObject);
        objectCreate.transform.SetParent(answerParent.transform);
        objectCreate.transform.position = pos;
    }

    public IEnumerator LivesCheck()
    {
        Debug.Log("lives check");
        yield return new WaitForSeconds(0.1f);

        if (!iswon && !UiManagerGame.Instance.isSelectCameraZoom)
        {
            if (uiManage.livesCount == 3)
            {
                BoxColliderEnbale();
                uiManage.isSelectOver = true;
                uiManage.gameOverPanel.SetActive(true);
                UiManagerGame.Instance.isSelectCameraZoom = true;///
            }
        }
    }

    public IEnumerator WinCheck()
    {
        Debug.Log("win check");
        yield return new WaitForSeconds(1);

        if (!UiManagerGame.Instance.isSelectCameraZoom)
        {
            uiManage.gameWinPanel.SetActive(true);
            UiManagerGame.Instance.isSelectCameraZoom = true;
        }
    }

    public void BoxColliderEnbale()
    {
        Debug.Log("box collider enable");
        levelObject.transform.GetComponent<BoxCollider2D>().enabled = false;
        levelObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
    }

    public IEnumerator SubmitAnsRoutine(string imageId, string isCorrect, string star, string toDayPoint, string totalPoint)
    {
        WWWForm form = new WWWForm();
        form.AddField("imageId", imageId);
        form.AddField("isCorrect", isCorrect);
        form.AddField("star", star);
        form.AddField("toDayPoint", toDayPoint);
        form.AddField("totalPoint", totalPoint);

        Debug.Log("id " + imageId);
        Debug.Log("iscorrect " + isCorrect);
        Debug.Log("star " + star);
        Debug.Log("todaypoint " + toDayPoint) ;
        Debug.Log("total point " + totalPoint);

        using (UnityWebRequest request = UnityWebRequest.Post(submitAnswer, form))
        {
            request.SetRequestHeader("Authorization", "Bearer " + ApiDataCall.Instance.token);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Sign-Up Successful: " + request.downloadHandler.text);
                ApiDataCall.Instance.toDayPoint = 3 - uiManage.livesCount; 
            }
            else
            {
                Debug.LogError("POST request failed!");
                Debug.LogError("Error: " + request.error);
                Debug.LogError("Response Code: " + request.responseCode);
                Debug.LogError("Response Text: " + request.downloadHandler.text);

                string Json = request.downloadHandler.text;
                SimpleJSON.JSONNode status = SimpleJSON.JSON.Parse(Json) ;
                DialogCanvas.Instance.ShowFailedDialog(status["message"]);
            }
        }
    }
}