using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public static UiManagerGame Instance;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        ActivePanel(gamePanel.name);
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
        SceneManager.LoadScene(2);
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
}