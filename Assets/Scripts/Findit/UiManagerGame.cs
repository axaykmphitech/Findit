using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.Networking;

public class UiManagerGame : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject gamePanel;
    public GameObject gameOverPanel;
    public GameObject gameWinPanel;
    public GameObject hintPanel;
    public GameObject subscriptionPanel;
    public GameObject termsandConditionsPanel;
    public GameObject privacyPolicyPanel;
    public GameObject levelObject;
    public GameObject hintButton;
    public GameObject leviesObject;

    [Header("Int & Float")]
    public int livesCount = 0;

    [Header("Bool")]
    public bool isSelectOver = false;
    public bool isSelectCameraZoom = false;
    public bool isPanelOpen = false;
    public bool isHintPanel = false;

    [Header("ShareText")]
    public ShareText Sharetext;

    [Header("Text")]
    public TextMeshProUGUI totalStars;

    public static UiManagerGame Instance;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        ActivePanel(gamePanel.name);
        SetStars();
        SetUpLevel();

        livesCount = 3 - ApiDataCall.Instance.toDayPoint;
    }

    IEnumerator LoadImageFromURL(string url)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            levelObject.GetComponent<SpriteRenderer>().sprite = null;
            DialogCanvas.Instance.loadingDialog.SetActive(true);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                Sprite sprite = Texture2DToSprite(texture);
                levelObject.GetComponent<SpriteRenderer>().sprite = sprite;
                DialogCanvas.Instance.loadingDialog.SetActive(false);
            }
            else
            {
                Debug.LogError("POST request failed!");
                Debug.LogError("Error: " + request.error);
                Debug.LogError("Response Code: " + request.responseCode);
                Debug.LogError("Response Text: " + request.downloadHandler.text);

                string Json = request.downloadHandler.text;
                SimpleJSON.JSONNode status = SimpleJSON.JSON.Parse(Json);
                DialogCanvas.Instance.ShowFailedDialog(status["message"]);
            }
        }
    }

    Sprite Texture2DToSprite(Texture2D texture)
    {
        if (texture == null)
        {
            Debug.LogError("Texture is null!");
            return null;
        }

        // Create a new sprite with full texture dimensions
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePanel.activeInHierarchy && !isHintPanel && !gameOverPanel.activeInHierarchy && !gameWinPanel.activeInHierarchy)
            {
                SceneManager.LoadScene(1);
            }
            else if (hintPanel.activeInHierarchy && isHintPanel)
            {
                hintPanel.SetActive(false);
                isHintPanel = false;
                CheckPopUp(true);
            }
            else if (subscriptionPanel.activeInHierarchy)
            {
                ActivePanel(gameOverPanel.name);
                gamePanel.SetActive(true);
            }
            else if (termsandConditionsPanel.activeInHierarchy)
            {
                ActivePanel(subscriptionPanel.name);
            }
            else if (privacyPolicyPanel.activeInHierarchy)
            {
                ActivePanel(subscriptionPanel.name);
            }
        }
    }

    public void TryAgainBtnClick()
    {
        
        if (ApiDataCall.Instance.toDayPoint <= 0)
        {
            if (ApiDataCall.Instance.totalPoint >= 3)
            {
                ApiDataCall.Instance.toDayPoint = 3;
                ApiDataCall.Instance.totalPoint -= 3;
                SceneManager.LoadScene(2);
            }
            else
            {
                SceneManager.LoadScene(1);
            }
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    public void Getmorelivefor10tokensButtonClick()
    {
        ActivePanel(subscriptionPanel.name);
    }

    public void SubscriptionPanelBackButtonClick()
    {
        ActivePanel(gameOverPanel.name);
        gamePanel.SetActive(true);
    }

    public void SubscriptionPanelTermsConditionsButtonClick()
    {
        ActivePanel(termsandConditionsPanel.name);
    }

    public void SubscriptionPanelPrivacyPolicyButtonClick()
    {
        ActivePanel(privacyPolicyPanel.name);
    }

    public void TermsConditionsPanelBackButtonClick()
    {
        ActivePanel(subscriptionPanel.name);
    }

    public void PrivacyPolicyBackButtonClick()
    {
        ActivePanel(subscriptionPanel.name);
    }

    public void SubscriptionPanelSubscribeButtonClick()
    {
        ActivePanel(gameOverPanel.name);
        gamePanel.SetActive(true);
    }

    public void GameWinPlayAgainBtnClick()
    {
        SceneManager.LoadScene(1);
    }

    public void BackBtnClick()
    {
        SceneManager.LoadScene(1);
    }

    public void HintButtonClick()
    {
        CheckPopUp(false);
        hintPanel.SetActive(true);
        isHintPanel = true;
    }

    public void ContinueButtonClick()
    {
        hintPanel.SetActive(false);
        isHintPanel = false;
        CheckPopUp(true);
    }

    public void CheckPopUp(bool open)
    {
        levelObject.transform.GetComponent<BoxCollider2D>().enabled = open;
        levelObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = open;
    }

    public void ShareButtonClick()
    {
        Sharetext.Share("Find IT!" + "\n" + "\n" + "Let me recommend you this game" + "\n" + "\n" + "https://play.google.com/store/games?hl=en");
    }

    public void ActivePanel(string panel)
    {
        gamePanel.SetActive(panel.Equals(gamePanel.name));
        gameOverPanel.SetActive(panel.Equals(gameOverPanel.name));
        gameWinPanel.SetActive(panel.Equals(gameWinPanel.name));
        subscriptionPanel.SetActive(panel.Equals(subscriptionPanel.name));
        privacyPolicyPanel.SetActive(panel.Equals(privacyPolicyPanel.name));
        termsandConditionsPanel.SetActive(panel.Equals(termsandConditionsPanel.name));
    }

    public void SetStars()
    {
        totalStars.text = ApiDataCall.Instance.totalPoint.ToString();
    }

    public void SetUpLevel()
    {
        if(ApiDataCall.Instance.photo != "")
            StartCoroutine(LoadImageFromURL(ApiDataCall.Instance.photo));
        gamePanel.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = ApiDataCall.Instance.title;
        hintPanel.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = ApiDataCall.Instance.hint;

        float x = float.Parse(ApiDataCall.Instance.xPos);
        float y = float.Parse(ApiDataCall.Instance.yPos);
        float xS = float.Parse(ApiDataCall.Instance.xScale);
        float yS = float.Parse(ApiDataCall.Instance.yScale);

        Transform rightAns = levelObject.transform.GetChild(0);
        rightAns.transform.position = new Vector3(x, y, -1);
        rightAns.transform.localScale = new Vector3(xS,yS, 0.2f);
    }
}